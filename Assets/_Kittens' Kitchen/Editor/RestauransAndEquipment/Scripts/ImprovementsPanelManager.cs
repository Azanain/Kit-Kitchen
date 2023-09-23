using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ImprovementsPanelManager : MonoBehaviour
{
    [Inject] private readonly EquipmentManager _equipmentManager;
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly Bank _bank;

    [SerializeField] private Text _priceText;
    [SerializeField] private Text _sublevel;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _priceProductText;

    public GameObject panel;

    private EquipmentInfo _equipmentInfo => _equipmentManager?.purchasePanel?.SelectedEquipmentPlace?.equipmentInfo;

    private void Awake()
    {
        _eventManager.ChangeActiveImprovementsPanelEvent += ChangeActiveImprovementsPanel;
        _eventManager.UpdateTextImprovementsPanelEvent += CalculateData;
    }
    private void ChangeActiveImprovementsPanel(bool isActive)
    {
        if (_equipmentInfo != null)
        {
            panel.SetActive(isActive);
            CalculateData();
        }
    }
    private void CalculateData()
    {
        float price = EquipmentImprovementSystem.StartingPriceImprovement +
            EquipmentImprovementSystem.StartingPriceImprovement * EquipmentImprovementSystem.PercentIncreasePriceEquipmentAfterImprovement / 100
            * _bank.GetNumberUpgradesEquipmentsInRestaurant(_equipmentManager.InfoRestaurant.name);

        var place = _equipmentManager.purchasePanel.SelectedEquipmentPlace;

        int subLevel = place.Sublevel % 10;
        int level = place.Sublevel / 10;
        level++;

        UpdateText(place, price, subLevel, level);

        _equipmentManager.GetPriceUpdateEquipment(price);
    }
    private void UpdateText(EquipmentPlace place, float price, int sublevel, int level)
    {
        Debug.Log("updat text impropanel");

        _priceText.text = $"{(int)price}";
        _sublevel.text = $"{sublevel}";
        _levelText.text = $"{level}";

        _priceProductText.text = 
            $"{_equipmentInfo.PriceProduct + place.Sublevel * EquipmentImprovementSystem.IncreasePriceProductAfterImprovement} " +
            $"+ {EquipmentImprovementSystem.IncreasePriceProductAfterImprovement}";
    }
    private void OnDestroy()
    {
        _eventManager.ChangeActiveImprovementsPanelEvent -= ChangeActiveImprovementsPanel;
        _eventManager.UpdateTextImprovementsPanelEvent -= CalculateData;
    }
}
