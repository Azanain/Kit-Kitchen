using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UpgradeWindow : MonoBehaviour
{
    //private readonly Checks _checks = new();
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly Bank _bank;


    //[SerializeField] private GoldCounter goldCounter;
    [SerializeField] private StoreUpgrade storeUpgrade;
    [SerializeField] private TextMeshProUGUI upgradesNeededForLevelUp;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI maxLevel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costUpgradeText;
    [SerializeField] private Button upgradeButton;

    [Header("Options")] 
    [SerializeField] private int maxCountProductByLevel;
    [SerializeField] private int costUpgrade;
    [SerializeField] private float costMultiplier;
    [SerializeField] private bool showDebugParameters;

    //[ShowIf("showDebugParameters")] 
    //[Button("Reset MaxLevel IsReached")]
    public bool ResetMaxLevelIsReached() => _maxLevelIsReached = false;
    
    [Header("Bar")]
    [SerializeField] private Bar upgradeBar;
    [SerializeField] private Sprite[] upgradeBarSprites;

    private int _upgradesRemaining;
    private int _costUpgrade;
    private int _currentCostUpgrade;
    private static bool _maxLevelIsReached;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(OnUpgradeStore);
    }
    
    private void Start()
    {
        _costUpgrade = costUpgrade;
        _currentCostUpgrade = _costUpgrade;
        upgradeBar.BarSprites = upgradeBarSprites;
        _upgradesRemaining = (storeUpgrade.UpgradesNeededForLevelUp - 1) - storeUpgrade.CurrentUpgrade;
        UpdateAll();
    }

    private void OnDestroy()
    {
        upgradeButton.onClick.RemoveListener(OnUpgradeStore);
    }

    private void OnUpgradeStore()
    {
        //goldCounter.SubtractGold(_costUpgrade);
        UpgradeStore();
        AddUpgrade();
        CheckMaxLevelReached();
        if (storeUpgrade.IsMaxLevel() && storeUpgrade.CurrentUpgrade == storeUpgrade.UpgradesNeededForLevelUp)
        {
            storeUpgrade.CurrentLevel = storeUpgrade.MaxLevel;
        }
        else
        {
            storeUpgrade.CheckNextLevel();
        }
        
        UpgradeStore();
        if (storeUpgrade.CurrentUpgrade < storeUpgrade.UpgradesNeededForLevelUp)
        {
            _upgradesRemaining--;
        }

        
        UpdateAll();
        
    }

    private void CheckMaxLevelReached()
    {
        if (storeUpgrade.CurrentUpgrade == storeUpgrade.UpgradesNeededForLevelUp)
        {
            UpdateCostText();
            if (storeUpgrade.IsMaxLevel())
            {
                IncreaseMaxCount();
                _maxLevelIsReached = true;
            }
        }
    }

    private void UpdateUpgradeButtonInteractable()
    {
        bool isMaxLevel = storeUpgrade.CurrentLevel == storeUpgrade.MaxLevel;
        bool isMaxUpgrades = storeUpgrade.CurrentUpgrade == storeUpgrade.UpgradesNeededForLevelUp;

        upgradeButton.interactable = !(_maxLevelIsReached || (isMaxLevel && isMaxUpgrades) || !Checks.CheckEnoughGold(_costUpgrade, _bank)/*goldCounter.CanSpendGold(_costUpgrade)*/);
        // If we're at the max level and this last upgrade, disable the button  
    }

    private void UpgradeStore()
    {
        Debug.Log($"CurrentUpgrade: {storeUpgrade.CurrentUpgrade}, UpgradesNeededForLevelUp: {storeUpgrade.UpgradesNeededForLevelUp}");
        bool isMaxUpgrade = storeUpgrade.CurrentUpgrade == storeUpgrade.UpgradesNeededForLevelUp;
        Debug.Log($"IsMaxUpgrade: {isMaxUpgrade}");
        if (storeUpgrade.CurrentUpgrade == storeUpgrade.UpgradesNeededForLevelUp)
        {
            _costUpgrade = _currentCostUpgrade;
        }
        else
            _costUpgrade = _currentCostUpgrade;
        
        Debug.Log("Cost Upgrade: " + _costUpgrade);
        if (storeUpgrade.CanUpgradeStore())
        {
            storeUpgrade.UpgradeStore();
            if (storeUpgrade.HasLevelIncreased())
            {
                _upgradesRemaining = storeUpgrade.UpgradesNeededForLevelUp - storeUpgrade.CurrentUpgrade;
                
                _costUpgrade += (int)(_currentCostUpgrade * costMultiplier);
                _currentCostUpgrade = _costUpgrade;

                IncreaseMaxCount();
                Debug.Log("max count increased!");
            }
        }
    }

    private void UpdateAll()
    {
        UpdateTextToLevelUp();
        UpdateCostText();
        

        maxLevel.text = "Max level: " + storeUpgrade.MaxLevel;
        currentLevel.text = "Current level: " + storeUpgrade.CurrentLevel;
        UpdateDescription();
        UpdateUpgradeButtonInteractable();
        _eventManager.UpdateTextCurrencies();
        //goldCounter.UpdateGoldText();
        if (!_maxLevelIsReached)
            upgradeBar.UpdateBar(storeUpgrade.CurrentUpgrade);
        else
            upgradeBar.UpdateBar(storeUpgrade.UpgradesNeededForLevelUp);
    }

    private void UpdateCostText() => costUpgradeText.text = _costUpgrade.ToString();
    

    private void UpdateTextToLevelUp()
    {
        if (_upgradesRemaining != 0)
            upgradesNeededForLevelUp.text = "Upgrades to level up: " + _upgradesRemaining;
        else if (!storeUpgrade.IsMaxLevel() || (storeUpgrade.IsMaxLevel() & storeUpgrade.CurrentUpgrade == storeUpgrade.UpgradesNeededForLevelUp - 1))
        {
            upgradesNeededForLevelUp.text = "Do you want to increase level?";
        }
        
        if (_maxLevelIsReached)
            upgradesNeededForLevelUp.text = "It is max level";
    }

    public void UpdateDescription()
    {
        foreach (ProductData productData in storeUpgrade.ProductList)
        {
            int currentMaxCountProduct = productData.MaxCount;
            
            if (!_maxLevelIsReached)
                descriptionText.text = $"Current MaxCount: {currentMaxCountProduct} => {currentMaxCountProduct + maxCountProductByLevel}";
            else
                descriptionText.text = $"Description: Max level is reached!";
        }
    }

    private bool MaxLevelIsUpgraded() => storeUpgrade.IsMaxLevel() && storeUpgrade.CurrentUpgrade == storeUpgrade.UpgradesNeededForLevelUp;

    public void IncreaseMaxCount()
    {
        foreach (ProductData productData in storeUpgrade.ProductList)
        {
            productData.MaxCount += maxCountProductByLevel;
        }
    }

    public void AddUpgrade() => storeUpgrade.AddUpgrade();
    public void RemoveUpgrade() => storeUpgrade.RemoveUpgrade();
    
}
