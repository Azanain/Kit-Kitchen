using UnityEngine.EventSystems;
using Zenject;

public class ButtonGoToRestaurant : ParentButton, IPointerDownHandler
{
    [Inject] private readonly RestaurantsManager _restaurantsManager;
    [Inject] private readonly EquipmentManager _equipmentManager;

    public override void OnPressButtonTrue()
    {
        SendInfoToEquipmentManager();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress();
        //������� �������� � ��������� � �������� � ���������� ������ ���������� ���������
    }
    private void SendInfoToEquipmentManager()
    {
        var info = _restaurantsManager.GetInfoRestaurant();

        _equipmentManager.GetInfoRestaurant(info);
    }
}
