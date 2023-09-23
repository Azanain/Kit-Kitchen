using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Unity.Plastic.Antlr3.Runtime.Debug;
using UnityEngine;
using Zenject;

public class ProductPurchase : MonoBehaviour
{
    [SerializeField] private ProductData productData;
    [SerializeField] private float timeToResetPurchase = 1f;
    [SerializeField] private float timeToCheckPurchase = 1f;
    private float _timeToReset;
    private float _timeToCheck;
    private bool _isChecked;
    public bool IsPurchased { get; set; }

    [Inject] private ClientSystem _clientSystem;
    private Client _currentClient;

    private void Update()
    {
        if (IsPurchased)
        {
            _timeToReset -= Time.deltaTime;
            if (_timeToReset <= 0)
            {
                IsPurchased = false;
                _timeToReset = timeToResetPurchase;
                Debug.Log("Is purchased: " + IsPurchased, gameObject);
            }
        }

        if (!_isChecked)
        {
            _timeToCheck += Time.timeScale;
            if (_timeToCheck >= timeToCheckPurchase)
            {
                _isChecked = true;
                _timeToCheck = 0;
            }
        }

        if (_isChecked)
        {
            CheckProduct();
            _isChecked = false;
        }
            
    }

    public void CheckProduct()
    {
        float checkRadius = 0.5f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);

        foreach (Collider2D collider in colliders)
        {
            Client newClient = collider.GetComponent<Client>();

            if (newClient != null && !IsPurchased)
            {
                if (newClient.DesiredProduct == productData)
                {
                    if (newClient.DesiredProduct.Count >= newClient.DesiredQuantityProduct)
                    {
                        if (newClient.MoneyToPay >= newClient.DesiredProduct.Cost)
                        {
                            newClient.MoneyToPay -= newClient.DesiredProduct.Cost;
                            newClient.BuyProduct(newClient.DesiredProduct, newClient.DesiredQuantityProduct);
                            IsPurchased = true;
                            Debug.Log("Client has bought product: " + newClient.name, newClient.gameObject);
                        }
                        else
                        {
                            Debug.Log("Client does not have enough money: " + newClient.name, newClient.gameObject);
                        }
                    }
                    else
                    {
                        Debug.Log("Product count is not enough");
                    }
                }
                else
                {
                    Debug.Log("Client is not interested in this product: " + newClient.name, gameObject);
                }
            }
            else
            {
                Debug.Log("Client not found or Product was purchased: " + IsPurchased);
            }
        }
    }

    public ProductData GetProductData() => productData;
}
