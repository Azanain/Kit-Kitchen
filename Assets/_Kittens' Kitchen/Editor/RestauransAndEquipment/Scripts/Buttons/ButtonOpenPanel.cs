using UnityEngine.EventSystems;
public class ButtonOpenPanel : ParentButton, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress();
    }
}

