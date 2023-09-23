using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientItem : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image  preview;
    [SerializeField] private Button buyButton;
    private IngredientData _ingredientData;
    private IngredientShop _ingredientShop;

    public event Action<IngredientData> OnBuyButtonClicked;

    private void Awake()
    {
        buyButton.onClick.AddListener(OnBuyIngredientPack);
    }

    public void DisplayIngredient(IngredientData ingredientData)
    {
        _ingredientData = ingredientData;

        nameText.text = ingredientData.Name;
        costText.text = "Cost: " + ingredientData.Cost;
        preview.sprite = ingredientData.Icon;

        _ingredientData.AddObserver(this);
        UpdateUIState();
    }

    private void OnDestroy()
    {
        buyButton.onClick.RemoveListener(OnBuyIngredientPack);
        if (_ingredientData != null)
        {
            _ingredientData.RemoveObserver(this);
        }
    }

    public void OnObservableUpdate(IngredientShop ingredientShop)
    {
        _ingredientShop = ingredientShop;
        UpdateUIState();
    }
    
    private void UpdateUIState()
    {
        if (_ingredientShop == null)
            return;
        
        if (_ingredientShop.IsMaxIngredients)
        {
            SetIngredientPurchased();
        }
        else
        {
            SetIngredientAvailable();
        }
    }

    private void SetIngredientPurchased()
    {
        costText.text = "Exhausted";
        buyButton.interactable = false;
    }

    private void SetIngredientAvailable()
    {
        costText.text = "Cost: " + _ingredientData.Cost;
        buyButton.interactable = true;
    }

    private void OnBuyIngredientPack()
    {
        OnBuyButtonClicked?.Invoke(_ingredientData);
    }
    
    public void SetBuyButtonInteractable(bool interactable)
    {
        buyButton.interactable = interactable;
    }
}
