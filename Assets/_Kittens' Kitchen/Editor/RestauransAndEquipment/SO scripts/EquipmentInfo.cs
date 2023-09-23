using UnityEngine;

[CreateAssetMenu(fileName = "Equipment")]
public class EquipmentInfo : ScriptableObject
{
    [SerializeField] private float _price;
    [SerializeField] private int _priceProduct;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private EquipmentTypes _equipmentTypes;

    public float Price => _price;
    public int PriceProduct => _priceProduct;
    public Sprite Sprite => _sprite;
    public EquipmentTypes EquipmentTypes => _equipmentTypes;
}
