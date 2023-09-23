using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbstractButton : Button
{
    public Button[] buttonsToDeactivate;

    protected override void Awake()
    {
        base.Awake();

        onClick.AddListener(OnButtonClick);
    }

    protected override void OnDestroy()
    {
        onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        SetOtherButtonsActivity(false);
    }

    private void SetOtherButtonsActivity(bool isActive)
    {
        foreach (Button button in buttonsToDeactivate)
        {
            button.interactable = isActive;
        }
    }
}

