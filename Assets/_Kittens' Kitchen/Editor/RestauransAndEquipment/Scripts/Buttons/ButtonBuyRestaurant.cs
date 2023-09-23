using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ButtonBuyRestaurant : ParentButton, IPointerDownHandler
{
    [Header("ButtonInfo")]
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly Bank _bank;
    //private readonly Checks _checks = new();

    [SerializeField] private Button _button;
    //[SerializeField] private TypeButton _typeButton;

    private float _price;
    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();

        _eventManager.ChangeValueGoldEvent += CheckActiveButton;
    }
    /// <summary>
    /// Проверка достаточности денег на покупку ресторана
    /// </summary>
    /// <param name="value"></param>
    private void CheckActiveButton(float value)
    {
        if (value > 0 && Checks.CheckEnoughGoldForBuyRestaurant(_price, _bank))
        {
            _button.interactable = true;
            canPress = true;
        }
        else if (!Checks.CheckEnoughGoldForBuyRestaurant(_price, _bank))
        {
            _button.interactable = false;
            canPress = false;
        }
    }
    /// <summary>
    /// Проврка активности кнопки по количеству золота
    /// </summary>
    /// <param name="price"></param>
    public void GetPrice(float price)
    {
        _price = price;
        bool isEnough = Checks.CheckEnoughGoldForBuyRestaurant(price, _bank);

        if (isEnough)
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

    public override void OnPressButtonTrue()
    {
        _eventManager.BuyRestaurant();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress();
    }

    private void OnDestroy()
    {
        _eventManager.ChangeValueGoldEvent -= CheckActiveButton;
    }
}
