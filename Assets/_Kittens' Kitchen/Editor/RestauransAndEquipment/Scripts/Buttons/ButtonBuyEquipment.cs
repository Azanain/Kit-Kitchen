using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ButtonBuyEquipment : MonoBehaviour, IPointerClickHandler
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly EquipmentManager _equipmentManager;

    [SerializeField] private Button _button;
    [SerializeField] private bool _canClick;

    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();
    }
    public void ChangeInteractableButton(bool interactable)
    {
        _button.interactable = interactable;
        _canClick = interactable;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_canClick)
        {
            _equipmentManager.BuyEquipment();
            _eventManager.EquipmentBought();
            _eventManager.ChangeActivePurchasePanel(false);
            _eventManager.ScrollViewPanelUpgradesActive(false);
        }
    }
}
