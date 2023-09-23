using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbstractBar : MonoBehaviour
{
    public Sprite[] BarSprites { get; set; }
    public Image Bar { get; set; }
    private int _currentIndex;

    public void UpdateBar(int currentValue)
    {
        int clampedValue = Mathf.Clamp(currentValue, 0, BarSprites.Length);
        Bar.sprite = BarSprites[clampedValue];
    }
    
    public bool ShouldResetIndex()
    {
        return _currentIndex == BarSprites.Length - 1;
    }

    public void ResetIndex()
    {
        Bar.sprite = BarSprites[0];
    }
}
