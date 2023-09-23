using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class StoreUpgrade : ScriptableObject
{
    [SerializeField] private int currentUpgrade; 
    [SerializeField] private int maxLevel;
    [SerializeField] private int currentLevel;
    [SerializeField] private bool showDebugParameters;
    [Header("Debug Parameters")]
  /*  [ShowIf("showDebugParameters")]*/ [SerializeField] private int _previousLevel;

    [SerializeField] private List<ProductData> products;
    public int UpgradesNeededForLevelUp => _upgradesNeededForLevelUp;
    public int MaxLevel => maxLevel;
    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            _previousLevel = currentLevel;
            currentLevel = value;
        }
    }
    public int CurrentUpgrade => currentUpgrade;
    public List<ProductData> ProductList => products;

    private const int _upgradesNeededForLevelUp = 10;
    //private int _previousLevel;
    
    public void UpgradeStore()
    {
        if (CanUpgradeStore() && currentLevel <= MaxLevel)
        {
            if (currentUpgrade == _upgradesNeededForLevelUp || HasLevelIncreased())
            {
                currentUpgrade = 0;
            }
            
            Debug.Log($"Upgraded store to level {currentLevel}");
            Debug.Log("current upgrade: " + currentUpgrade);
        }
        else
        {
            Debug.Log("Cannot upgrade store at the moment.");
        }
    }

    public void CheckNextLevel()
    {
        if (currentUpgrade == _upgradesNeededForLevelUp)
            currentLevel++;
    }

    public bool HasLevelIncreased()
    {
        return currentLevel > _previousLevel;
    }

    public bool IsMaxLevel()
    {
        return currentLevel == maxLevel;
    }
    
    public void AddUpgrade() => currentUpgrade++;
    
    public void RemoveUpgrade() => currentUpgrade--;

    public bool CanUpgradeStore()
    {
        return currentUpgrade == _upgradesNeededForLevelUp && currentLevel <= maxLevel;
    }
}
