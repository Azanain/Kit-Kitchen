using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class GoldCounter : MonoBehaviour
{
    [SerializeField] private int initialGold;
    [SerializeField] private TextMeshProUGUI goldText;
    //public int CurrentGold { get; set; }
    private event Action OnChangedValue;

    public GoldCounter(int currentGold)
    {
        //CurrentGold = currentGold;
    }

    private void Start()
    {
        //CurrentGold = initialGold;
        OnChangedValue += OnChangeValue;
        UpdateGoldText();
    }

    private void OnChangeValue()
    {
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        //goldText.text = CurrentGold.ToString();
    }

    public void AddGold(int amount)
    {
        //if (CanSpendGold(amount))
            //CurrentGold += amount;
        
        OnChangedValue?.Invoke();
    }

    public void SubtractGold(int amount)
    {
        //if (CanSpendGold(amount))
            //CurrentGold -= amount;
        
        OnChangedValue?.Invoke();
    }

    //public bool CanSpendGold(int amount) => CurrentGold >= amount;

    //public int GetCurrentGold() => CurrentGold;
}
