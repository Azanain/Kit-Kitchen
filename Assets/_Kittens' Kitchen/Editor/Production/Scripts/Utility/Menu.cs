using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private int defaultLayer;

    private void Start()
    {
        defaultLayer = LayerMask.NameToLayer("Default");
    }
    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        SetBlockingLayer();
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        Time.timeScale = 1;
        RestoreDefaultLayer();
    }
    
    public void ExitApplication()
    {
        Application.Quit();
    }
    
    private void SetBlockingLayer()
    {
        foreach (var obj in FindObjectsOfType<Collider2D>())
        {
            obj.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    private void RestoreDefaultLayer()
    {
        foreach (var obj in FindObjectsOfType<Collider2D>())
        {
            obj.gameObject.layer = defaultLayer;
        }
    }
}
