using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public Action<Vector3, EquipmentPlace> OpenPanelBuyEquipmentEvent;

    public Action<Vector3, RestaurantInfo> OpenPanelBuyRestaurantEvent;

    public Action<ButtonRestaurant> SelectButtonRestaurantEvent;

    public Action<EquipmentInfo, int> AddLevelEquipmentEvent;

    public event Action<bool> SaleModeEvent;

    public Action<bool> ChangeActiveImprovementsPanelEvent;
    public Action<bool> ChangeActivePurchasePanelEvent;
    public Action<bool> ScrollViewUpgradesActiveEvent;

    public Action<float> ChangeValueGoldEvent;

    public Action<int> ChangeValueDimondsEvent;
    public Action<int> ChangeValueCutletsEvent;
    public Action<int> ChangeValueSugarEvent;

    public Action BuyRestaurantEvent;
    public Action UpdateTextCurrenciesEvent;
    public Action UpdateTextIngredientsEvent;
    public Action UpdateTextImprovementsPanelEvent;
    public Action EquipmentBoughtEvent;

    public void OpenPanelBuyEquipment(Vector3 position, EquipmentPlace equipmentPlace)
    { OpenPanelBuyEquipmentEvent?.Invoke(position, equipmentPlace); }
    public void OpenPanelBuyRestaurant(Vector3 position, RestaurantInfo info)
    { OpenPanelBuyRestaurantEvent?.Invoke(position, info); }

    public void AddLevelEquipment(EquipmentInfo info, int level)
    { AddLevelEquipmentEvent?.Invoke(info, level); }

    public void ChangeValueGold(float value)
    {
        ChangeValueGoldEvent?.Invoke(value);
        UpdateTextCurrencies();
    }
    public void BuyRestaurant()
    { BuyRestaurantEvent?.Invoke(); }
    public void SelectButtonRestaurant(ButtonRestaurant buttonRestaurant)
    { SelectButtonRestaurantEvent?.Invoke(buttonRestaurant); }
    public void ChangeValueDimond(int value)
    {
        ChangeValueDimondsEvent?.Invoke(value);
        UpdateTextCurrencies();
    }
    public void ChangeValueCutlets(int value)
    {
        ChangeValueCutletsEvent(value);
        UpdateTextIngredients();
    }
    public void ChangeValueSugar(int value)
    {
        ChangeValueSugarEvent?.Invoke(value);
        UpdateTextIngredients();
    }
    public void UpdateTextCurrencies()
    { UpdateTextCurrenciesEvent?.Invoke(); }
    public void UpdateTextIngredients()
    { UpdateTextIngredientsEvent?.Invoke(); }
    public void EquipmentBought()
    { EquipmentBoughtEvent?.Invoke(); }
    public void SaleMode(bool isActive)
    { SaleModeEvent?.Invoke(isActive); }
    public void ChangeActiveImprovementsPanel(bool isActive)
    { ChangeActiveImprovementsPanelEvent?.Invoke(isActive); }
    public void ChangeActivePurchasePanel(bool isActive)
    { ChangeActivePurchasePanelEvent?.Invoke(isActive); }
    public void ScrollViewPanelUpgradesActive(bool isActive)
    { ScrollViewUpgradesActiveEvent?.Invoke(isActive); }
    public void UpdateTextImprovementsPanel()
    { UpdateTextImprovementsPanelEvent.Invoke(); }
}
