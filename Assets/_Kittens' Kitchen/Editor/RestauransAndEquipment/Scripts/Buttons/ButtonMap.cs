using UnityEngine.EventSystems;
using Zenject;

public class ButtonMap : ParentButton, IPointerClickHandler
{
    [Inject] private readonly EventManager _eventManager;
    public void OnPointerClick(PointerEventData eventData)
    {
        _eventManager.ChangeActiveImprovementsPanel(false);
        _eventManager.ChangeActivePurchasePanel(false);
        _eventManager.ScrollViewPanelUpgradesActive(false);

        OnPress();
    }
    public override void OnPressButtonFalse()
    {
        ChangeActivePanels();
    }
}
