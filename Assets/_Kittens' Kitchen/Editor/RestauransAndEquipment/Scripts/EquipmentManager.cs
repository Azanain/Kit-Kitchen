using UnityEngine;
using Zenject;

public class EquipmentManager : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly EquipmentManager _equipmentManager;
    [Inject] private readonly Bank _bank;
    //private readonly Checks _checks = new();

    public PurchasePanel purchasePanel;
  
    [SerializeField] private EquipmentPlacesHolder _equpmentPlacesHolder;
    [SerializeField] private UpgradeEquipmentsManager _upgradeEquipmentsManager;
    public RestaurantInfo InfoRestaurant { get; private set; }

    private float _priceUpgradeEquipment;

    public EventManager GetEventManager()
    {
        return _eventManager;
    }
    /// <summary>
    /// Покупка улучшения выбранного оборудования, с проверкой на достаточность золота
    /// </summary>
    public void BuyUpdateEquipment()
    {
        if (Checks.CheckEnoughGoldForBuyUpdateEquipment(_bank.GetNumberUpgradesEquipmentsInRestaurant(InfoRestaurant.name), _bank))
        {
            _eventManager.ChangeValueGold(-_priceUpgradeEquipment);
            UpgradeEquipment();
            _eventManager.ChangeValueDimond(EquipmentImprovementSystem.DimondsLevelEquipmentReward);
        }
    }
    /// <summary>
    /// Покупка оборудования
    /// </summary>
    public void BuyEquipment()
    {
        Debug.Log($"{Checks.CheckEmptyEquipmentSlot(InfoRestaurant.MaxNumberEquipmentPlaces, InfoRestaurant.name, _bank)} " +
            $" {Checks.CheckEnoughGoldForBuyEquipment(purchasePanel.SelectedEquipmentPlace.equipmentInfo.Price, _bank) }  " +
            $"{Checks.CheckDuplicationBoughtEquipment(_bank, _equipmentManager)}");

        if (Checks.CheckEmptyEquipmentSlot(InfoRestaurant.MaxNumberEquipmentPlaces, InfoRestaurant.name, _bank)
            && Checks.CheckEnoughGoldForBuyEquipment(purchasePanel.SelectedEquipmentPlace.equipmentInfo.Price, _bank) 
            && Checks.CheckDuplicationBoughtEquipment(_bank, _equipmentManager))
        {
            if (!Checks.CheckFirstBuyEquipment(_bank))
            {
                _eventManager.ChangeValueGold(-purchasePanel.SelectedEquipmentPlace.equipmentInfo.Price);
                _bank.RegisterRestaurantInvestments(InfoRestaurant.name, purchasePanel.SelectedEquipmentPlace.equipmentInfo.Price); 
            }

            _bank.RegisterBoughtEquipmentInRestaurant(InfoRestaurant.name, purchasePanel.SelectedEquipmentPlace.equipmentInfo.name);
        }
    }
    /// <summary>
    /// Получение инфы о выбранном ресторане
    /// </summary>
    /// <param name="info"></param>
    public void GetInfoRestaurant(RestaurantInfo info)
    {
        InfoRestaurant = info;
        _equpmentPlacesHolder.CreateEquipmentPlaces(info.EquipmentInfos, info.SpawnPositions);
    }
    /// <summary>
    /// Получение стоимости улучшения выбранного оборудования
    /// </summary>
    /// <param name="price"></param>
    public void GetPriceUpdateEquipment(float price)
    {
        _priceUpgradeEquipment = price;
    }
    public void UpgradeEquipment()
    {
        var place = purchasePanel.SelectedEquipmentPlace;
        place.ChangeLevel(1);//добавил сублевел в выбранное оборудование
        _bank.RegisterSublevelEquipment(place.equipmentInfo.name, place.Sublevel);//сохранил уровень в выбранном оборудовании
        _bank.RegisterRestaurantInvestments(InfoRestaurant.name, _priceUpgradeEquipment);//прибавляет цену улучшения к инвестициям и сохраняет
        _bank.RegisterNumberUpgradesEquipmentsInRestaurant(InfoRestaurant.name);
    }
}
