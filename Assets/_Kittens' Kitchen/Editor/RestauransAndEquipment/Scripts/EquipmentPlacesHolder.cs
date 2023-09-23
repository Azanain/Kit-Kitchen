using UnityEngine;
using Zenject;

public class EquipmentPlacesHolder : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly EquipmentManager _equipmentManager;

    [SerializeField] private EquipmentPlace _placePrefab;

    [Header("Data")]
    [SerializeField] private IngredientCounters ingredientCounters;//
    [SerializeField] private UpgradeWindow upgradeWindow;//
    [SerializeField] private MiniGameInitializer miniGameInitializer;//
    public void CreateEquipmentPlaces(EquipmentInfo[] equipmentInfos, Vector2[] positions)
    {
        DeletePlaces();

        for (int i = 0; i < equipmentInfos.Length; i++)
        {
            var place = Instantiate(_placePrefab, positions[i], Quaternion.identity, transform);
            place.equipmentInfo = equipmentInfos[i];

            place.GetData(ingredientCounters, upgradeWindow, miniGameInitializer);//
            place.GetProductData(_equipmentManager?.InfoRestaurant.ProductDatas[i]);
            place.GetData(_eventManager, equipmentInfos[i].Sprite);
        }
    }
    private void DeletePlaces()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}
