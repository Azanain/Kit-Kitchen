using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ButtonUpgradeRestaurant : ParentButton, IPointerClickHandler
{
    public List<UpgradeRestaurantInfo> upgradeRestaurants = new();
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPress();
    }
    public override void OnPressButtonFalse()
    {
        if (!canPress)
        { 
            //создаёт список возможных улучшений
        }
    }
}
