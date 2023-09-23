using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ButtonBuyUpgradeEquipment : ParentButton, IPointerClickHandler
{
    [Header("ButtonData")]
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly EquipmentManager _equipmentManager;
    [Inject] private readonly Bank _bank;
    //private readonly Checks _checks = new();

    [SerializeField] private Button _button;

    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();

        _eventManager.ChangeValueGoldEvent += CheckActiveButton;
    }
    private void OnEnable()
    {
        CheckActiveButton();
    }
    /// <summary>
    /// Проверяет хватает ли денег, если да, то кнопка активна
    /// </summary>
    /// <param name="value"></param>
    private void CheckActiveButton(float value = 0)
    {
        if (Checks.CheckEnoughGoldForBuyUpdateEquipment(_bank.GetNumberUpgradesEquipmentsInRestaurant(_equipmentManager.InfoRestaurant.name), _bank))
            ChangeActiveButton(true);
        else 
            ChangeActiveButton(false);
    }
    /// <summary>
    /// Изменяет активность кнопки
    /// </summary>
    /// <param name="isActive"></param>
    private void ChangeActiveButton(bool isActive)
    {
        _button.interactable = isActive;
        canPress = isActive;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPress();
    }
    public override void OnPressButtonTrue()
    {
        if (canPress)
        {
            _equipmentManager.BuyUpdateEquipment();
            ChangeActivePanels();
            _eventManager.UpdateTextImprovementsPanel();
        }
    }
    private void OnDestroy()
    {
        _eventManager.ChangeValueGoldEvent -= CheckActiveButton;
    }
}
