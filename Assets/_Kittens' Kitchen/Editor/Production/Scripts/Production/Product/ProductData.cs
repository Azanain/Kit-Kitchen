using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "Products/Product Data")]
public class ProductData : ScriptableObject
{
   /* [ShowAssetPreview]*/ public Sprite Icon;
    public int Count;
    public int MaxCount;
    public string Label;
    public int Cost;
/*    [ResizableTextArea]*/ public string Description;
    public List<ProductionRequirements> RequiredIngredients;
}

[System.Serializable]
public class ProductionRequirements
{
    public IngredientData Ingredient;
    public int Amount;
}
