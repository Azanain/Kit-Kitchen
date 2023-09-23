using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class IngredientCounters : MonoBehaviour
{
    [SerializeField] private IngredientShop ingredientShop;
    //[SerializeField] private TextMeshProUGUI breadCounterText;
    //[SerializeField] private TextMeshProUGUI sugarCounterText;
    [SerializeField] private TextMeshProUGUI basketCounterText;

    [Header("Options")] 
    //[InfoBox("Если false, то ингредиенты не потратятся, если максимальное количество продукта достигнуто")]
    [SerializeField] private bool ignoreMaxCountProduct;

    private readonly Dictionary<IngredientData, int> _purchasedIngredients = new Dictionary<IngredientData, int>();

    private void Start()
    {
        UpdateCounterTexts();
        ingredientShop.OnIngredientPurchased += AddIngredients;
    }

    private void OnDestroy()
    {
        ingredientShop.OnIngredientPurchased -= AddIngredients;
    }


    public void UpdateCounterTexts()
    {
        int totalBreadCount = ingredientShop.GetIngredientItem("bread").Count;
        int totalSugarCount = ingredientShop.GetIngredientItem("sugar").Count;

        //breadCounterText.text = totalBreadCount.ToString();
        //sugarCounterText.text = totalSugarCount.ToString();

        int totalIngredients = totalBreadCount + totalSugarCount;
        basketCounterText.text = totalIngredients.ToString(); 
    }
    
    public void AddIngredients(IngredientData ingredientData, int count)
    {
        ingredientData.Count += ingredientData.AmountPerPack;
    }
    
    public void RemoveIngredient(IngredientData ingredient, int amount = 1)
    {
        int totalAmountToRemove = amount * ingredient.AmountPerPack;

        if (ingredient.Count > 0)
        {
            int remainingToRemove = totalAmountToRemove;
            while (remainingToRemove > 0)
            {
                if (ingredient.Count > 0)
                {
                    int removeAmount = Mathf.Min(remainingToRemove, ingredient.Count * ingredient.AmountPerPack);
                    ingredient.Count -= removeAmount / ingredient.AmountPerPack;
                    remainingToRemove -= removeAmount;
                }
                else
                {
                    break;
                }
            }

            Debug.Log(amount + "x " + ingredient.Name + " removed from inventory.");
        }
        else
        {
            Debug.LogWarning("Attempted to remove " + ingredient.Name + " from inventory, but it wasn't available.");
        }
    }
    
    public bool ContainsIngredient(IngredientData ingredient, int amount)
    {
        int totalAmount = ingredient.Count * ingredient.AmountPerPack;
        return totalAmount >= amount;
    }

    public Dictionary<IngredientData, int> GetPurchasedIngredients() => _purchasedIngredients;
    public bool GetIgnoreMaxCountProduct() => ignoreMaxCountProduct;
}
