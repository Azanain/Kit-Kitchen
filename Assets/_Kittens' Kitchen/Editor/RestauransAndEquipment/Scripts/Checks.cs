using UnityEngine;
public struct Checks
{
    /// <summary>
    /// Проверка на покупку выбранного ресторана
    /// </summary>
    /// <returns></returns>
    public static bool CheckRestaurantContains(ButtonRestaurant buttonRestaurant, RestaurantsManager restaurantsManager)
    {
        bool isExist = false;

        if (restaurantsManager.BoughtRestaurants.Contains(buttonRestaurant/*_restaurantsManager.SelectedButtonRestaurant*/))
            isExist = true;

        return isExist;
    }
    /// <summary>
    /// Проверка наличия золота
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool CheckEnoughGold(float value, Bank bank)
    {
        bool isEnough = false;

        if (bank.Gold - value >= 0)
            isEnough = true;

        return isEnough;
    }
    /// <summary>
    /// проверка достаточно-ли золота
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public static bool CheckEnoughGoldForBuyRestaurant(float price, Bank bank)
    {
        bool isEnough = false;

        if (bank.Gold >= price)
            isEnough = true;

        return isEnough;
    }
    /// <summary>
    /// Определение типа оборудования для сохранения/загрузки/передачи данных
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    //public static int CheckTypeEquipment(EquipmentTypes type)
    //{
    //    int level = (int)type;
    //    return level;
    //}
    /// <summary>
    /// Проверка на первую покупку оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckFirstBuyEquipment(Bank bank)
    {
        bool firtTime = true;

        if (bank.BoughtEquipmentsInRestaurant.Count != 0)
            firtTime = false;

        return firtTime;
    }
    /// <summary>
    /// Проверка, хватит ли денег для покупки оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckEnoughGoldForBuyEquipment(float price, Bank bank)
    {
        bool enoughGold = false;

        if (bank.Gold >= price || CheckFirstBuyEquipment(bank))
            enoughGold = true;

        return enoughGold;
    }
    /// <summary>
    /// Проверка, хватит ли денег для покупки улучшения оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckEnoughGoldForBuyUpdateEquipment(int numberUpdate, Bank bank)
    {
        bool enoughGold = false;
        float price = EquipmentImprovementSystem.StartingPriceImprovement + (numberUpdate * EquipmentImprovementSystem.IncreasePriceProductAfterImprovement);

        if (bank.Gold >= price)
            enoughGold = true;

        return enoughGold;
    }
    /// <summary>
    /// Проверка на дублирование купленного оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckDuplicationBoughtEquipment(Bank bank, EquipmentManager equipmentManager)
    {
        bool isDuplicat = false;

        //if (!_equipmentManager.BoughtEquipmentPlaces.Contains(_equipmentManager.purchasePanel.SelectedEquipmentPlace))
        if(!bank.BoughtEquipmentsInRestaurant.ContainsKey(equipmentManager.InfoRestaurant.Name))
            isDuplicat = true;

        return isDuplicat;
    }
    /// <summary>
    /// Проверка, есть ли хоть 1 пустой слот оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckEmptyEquipmentSlot(int maxNumberEquipments, string nameRestaurant, Bank bank)
    {
        bool emptySlotExist = false;

        if (bank.GetNumberBoughtEquipmentInRestaurant(nameRestaurant) < maxNumberEquipments)
            emptySlotExist = true;

        return emptySlotExist;
    }
    /// <summary>
    /// Проверка, сколько оборудования куплено
    /// </summary>
    /// <returns></returns>
    //public static int CheckIndexBoughtEquipmentPlaces()
    //{
    //    int numberBoughtEquipmentPlaces = _bank.BoughtEquipmentsInRestaurant.Count; 
    //    return numberBoughtEquipmentPlaces;
    //}
    /// <summary>
    /// Проверка, сколько ресторанов куплено
    /// </summary>
    /// <returns></returns>
    public static int CheckIndexBoughtRestaurantPlaces(RestaurantsManager restaurantsManager)
    {
        int numberBoughtRestaurantPlaces = restaurantsManager.BoughtRestaurants.Count;
        return numberBoughtRestaurantPlaces;
    }
    /// <summary>
    /// Проверка на купленное оборудование
    /// </summary>
    /// <param name="place"></param>
    /// <returns></returns>
    //public static bool ChechPlaceIsBought()
    //{
    //    bool exist = false;

    //    if (_bank.BoughtEquipmentsInRestaurant.ContainsKey(_equipmentManager.InfoRestaurant.Name))
    //        exist = true;

    //    return exist;
    //}
}

