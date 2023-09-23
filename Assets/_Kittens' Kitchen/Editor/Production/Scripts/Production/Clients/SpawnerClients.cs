using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Zenject;

public class SpawnerClients : MonoBehaviour
{
    [SerializeField] private Client clientPrefab;
    [SerializeField] private WaypointSystem waypointSystem;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform[] targetsToPurchase;
    [SerializeField] private List<Sprite> clientSprites;

    [Header("Options")]
    [SerializeField] private float spawnInterval;

    [SerializeField] private int minQuantityProduct;
    [SerializeField] private int maxQuantityProduct;
    [SerializeField] private int maxClients = 5;
    [SerializeField] [Range(0, 1)] private float clientSpawnChance;

    [Inject] private ClientFactory _clientFactory;
    [Inject] private ClientPool _clientPool;
    [Inject] private ClientSystem _clientSystem;

    private Transform _desiredTarget;
    private Client _currentClient;

    private float _timeToSpawn;
    private int _clientCount;
    
    private readonly Dictionary<ProductData, Transform> _desiredTargetPositions = new Dictionary<ProductData, Transform>();

    private void Start()
    {
        _clientPool.Initialize(clientPrefab, 20);
        _clientFactory.SetClientPool(_clientPool);

        FillTargetPositionsWithDesiredProduct();
        ClientPurchaseSetter.ClientAlreadyMovingToProduct = false;
    }

    private void FillTargetPositionsWithDesiredProduct()
    {
        for (int i = 0; i < targetsToPurchase.Length; i++)
        {
            ProductPurchase productPurchase = targetsToPurchase[i].GetComponent<ProductPurchase>();

            if (productPurchase != null)
            {
                ProductData productData = productPurchase.GetProductData();
                _desiredTargetPositions[productData] = targetsToPurchase[i];
            }
        }
    }

    private void Update()
    {
        _timeToSpawn += Time.deltaTime;
        if (_timeToSpawn >= spawnInterval)
        {
            if (clientSpawnChance >= UnityEngine.Random.value && _clientCount < maxClients)
                 SpawnClient();
            
            _timeToSpawn = 0;
        }
    }

    private void SpawnClient()
    {
        var randomSprite = SetRandomSprite();
        var randomProduct = SetRandomProduct();

        Client newClient = _clientFactory.CreateClient(spawnPoint.position, randomSprite);
        newClient.transform.SetParent(transform);
        newClient.DesiredProduct = randomProduct;
        newClient.DesiredQuantityProduct = newClient.SetRandomQuantityProduct(minQuantityProduct, maxQuantityProduct);

        InitializeClientMovement(newClient);

        _clientSystem.RegisterClient(newClient);
        _clientCount++;
    }

    private void InitializeClientMovement(Client newClient)
    {
        ClientMovement clientMovement = newClient.GetComponent<ClientMovement>();
        clientMovement.Initialize(_clientPool, this, _clientSystem);
        clientMovement.InitializeParameters(newClient, _desiredTarget, targetsToPurchase, exitPoint);
        clientMovement.SetIsActive(true);
    }

    private Sprite SetRandomSprite()
    {
        int randomIndexSprite = UnityEngine.Random.Range(0, clientSprites.Count);
        Sprite randomSprite = clientSprites[randomIndexSprite];
        return randomSprite;
    }

    private ProductData SetRandomProduct()
    {
        int randomIndexProduct = UnityEngine.Random.Range(0, clientPrefab.ProductsList.Count);
        ProductData randomProduct = clientPrefab.ProductsList[randomIndexProduct];

        return randomProduct;
    }

    public void DecreaseClientCount() => _clientCount--;
    public Dictionary<ProductData, Transform> GetDesiredTargetPositions() => _desiredTargetPositions;

    public WaypointSystem GetWaypointSystem() => waypointSystem;
}