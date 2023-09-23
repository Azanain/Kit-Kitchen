using UnityEngine;

[CreateAssetMenu(fileName = "Restaurant")]
public class RestaurantInfo : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private float _price;
    [SerializeField] private RecommendedProduct _recommendedProduct;
    [SerializeField] private float _bonusRecommendedProduct;
    [SerializeField] private int _maxNumberEquipmentPlaces;
    [SerializeField] private EquipmentInfo[] _equipmentInfos;
    [SerializeField] private ProductData[] _productDatas;
    [SerializeField] private Vector2[] _spawnPositions;

    public string Name => _name;
    public float Price => _price;
    public RecommendedProduct RecommendedProduct => _recommendedProduct;
    public float BonusRecommendedProduct => _bonusRecommendedProduct;
    public int MaxNumberEquipmentPlaces => _maxNumberEquipmentPlaces;
    public EquipmentInfo[] EquipmentInfos => _equipmentInfos;

    public ProductData[] ProductDatas => _productDatas;
    public Vector2[] SpawnPositions => _spawnPositions;
}
