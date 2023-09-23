using UnityEngine;

public class ParentButton : MonoBehaviour
{
    public GameObject[] OldPanels;
    public GameObject[] NewPanels;

    public TypeButton _typeButton;

    public bool changeIndexNewPanels;
    public bool canPress;

    public virtual void ChangeActivePanels(bool changeIndexNewPanels = false)
    {
        for (int i = 0; i < OldPanels.Length; i++)
            OldPanels[i].SetActive(false);

        for (int i = 0; i < NewPanels.Length; i++)
        {
            if (changeIndexNewPanels)
                NewPanels[i].transform.SetSiblingIndex(transform.parent.childCount);

            NewPanels[i].SetActive(true);
        }
    }
    
    public void ChangePanelsValue()
    {
        GameObject[] panels = NewPanels;

        NewPanels = OldPanels;
        OldPanels = panels;
    }
    public virtual void ChangeActivePanel(GameObject newPanel, bool isActive)
    {
        newPanel.SetActive(isActive);
    }
    public void OnPress()
    {
        switch (_typeButton)
        {
            case TypeButton.SwitchPanels:
                if (canPress == false)
                {
                    canPress = true;
                    OnPressButtonTrue();
                    ChangeActivePanels();
                }
                else
                {
                    canPress = false;
                    OnPressButtonFalse();
                    ChangePanelsValue();
                    ChangeActivePanels();
                    ChangePanelsValue();
                }
                break;

            case TypeButton.OpenPanel:
                ChangeActivePanels();
                OnPressButtonTrue();
                break;
        }
    }
    public virtual void OnPressButtonTrue()
    { 
    
    }
    public virtual void OnPressButtonFalse()
    { 
    
    }
}
