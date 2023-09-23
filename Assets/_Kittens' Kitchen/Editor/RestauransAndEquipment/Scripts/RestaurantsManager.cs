using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RestaurantsManager : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly RestaurantsManager _restaurantsManager;
    [Inject] private readonly Bank _bank;

    [SerializeField] private ButtonRestaurant[] _totalRestaurants;//����� ���������� ����������
    [SerializeField] private ButtonGoToRestaurant _buttonGoToRestaurant;
    [SerializeField] private PanelInfoRestaurant _panelBuyRestaurant;

    public ButtonRestaurant SelectedButtonRestaurant { get; private set; }
    public List<ButtonRestaurant> BoughtRestaurants { get; private set; } = new List<ButtonRestaurant>();//��������� ���������, ��������� � ���� ��� ��

    private void Awake()
    {
        _eventManager.SelectButtonRestaurantEvent += SelectRestaurant;
        _eventManager.BuyRestaurantEvent += BuyRestaurant;
    }
    private void Start()
    {
        //������� �������� �� ������� ���������� � ��������� ��� ������� ����� ������ � ��������� 
        CreateNewDataRestaurants();
        LoadSelectedRestaurant();
    }
    /// <summary>
    /// �������� ���������� ��������� ��� ���������� � ������� � ���� 
    /// </summary>
    private void LoadSelectedRestaurant()
    {
        //���������� 
        SelectRestaurant(_totalRestaurants[0]);
        _buttonGoToRestaurant.OnPressButtonTrue();
    }

    public ButtonRestaurant[] GetTotalRestaurants()
    {
        return _totalRestaurants;
    }
    /// <summary>
    /// ������� ��������� � ���������
    /// </summary>
    private void BuyRestaurant()
    {
        if (SelectedButtonRestaurant != null && !Checks.CheckRestaurantContains(SelectedButtonRestaurant, _restaurantsManager) 
            && Checks.CheckEnoughGold(SelectedButtonRestaurant.Info.Price, _bank))
        {
            BoughtRestaurants.Add(SelectedButtonRestaurant);
            _eventManager.ChangeValueGold(-SelectedButtonRestaurant.Info.Price);
            _bank.RegisterRestaurantInvestments(SelectedButtonRestaurant.Info.name, SelectedButtonRestaurant.Info.Price);
            Debug.Log($"bought restaurant for {SelectedButtonRestaurant.Info.Price}");
        }
    }
    /// <summary>
    /// �������� � ���������� ������ � ��������� ����������
    /// </summary>
    public void SellRestaurant()
    {
        if (Checks.CheckIndexBoughtRestaurantPlaces(_restaurantsManager) > 1 && Checks.CheckRestaurantContains(SelectedButtonRestaurant, _restaurantsManager))
        {
            string nameRestaurant = SelectedButtonRestaurant.Info.name;
            float gold = _bank.GetRestaurantInvestments(nameRestaurant);
            Debug.Log($"gold added by sell restaurant = {gold}");
            _bank.UnRegisterRestaurantInvestments(nameRestaurant);
            _bank.UnRegisterSubLevelEquipment(nameRestaurant);
            _bank.UnRegisterNumberUpgradesEquipmentsInRestaurant(nameRestaurant);
            _bank.UnRegisterBoughtEquipmentInRestaursnt(nameRestaurant);
            _eventManager.ChangeValueGold(gold);

            BoughtRestaurants.Remove(SelectedButtonRestaurant);
        }
    }
    /// <summary>
    /// ��������� ���������� � ��������� ���������
    /// </summary>
    public RestaurantInfo GetInfoRestaurant()
    {
        var info = SelectedButtonRestaurant.Info;

        return info;
    }
    private void CreateNewDataRestaurants()
    {
        BoughtRestaurants.Add(_totalRestaurants[0]);
        //�������� ����������
    }
    public void GetInfoToPanel(RestaurantInfo info)
    {
        _panelBuyRestaurant.GetInfo(info);
    }
    private void SelectRestaurant(ButtonRestaurant buttonRestaurant)
    {
        SelectedButtonRestaurant = buttonRestaurant;
    }
    private void OnDestroy()
    {
        _eventManager.SelectButtonRestaurantEvent -= SelectRestaurant;
        _eventManager.BuyRestaurantEvent -= BuyRestaurant;
    }
}
