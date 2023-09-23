using UnityEngine;
public struct Checks
{
    /// <summary>
    /// �������� �� ������� ���������� ���������
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
    /// �������� ������� ������
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
    /// �������� ����������-�� ������
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
    /// ����������� ���� ������������ ��� ����������/��������/�������� ������
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    //public static int CheckTypeEquipment(EquipmentTypes type)
    //{
    //    int level = (int)type;
    //    return level;
    //}
    /// <summary>
    /// �������� �� ������ ������� ������������
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
    /// ��������, ������ �� ����� ��� ������� ������������
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
    /// ��������, ������ �� ����� ��� ������� ��������� ������������
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
    /// �������� �� ������������ ���������� ������������
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
    /// ��������, ���� �� ���� 1 ������ ���� ������������
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
    /// ��������, ������� ������������ �������
    /// </summary>
    /// <returns></returns>
    //public static int CheckIndexBoughtEquipmentPlaces()
    //{
    //    int numberBoughtEquipmentPlaces = _bank.BoughtEquipmentsInRestaurant.Count; 
    //    return numberBoughtEquipmentPlaces;
    //}
    /// <summary>
    /// ��������, ������� ���������� �������
    /// </summary>
    /// <returns></returns>
    public static int CheckIndexBoughtRestaurantPlaces(RestaurantsManager restaurantsManager)
    {
        int numberBoughtRestaurantPlaces = restaurantsManager.BoughtRestaurants.Count;
        return numberBoughtRestaurantPlaces;
    }
    /// <summary>
    /// �������� �� ��������� ������������
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

