using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableOtherButtons : MonoBehaviour
{
    private Button[] allButtons;

    private void OnEnable()
    {
        allButtons = FindObjectsOfType<Button>();

        foreach (Button button in allButtons)
        {
            if (button.gameObject != gameObject && !button.CompareTag("Not Disable"))
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        foreach (Button button in allButtons)
        {
            if (!button.gameObject.activeSelf)
                button.gameObject.SetActive(true);
        }
    }
}
