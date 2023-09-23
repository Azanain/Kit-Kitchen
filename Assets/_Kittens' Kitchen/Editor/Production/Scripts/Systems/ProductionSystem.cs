using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionSystem : MonoBehaviour
{
    [SerializeField] private IngredientCounters ingredientCounters;
    [SerializeField] private UpgradeWindow upgradeWindow;
    [SerializeField] private MiniGameInitializer miniGameInitializer;
    
    [Header("Options")]
    [SerializeField] private ProductData productData;
    [SerializeField] private float productionTime = 5.0f;
    [SerializeField] private int numberOfProductsProduced = 3;
    [SerializeField] private int miniGameIsFailedProductsProduced = 0;

    private bool _isProducing;
    private BoxCollider2D _collider;
    private static bool _globalProductionActive;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        if (!_isProducing && !_globalProductionActive)
        {
            StartCoroutine(ProductionProcess());
        }
    }

    private IEnumerator ProductionProcess()
    {
        _isProducing = true;
        _globalProductionActive = true;
        
        Debug.Log("Starting production...");
        _collider.enabled = false;
        
        bool miniGameResult = false;

        GameObject ballInHolePrefab = miniGameInitializer.MiniGames[0];
        
        Vector3 offsetPositionPrefab = new Vector2(0, -2);
        GameObject ballInHoleInstance = Instantiate(ballInHolePrefab, Vector3.down + offsetPositionPrefab, Quaternion.identity);
        Debug.Log("this mini game: " + ballInHoleInstance.gameObject.name, ballInHoleInstance.gameObject);
        
        IMiniGame ballInHole = ballInHoleInstance.GetComponent<BallInHole>();
        ballInHole.OnMiniGameFinished += result => miniGameResult = result;
        
        Debug.Log("mini game was started!");
        
        while (!miniGameResult)
        {
            yield return null;
            if (ballInHole.IsGameFinished)
                break;
        }
        
        if (miniGameResult)
            Debug.Log("Mini game good result is: " + miniGameResult);
        Destroy(ballInHoleInstance);

        int productsToProduce = miniGameResult ? numberOfProductsProduced : miniGameIsFailedProductsProduced;
        
        yield return new WaitForSeconds(productionTime);
        _globalProductionActive = false;
        
        List<ProductData> producedProducts = new List<ProductData>();
        
        if (HasRequiredIngredients())
        {
            if (!ingredientCounters.GetIgnoreMaxCountProduct() && productData.Count >= productData.MaxCount)
            {
                Debug.Log("Production already at max count: " + productData.MaxCount);
                _isProducing = false;
                _collider.enabled = true;
                yield break;
            }

            foreach (ProductionRequirements requirement in productData.RequiredIngredients)
            {
                ingredientCounters.RemoveIngredient(requirement.Ingredient, requirement.Amount);
            }
            
            for (int i = 0; i < productsToProduce; i++)
            {
                if (productData.Count < productData.MaxCount)
                {
                    Debug.Log("Product " + (i+1) + " produced! " + productData.Label);
                    producedProducts.Add(ScriptableObject.CreateInstance<ProductData>());
                    productData.Count++;
                }
                else
                    Debug.LogWarning("Max count reached: " + productData.MaxCount);
            }
            ingredientCounters.UpdateCounterTexts();
            upgradeWindow.UpdateDescription();
        }
        else
        {
            Debug.Log("Not enough ingredients or mini-game failed to produce the product.");
        }

        _isProducing = false;
        _globalProductionActive = false;
        _collider.enabled = true;
    }

    bool HasRequiredIngredients()
    {
        foreach(ProductionRequirements requirement in productData.RequiredIngredients)
        {
            if (!ingredientCounters.ContainsIngredient(requirement.Ingredient, requirement.Amount))
            {
                return false;
            }
        }
        return true;
    }
}
