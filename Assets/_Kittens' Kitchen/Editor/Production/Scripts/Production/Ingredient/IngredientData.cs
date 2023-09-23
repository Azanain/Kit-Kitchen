using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "IngredientData", menuName = "Ingredients/Ingredient Data")]
public class IngredientData : ScriptableObject, IObservable
{
    //private readonly Checks _checks = new();

   /* [ShowAssetPreview(32, 32)]*/ public Sprite Icon;
    public string Name;
    public int Cost;
    public int Count;
    [SerializeField] private int buyAmountPerPack;
    public bool IsPurchased { get; set; }
    public int AmountPerPack { get => buyAmountPerPack; set => buyAmountPerPack = value; }
    
    private List<IObserver> _observers = new List<IObserver>();
    
    public bool TryPurchase(/*GoldCounter goldCounter, */IngredientShop shop)
    {
        if (shop.CanPlaceOrder(this))
        {
            if (!IsPurchased /*&& Checks.CheckEnoughGold(Cost)*/ /*goldCounter.CanSpendGold(Cost)*/)
            {
                shop.CheckFreeOrder(Cost);

                return true;
            }
        }

        return false;
    }
    
    public void AddObserver(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }
    
    public void NotifyObservers(IngredientShop shop)
    {
        foreach (var observer in _observers)
        {
            observer.OnObservableUpdate(shop);
        }
    }
}
