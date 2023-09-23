using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientBasketUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI purchasedPacksText;
    [SerializeField] private TextMeshProUGUI purchasedTotalIngredientsText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private IngredientShop ingredientShop;

    private Dictionary<IngredientData, int> _purchasedIngredients = new Dictionary<IngredientData, int>();

    private void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmPurchase);
    }

    private void OnDestroy()
    {
        confirmButton.onClick.RemoveListener(OnConfirmPurchase);
    }

    public void OnConfirmPurchase()
    {
        List<IngredientData> basketContents = ingredientShop.GetBasketContents();
        
        foreach (IngredientData ingredient in basketContents)
        {
            int packsToAdd = ingredientShop.GetPacksInBasket(ingredient);
            AddIngredients(ingredient, packsToAdd);
        }
        
        ingredientShop.ClearBasket();
    }
    
    public void AddIngredients(IngredientData ingredientData, int count)
    {
        if (_purchasedIngredients.ContainsKey(ingredientData))
        {
            _purchasedIngredients[ingredientData] += count;
        }
        else
        {
            _purchasedIngredients.Add(ingredientData, count);
        }

        UpdateBasketText();
    }

    public void UpdateBasketText()
    {
        purchasedPacksText.text = "Packs: ";

        int totalPacks = 0;
        int totalIngredients = 0;

        foreach (var ingredient in _purchasedIngredients.Keys)
        {
            int quantity = _purchasedIngredients[ingredient];
            totalPacks += quantity;

            purchasedPacksText.text += quantity + "x " + ingredient.Name + ", ";

            totalIngredients += ingredient.AmountPerPack * quantity;
        }

        purchasedTotalIngredientsText.text = $"Total ingredients: {totalIngredients}";
    }
}
