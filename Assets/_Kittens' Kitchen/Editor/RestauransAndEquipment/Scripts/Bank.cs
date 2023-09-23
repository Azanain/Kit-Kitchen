using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bank : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly EquipmentManager _equipmentManager;
    [Inject] private readonly RestaurantsManager _restaurantsManager;

    public float Gold { get; private set; }
    public int Dimonds { get; private set; }
    public int Cutlets { get; private set; }
    public int Sugar { get; private set; }

    public bool FreePurchaseSpent { get; private set; }

    /// <summary>
    /// Словарь инвестиций в каждом купленном ресторане
    /// </summary>
    public Dictionary<string, float> RestaurantInvestments { get; private set; } = new();

    /// <summary>
    /// Словарь подуровней в каждом оборудовании
    /// </summary>
    public Dictionary<string, int> SublevelEquipments { get; private set; } = new();

    /// <summary>
    /// Словарь количества улучшений оборудований в каждом купленном ресторане
    /// </summary>
    public Dictionary<string, int> NumberUpgradesEquipmentsInRestaurant { get; private set; } = new();

    /// <summary>
    /// Словарь купленного оборудования в каждом купленном ресторане
    /// </summary>
    public Dictionary<string, List<string>> BoughtEquipmentsInRestaurant { get; private set; } = new();

    private void Awake()
    {
        _eventManager.ChangeValueGoldEvent += ChangeValueGold;
        _eventManager.ChangeValueDimondsEvent += ChangeValueDimonds;
        _eventManager.ChangeValueSugarEvent += ChangeValueSugar;
        _eventManager.ChangeValueCutletsEvent += ChangeValueCutlets;
    }
    private void ChangeValueGold(float value)
    {
        Gold += value;
    }
    private void ChangeValueDimonds(int value)
    {
        Dimonds += value;
    }
    private void ChangeValueCutlets(int value)
    {
        Cutlets += value;
    }
    private void ChangeValueSugar(int value)
    {
        Sugar += value;
    }
    private void OnDestroy()
    {
        _eventManager.ChangeValueGoldEvent -= ChangeValueGold;
        _eventManager.ChangeValueDimondsEvent -= ChangeValueDimonds;
        _eventManager.ChangeValueSugarEvent -= ChangeValueSugar;
        _eventManager.ChangeValueCutletsEvent -= ChangeValueCutlets;
    }


    ///   RestaurantInvestments
    public void RegisterSublevelEquipment(string nameInfo, int sublevel)
    {
        if (SublevelEquipments.ContainsKey(nameInfo))
            SublevelEquipments[nameInfo] = sublevel;
        else
            SublevelEquipments.Add(nameInfo, sublevel);
        
        Debug.Log($"sublevel upgrade equipment = {SublevelEquipments[nameInfo]}");
        //add save
    }
    public void UnRegisterSubLevelEquipment(string nameInfo)
    {
        if (SublevelEquipments.ContainsKey(nameInfo))
            SublevelEquipments.Remove(nameInfo);
        //add save
    }
    public float GetSubLevelequipment(string nameInfo)
    {
        return SublevelEquipments[nameInfo];
    }
    ///  SublevelEquipments
    public void RegisterRestaurantInvestments(string nameInfo, float investments)
    {
        if (RestaurantInvestments.ContainsKey(nameInfo))
        {
            float invert = GetRestaurantInvestments(nameInfo) + investments;
            RestaurantInvestments[nameInfo] = invert;
        }
        else
            RestaurantInvestments.Add(nameInfo, investments);

        Debug.Log($"investments currnt restaurant = {RestaurantInvestments[nameInfo]}");
        //add save
    }
    public void UnRegisterRestaurantInvestments(string nameInfo)
    {
        RestaurantInvestments.Remove(nameInfo);
        //add save
    }
    public float GetRestaurantInvestments(string nameInfo)
    {
        return RestaurantInvestments[nameInfo];
    }
    /// NumberUpgradesEquipmentsInRestaurant
    ///
    public void RegisterNumberUpgradesEquipmentsInRestaurant(string nameInfo, int number = 1)
    {
        if (NumberUpgradesEquipmentsInRestaurant.ContainsKey(nameInfo))
        {
            int num = GetNumberUpgradesEquipmentsInRestaurant(nameInfo) + 1;
            NumberUpgradesEquipmentsInRestaurant[nameInfo] = num;
        }
        else
            NumberUpgradesEquipmentsInRestaurant.Add(nameInfo, number);

        Debug.Log($"number upgrades in current restaurant = {NumberUpgradesEquipmentsInRestaurant[nameInfo]}");
        //add save
    }
    public void UnRegisterNumberUpgradesEquipmentsInRestaurant(string nameInfo)
    {
        if (NumberUpgradesEquipmentsInRestaurant.ContainsKey(nameInfo))
            NumberUpgradesEquipmentsInRestaurant.Remove(nameInfo);
        //add save
    }
    public int GetNumberUpgradesEquipmentsInRestaurant(string nameInfo)
    {
        int number = 0;

        if (NumberUpgradesEquipmentsInRestaurant.ContainsKey(nameInfo))
            number = NumberUpgradesEquipmentsInRestaurant[nameInfo];
        else
            RegisterNumberUpgradesEquipmentsInRestaurant(nameInfo, 0);

        return number;
    }
    /// BoughtEquipmentsInRestaurant
    public void RegisterBoughtEquipmentInRestaurant(string nameRestaurant, string nameEquipment)
    {
        List<string> list;
        if (BoughtEquipmentsInRestaurant.ContainsKey(nameRestaurant))
        {
            list = BoughtEquipmentsInRestaurant[nameRestaurant];
            list.Add(nameEquipment);
            BoughtEquipmentsInRestaurant[nameRestaurant] = list;
        }
        else
        {
            list = new();
            list.Add(nameEquipment);
            BoughtEquipmentsInRestaurant.Add(nameRestaurant, list);
        }

        Debug.Log($"name restaurant = {nameRestaurant}, equip = {nameEquipment}");
        //add save
    }
    public void UnRegisterBoughtEquipmentInRestaursnt(string nameRestaurant)
    {
        if (BoughtEquipmentsInRestaurant.ContainsKey(nameRestaurant))
            BoughtEquipmentsInRestaurant.Remove(nameRestaurant);
        //add save
    }
    public bool CheckBoughtEquipmentInRestaurant(string nameRestaurant, string nameEquipment)
    {
        bool isExist = false;

        if (BoughtEquipmentsInRestaurant.ContainsKey(nameRestaurant))
        {
            List<string> list = BoughtEquipmentsInRestaurant[nameRestaurant];

            if (list.Contains(nameEquipment))
                isExist = true;
        }

        return isExist;
    }
    public int GetNumberBoughtEquipmentInRestaurant(string nameRestaurant)
    {
        int number = 0;

        if (BoughtEquipmentsInRestaurant.ContainsKey(nameRestaurant))
        {
            List<string> list = BoughtEquipmentsInRestaurant[nameRestaurant];

            number = list.Count;
        }

        return number;
    }
}
