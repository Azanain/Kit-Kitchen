using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SpriteRenderer))]
public class EquipmentPlace : MonoBehaviour
{
    [HideInInspector] [Inject] public EventManager _eventManager;

    [Header("Data")]
    [SerializeField] private IngredientCounters _ingredientCounters;//
    [SerializeField] private UpgradeWindow _upgradeWindow;//
    [SerializeField] private MiniGameInitializer _miniGameInitializer;//

    [Header("Options")]
    [SerializeField] private ProductData _productData;//
    [SerializeField] private float _productionTime = 5.0f;//
    [SerializeField] private int _numberOfProductsProduced = 3;//
    [SerializeField] private int _miniGameIsFailedProductsProduced = 0;//
    [SerializeField] private BoxCollider2D _collider;//

    public EquipmentInfo equipmentInfo;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public int Sublevel { get; private set; }// загружать при выборе ресторана

    private bool _isProducing;//
    private static bool _globalProductionActive;//

    private void Awake()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_collider == null)
            _collider = GetComponent<BoxCollider2D>();
    }
    public void GetData(IngredientCounters ingredientCounters, UpgradeWindow upgradeWindow, MiniGameInitializer miniGameInitializer)
    {
        _ingredientCounters = ingredientCounters;
        _upgradeWindow = upgradeWindow;
        _miniGameInitializer = miniGameInitializer;
    }
    public void GetData(EventManager eventManager, Sprite sprite)
    {
        if(_eventManager == null)
            _eventManager = eventManager;

        _spriteRenderer.sprite = sprite;
    }
    public void GetProductData(ProductData productData)
    {
        _productData = productData;
    }

    public int ChangeLevel(int level)
    {
        Sublevel += level;
        return Sublevel;
    }
   
    private void OnMouseUpAsButton()
    {
        Vector3 newPositionPanel = new(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z - 5);
        _eventManager.OpenPanelBuyEquipmentEvent(newPositionPanel, this);

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

        GameObject ballInHolePrefab = _miniGameInitializer.MiniGames[0];

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

        int productsToProduce = miniGameResult ? _numberOfProductsProduced : _miniGameIsFailedProductsProduced;

        yield return new WaitForSeconds(_productionTime);
        _globalProductionActive = false;

        List<ProductData> producedProducts = new List<ProductData>();

        if (HasRequiredIngredients())
        {
            if (!_ingredientCounters.GetIgnoreMaxCountProduct() && _productData.Count >= _productData.MaxCount)
            {
                Debug.Log("Production already at max count: " + _productData.MaxCount);
                _isProducing = false;
                _collider.enabled = true;
                yield break;
            }

            foreach (ProductionRequirements requirement in _productData.RequiredIngredients)
            {
                _ingredientCounters.RemoveIngredient(requirement.Ingredient, requirement.Amount);
            }

            for (int i = 0; i < productsToProduce; i++)
            {
                if (_productData.Count < _productData.MaxCount)
                {
                    Debug.Log("Product " + (i + 1) + " produced! " + _productData.Label);
                    producedProducts.Add(ScriptableObject.CreateInstance<ProductData>());
                    _productData.Count++;
                }
                else
                    Debug.LogWarning("Max count reached: " + _productData.MaxCount);
            }
            _ingredientCounters.UpdateCounterTexts();
            _upgradeWindow.UpdateDescription();
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
        foreach (ProductionRequirements requirement in _productData.RequiredIngredients)
        {
            if (!_ingredientCounters.ContainsIngredient(requirement.Ingredient, requirement.Amount))
            {
                return false;
            }
        }
        return true;
    }
}
