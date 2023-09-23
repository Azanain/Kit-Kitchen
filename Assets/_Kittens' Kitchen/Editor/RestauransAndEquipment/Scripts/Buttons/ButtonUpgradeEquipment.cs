using UnityEngine.UI;
using UnityEngine;
using Zenject;
using UnityEngine.EventSystems;

public class ButtonUpgradeEquipment : ParentButton, IPointerClickHandler
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly Bank _bank;

    [SerializeField] private Button _button;
    //[SerializeField] private ImprovementsPanelManager _improvementsPanelManager;

    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();

        _eventManager.EquipmentBoughtEvent += UpdateButton;
    }
    private void Start()
    {
        UpdateButton();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPress();
    }
    public override void OnPressButtonTrue()
    {
        _eventManager.ChangeActivePurchasePanel(false);
        _eventManager.ScrollViewPanelUpgradesActive(false);
    }
    public override void OnPressButtonFalse()
    {
        for (int i = 0; i < NewPanels.Length; i++)
            NewPanels[i].SetActive(true);

        _eventManager.ScrollViewPanelUpgradesActive(true);
    }
    private void UpdateButton()
    {
        if (_bank.BoughtEquipmentsInRestaurant.Count > 0)
        {
            canPress = true;
            _button.interactable = true;
        }
        else
        {
            canPress = false;
            _button.interactable = false;
        }
    }
    private void OnDestroy()
    {
        _eventManager.EquipmentBoughtEvent -= UpdateButton;
    }
}
