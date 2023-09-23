using UnityEngine.EventSystems;
using Zenject;
public class ButtonSellRestaurant : ParentButton, IPointerDownHandler
{
    [Inject] private readonly RestaurantsManager _restaurantsManager;
    [Inject] private readonly EventManager _eventManager;

    public override void OnPressButtonTrue()
    {
        _restaurantsManager.SellRestaurant();

        _eventManager.SaleMode(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress();
    }
}

