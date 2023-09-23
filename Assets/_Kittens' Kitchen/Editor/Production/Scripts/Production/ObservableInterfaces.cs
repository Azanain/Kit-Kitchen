using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IObservable
{
    void AddObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers(IngredientShop shop);
}

public interface IObserver
{
    void OnObservableUpdate(IngredientShop shop);
}