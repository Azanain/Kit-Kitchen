using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiamondCounter : MonoBehaviour
{
    [SerializeField] private int initialDiamonds;
    [SerializeField] private TextMeshProUGUI diamondText;
    private int _currentDiamonds;

    public DiamondCounter(int currentDiamonds)
    {
        _currentDiamonds = currentDiamonds;
    }

    private void Start()
    {
        _currentDiamonds = initialDiamonds;
        UpdateDiamondText();
    }

    public void UpdateDiamondText()
    {
        diamondText.text = _currentDiamonds.ToString();
    }

    public void AddDiamonds(int amount)
    {
        _currentDiamonds += amount;
    }

    public void SubtractDiamonds(int amount)
    {
        _currentDiamonds -= amount;
    }

    public bool CanSpendDiamonds(int amount) => _currentDiamonds >= amount;

    public int GetCurrentDiamonds() => _currentDiamonds;
}
