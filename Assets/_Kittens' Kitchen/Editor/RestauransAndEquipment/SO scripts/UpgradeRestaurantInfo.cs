using UnityEngine;

public class UpgradeRestaurantInfo : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private float _price;
    [SerializeField] private float _priceIncrease;

    public string Title => _title;
    public string Description => _description;
    public float Price => _price;
    public float PriceIncrease => _priceIncrease;
}

