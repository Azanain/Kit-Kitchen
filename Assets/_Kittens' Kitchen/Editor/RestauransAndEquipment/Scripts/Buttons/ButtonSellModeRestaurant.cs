using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ButtonSellModeRestaurant : MonoBehaviour, IPointerDownHandler
{
    [Inject] private readonly EventManager _eventManager;

    private bool _saleModeIsActive;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_saleModeIsActive == false)
        {
            _saleModeIsActive = true;
            _eventManager.SaleMode(true);
        }
        else
        {
            _saleModeIsActive = false;
            _eventManager.SaleMode(false);
        }
        Debug.Log($"sale mode is {_saleModeIsActive}");
    }
}
