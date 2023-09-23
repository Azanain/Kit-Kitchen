using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Client : MonoBehaviour, IClientObservable
{
    [SerializeField] private int moneyToPay;
    [SerializeField] private List<ProductData> productsList;
    public int DesiredQuantityProduct { get; set; }

    public List<ProductData> ProductsList => productsList;
    public ProductData DesiredProduct { get; set; }
    public int MoneyToPay { get => moneyToPay; set => moneyToPay = value; }

    public event Action<ProductData> ProductBought;
    private List<IClientObserver> observers = new List<IClientObserver>();

    public void BuyProduct(ProductData productData, int desiredQuantity)
    {
        ProductBought?.Invoke(productData);
        NotifyObservers(this, productData, desiredQuantity);
    }

    public void SellProduct(int productsSold)
    {
        if (DesiredProduct != null && DesiredProduct.Count > 0)
        {
            if (productsSold > 0)
            {
                Debug.Log($"Client bought {productsSold} {DesiredProduct.Label}");
            }
            else
            {
                Debug.Log("Client tried to buy, but no products were sold.");
            }
        }
        else
        {
            Debug.Log("Client tried to buy, but no desired product is set.");
        }
    }

    public void RegisterObserver(IClientObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void UnregisterObserver(IClientObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers(Client client, ProductData product, int desiredQuantity)
    {
        foreach (var observer in observers)
        {
            observer.OnObservableUpdate(client, product, desiredQuantity);
        }
    }

    public int SetRandomQuantityProduct(int minQuantityProduct, int maxQuantityProduct)
    {
        int randomQuantity = UnityEngine.Random.Range(minQuantityProduct, maxQuantityProduct);
        DesiredQuantityProduct = randomQuantity;
        return DesiredQuantityProduct;
    }
}

[System.Serializable]
public class ProductRequest
{
    [SerializeField] private ProductData productData;
    [SerializeField] private int desiredQuantity;

    public ProductData ProductData { get => productData; set => productData = value; }
    public int DesiredQuantity { get => desiredQuantity; set => desiredQuantity = value; }
}
