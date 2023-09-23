using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ButtonRestaurant : ParentButton, IPointerDownHandler
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly RestaurantsManager _restaurantsManager;

    [SerializeField] private RestaurantInfo _info;

    public RestaurantInfo Info => _info;

    public override void OnPressButtonTrue()
    {
        _eventManager.SelectButtonRestaurant(null);
        
        Vector3 newPositionPanel = new Vector3(transform.localPosition.x, transform.localPosition.y + 125, transform.localPosition.z - 5);
        _eventManager.OpenPanelBuyRestaurantEvent(newPositionPanel, _info);
        _eventManager.SelectButtonRestaurant(this);
       _restaurantsManager.GetInfoToPanel(_info);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress();
    }
}
