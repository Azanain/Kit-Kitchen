using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBoughtEquipment : ParentButton, IPointerDownHandler
{
    [SerializeField] private Image _image;
    private EventManager _eventManager;

    public EquipmentInfo EquipmentInfo { get; private set; }

    [HideInInspector] public UpgradeEquipmentsManager upgradeEquipments;

    private void Awake()
    {
        if (_image == null)
            _image = GetComponent<Image>();
    }
    public void GetEquipmentInfo(EquipmentInfo info, EventManager eventManager)
    {
        EquipmentInfo = info;
        GetSprite(info.Sprite);
        _eventManager = eventManager;
    }
    private void GetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }
    public override void OnPressButtonTrue()
    {
        if (upgradeEquipments.PanelIsOpened)
        {
            Debug.Log("+");
            upgradeEquipments.ChangeActivatePanel();
            _eventManager.ChangeActiveImprovementsPanel(false);
        }
        else
        {
            Debug.Log("-");
            upgradeEquipments.ChangeActivatePanel();
            _eventManager.ChangeActiveImprovementsPanel(true);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress();
    }
}
