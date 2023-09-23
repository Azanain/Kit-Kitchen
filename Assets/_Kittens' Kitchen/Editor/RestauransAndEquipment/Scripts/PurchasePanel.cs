using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class PurchasePanel : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManger;
    [Inject] private readonly EquipmentManager _equipmentManager;
    [Inject] private readonly Bank _bank;
    //private readonly Checks _checks = new();

    [SerializeField] private GameObject _panel;
    [SerializeField] private ButtonBuyEquipment _buttonBuyEquipment;
    [SerializeField] private Text _textPrice;
    [SerializeField] private Text _textType;
   
    private bool _panelIsOpened;
    public EquipmentPlace SelectedEquipmentPlace { get; private set; }

    private void Awake()
    {
        //_eventManger.EquipmentBoughtEvent += ClosePanel;
        _eventManger.OpenPanelBuyEquipmentEvent += OpenPanelAndGetData;
        _eventManger.ChangeValueGoldEvent += CheckButtonInteractable;
        _eventManger.ChangeActivePurchasePanelEvent += ChangeActivePanel;
    }
    /// <summary>
    /// Закрытие этой панели при покупке оборудования
    /// </summary>
    //private void ClosePanel()
    //{
    //    ChangeActivePanel(false);
    //}
    /// <summary>
    /// Обновить текст если оборудование купленно
    /// </summary>
    /// <param name="isActive"></param>
    private void UpdateText(bool isActive)
    {
        if (isActive)
        {
            if (Checks.CheckFirstBuyEquipment(_bank) == false)
                _textPrice.text = SelectedEquipmentPlace.equipmentInfo.Price.ToString();
            else
                _textPrice.text = "0";

            _textType.text = SelectedEquipmentPlace.equipmentInfo.EquipmentTypes.ToString();
        }
    }
    /// <summary>
    /// При нажатии на место оборудования открывает панель над ней и выбирает оборудование
    /// </summary>
    /// <param name="position"></param>
    /// <param name="equipmentPlace"></param>
    private void OpenPanelAndGetData(Vector3 position, EquipmentPlace equipmentPlace)
    {
        if (_panel != null)
            _panel.transform.position = position;

        SelectedEquipmentPlace = equipmentPlace;

        CheckBoughSelectedEquipment();
        CheckButtonInteractable();

        if (_panelIsOpened == false)
        {
            _panelIsOpened = true;
            ChangeActivePanel(true);
        }
        else
        {
            _panelIsOpened = false;
            ChangeActivePanel(false);
        }
    }
    /// <summary>
    /// Вызов проверки купленности этого оборудования
    /// </summary>
    private void CheckBoughSelectedEquipment()
    { 
        if(!_bank.CheckBoughtEquipmentInRestaurant(_equipmentManager.InfoRestaurant.name, SelectedEquipmentPlace.equipmentInfo.name))
        {
            UpdateText(true);
            _textPrice.gameObject.SetActive(true);
            _buttonBuyEquipment.gameObject.SetActive(true);
        }
        else                                                            
        {
            UpdateText(false);
            _textPrice.gameObject.SetActive(false);
            _buttonBuyEquipment.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Проверка отображения кнопки покупки выбранного оборудования
    /// </summary>
    private void CheckButtonInteractable(float value = 0)
    {
        if (_equipmentManager.purchasePanel.SelectedEquipmentPlace != null)
        {
            if (Checks.CheckEnoughGoldForBuyEquipment(SelectedEquipmentPlace.equipmentInfo.Price, _bank) || Checks.CheckFirstBuyEquipment(_bank))
                _buttonBuyEquipment.ChangeInteractableButton(true);
            else
                _buttonBuyEquipment.ChangeInteractableButton(false);
        }
    }
    /// <summary>
    /// Переключение активности панели
    /// </summary>
    /// <param name="active"></param>
    private void ChangeActivePanel(bool active)
    {
        _panel.SetActive(active);
    }
    private void OnDestroy()
    {
        //_eventManger.EquipmentBoughtEvent -= ClosePanel;
        _eventManger.OpenPanelBuyEquipmentEvent -= OpenPanelAndGetData;
        _eventManger.ChangeValueGoldEvent -= CheckButtonInteractable;
        _eventManger.ChangeActivePurchasePanelEvent -= ChangeActivePanel;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            _eventManger.ChangeValueGold(5000);
        else if(Input.GetKeyDown(KeyCode.D))
            _eventManger.ChangeValueDimond(50);
    }
#endif
}
