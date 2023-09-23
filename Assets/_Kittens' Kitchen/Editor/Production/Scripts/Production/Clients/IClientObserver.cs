using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClientObservable
{
   public void RegisterObserver(IClientObserver observer);
   public void UnregisterObserver(IClientObserver observer);
   public void NotifyObservers(Client client,ProductData product, int desiredQuantity);
}

public interface IClientObserver
{
   public void OnObservableUpdate(Client client,ProductData product, int desiredQuantity);
}
