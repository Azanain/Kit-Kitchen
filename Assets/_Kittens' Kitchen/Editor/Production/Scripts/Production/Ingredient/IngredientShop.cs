using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class IngredientShop : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly Bank _bank;


    [SerializeField] private IngredientCounters ingredientCounters;
    //[SerializeField] private GoldCounter goldCounter;
    [SerializeField] private TextMeshProUGUI freeOrdersText;
    [SerializeField] private TextMeshProUGUI totalOrdersText;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private List<IngredientData> ingredientItems;

    [Header("Options")] 
    [SerializeField] private int freeOrdersEveryDay = 3;
    [SerializeField] private int maxOrdersAvailable = 10;
    public bool IsMaxIngredients { get; private set; }
    public event Action<IngredientData, int> OnIngredientPurchased;

    private readonly List<IngredientData> _basketContents = new List<IngredientData>();
    private IngredientItem _ingredientItem;
    
    private int _remainingFreeOrders;
    private int _totalOrdersToday;
    

    private void OnEnable()
    {
        InitializeIngredients();
        UpdateBuyButtons();
    }

    private void OnDisable()
    {
        DestroyAllChildren();
        _ingredientItem.OnBuyButtonClicked -= OnBuyIngredientPack;
    }
    
    private void DestroyAllChildren()
    {
        int childCount = content.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    private void InitializeIngredients()
    {
        RefreshIngredientList();

        _remainingFreeOrders = freeOrdersEveryDay;
        _totalOrdersToday = 0;

        freeOrdersText.text = "Free orders: " + _remainingFreeOrders;
        UpdateEventsAndText();
    }
    
    private void RefreshIngredientList()
    {
        foreach (Transform child in content)
        {
            Destroy(child);
        }
        
        foreach (var ingredientData in ingredientItems)
        {
            if (!IsMaxIngredients)
                CreateIngredient(ingredientData);
        }
    }

    private void CreateIngredient(IngredientData ingredientData)
    {
        GameObject shopItem = Instantiate(ingredientPrefab, content);
        IngredientItem ingredientItem = shopItem.GetComponent<IngredientItem>();
        _ingredientItem = ingredientItem;

        if (_ingredientItem != null)
        {
            _ingredientItem.DisplayIngredient(ingredientData);
            _ingredientItem.OnBuyButtonClicked += OnBuyIngredientPack;
        }
    }

    private void OnBuyIngredientPack(IngredientData data)
    {
        BuyIngredientPack(data);
        foreach (var item in ingredientItems)
        {
            item.NotifyObservers(this);
        }
        UpdateEventsAndText();
    }

    private void BuyIngredientPack(IngredientData ingredientData)
    {
        if (ingredientData.TryPurchase(/*goldCounter,*/ this))
        {
            Debug.Log("Purchase successful: 1 pack of " + ingredientData.Name);

            if (_basketContents.Count < maxOrdersAvailable)
            {
                _basketContents.Add(ingredientData);
            }
            else
            {
                IsMaxIngredients = true;
                Debug.Log("Purchase failed: Maximum ingredients reached.");
                return;
            }
            
            OnIngredientPurchased?.Invoke(ingredientData, ingredientData.AmountPerPack);
            //Debug.Log("gold:"  + goldCounter.GetCurrentGold());
        }
        else
        {
            Debug.Log("Purchase failed: Not enough gold for 1 pack of " + ingredientData.Name);
        }
    }

    private void UpdateEventsAndText()
    {
        //goldCounter.UpdateGoldText();
        _eventManager.UpdateTextCurrencies();

        ingredientCounters.UpdateCounterTexts();
        UpdateFreeOrdersText();
        UpdateTotalOrdersText();
        UpdateBuyButtons();
    }

    private void UpdateFreeOrdersText()
    {
        if (_remainingFreeOrders == 0)
            freeOrdersText.text = "Free orders were exhausted!";
        else 
            freeOrdersText.text = "Free orders: " + _remainingFreeOrders;
    }

    private void UpdateTotalOrdersText()
    {
        if (_basketContents.Count < maxOrdersAvailable)
            totalOrdersText.text = "Total orders: " + _totalOrdersToday;
        else
        {
            totalOrdersText.text = "Max of orders is exhausted!";
        }
    }
    
    private void UpdateBuyButtons()
    {
        foreach (IngredientItem item in content.GetComponentsInChildren<IngredientItem>())
        {
            item.SetBuyButtonInteractable(_basketContents.Count < maxOrdersAvailable);
        }
        Debug.Log("max ingredients: " + IsMaxIngredients);
    }

    public void CheckFreeOrder(int nextOrderCost)
    {
        if (_remainingFreeOrders > 0)
        {
            _remainingFreeOrders--;
            Debug.Log("Remaining free orders: " + _remainingFreeOrders);
        }
        else
        {
            Debug.Log("Using gold to pay for the order");
            _eventManager.ChangeValueGold(nextOrderCost);
            //goldCounter.CurrentGold -= nextOrderCost;
            Debug.Log("Available free orders were exhausted!");
        }
        _totalOrdersToday++;

        Debug.Log("total orders: " + _totalOrdersToday);
    }

    public bool CanPlaceOrder(IngredientData data)
    {
        bool hasRemainingAmountFreeOrders = _remainingFreeOrders > 0;
        bool hasEnoughGold = _bank.Gold/*goldCounter.CurrentGold*/ >= data.Cost;
        bool canPlaceOrder = _totalOrdersToday <= maxOrdersAvailable && (hasRemainingAmountFreeOrders || hasEnoughGold);
        
        Debug.Log("Can place order: " + canPlaceOrder);
        Debug.Log("has enough gold: " + hasEnoughGold);

        return canPlaceOrder;
    }

    public int GetPacksInBasket(IngredientData ingredientData)
    {
        int packsInBasket = 0;
    
        foreach (IngredientData ingredient in _basketContents)
        {
            if (ingredient == ingredientData)
            {
                packsInBasket++;
            }
        }
    
        return packsInBasket;
    }

    public List<IngredientData> GetIngredients() => _basketContents;
    
    public IngredientData GetIngredientItem(string ingredientName)
    {
        return ingredientItems.FirstOrDefault(ingredient => ingredient.Name.ToLower() == ingredientName.ToLower());
    }
    
    public List<IngredientData> GetBasketContents() => _basketContents;
    public void ClearBasket() => _basketContents.Clear();
}