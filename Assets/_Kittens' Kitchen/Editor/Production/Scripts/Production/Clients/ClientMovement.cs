using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class ClientMovement : MonoBehaviour
{
    [Header("Move parameters")] 
    [SerializeField] private bool showParameters;
 /*   [ShowIf("showParameters")] */[SerializeField] private float movementSpeed = 3f;
   /* [ShowIf("showParameters")]*/ [SerializeField] private float swayAmount = 3f;
    /*[ShowIf("showParameters")]*/ [SerializeField] private float swaySpeed = 4f;
    
    [Header("Squash Parameters")]
  /*  [ShowIf("showParameters")] */[SerializeField] private float squashSpeed = 1f;
   /* [ShowIf("showParameters")]*/ [SerializeField] private float squashAmount = 1f;

    [Header("Options")]
    [SerializeField] private float delayAfterPurchase = 1f;
    [SerializeField] private float timeBetweenToPurchase = 1f;
    [SerializeField] private bool useExitPoint;
    private Transform _desiredTarget;
    private Transform _currentTarget;
    private Transform[] _targetsToPurchase;
    private Vector3 _startPosition;
    private Vector3 _previousPosition;
    private Vector3 _initialScale;
    private Quaternion _initialRotation;
    private Client _client;

    private float swayTimer;
    private int _randomTarget;
    private int _currentWaypoint;
    private bool _isWaitingForDelay;
    private bool _reachedEndOfWaypoints;
    private bool _reachedFreeSpace;
    private bool _reachedCurrentTarget;
    private bool _isActive;
    private bool _clientIsBack;
    private bool _isColliding;
    private bool _passedLastWaypoint;

    private enum ClientState 
    {   MovingToTarget, 
        WaitingAfterDestinationExit, 
        MovingBack, 
        MovingToFreeSpace, 
        MovingToWaypoints,
        MovingToLastWaypoints,
        MovingToExitPoint
    }
    private ClientState _currentState;

    private ProductPurchase _currentProduct;
    private ClientPool _clientPool;
    private SpawnerClients _spawnerClients;
    private ClientSystem _clientSystem;
    private WaypointSystem _waypointSystem;
    
    private List<Transform> _waypoints;
    private Transform _currentFreeSpace;
    private Transform _lastWaypoint;
    private Transform _secondLastWaypoint;
    private Transform _exitPoint;

    private static readonly object _purchaseMutex = new object();

    public void Initialize(ClientPool clientPool, SpawnerClients spawnerClients, ClientSystem clientSystem)
    {
        _clientPool = clientPool;
        _spawnerClients = spawnerClients;
        _clientSystem = clientSystem;
    }

    public void InitializeParameters(Client client, Transform desiredTarget, Transform[] targetsToPurchase, Transform exitPoint)
    {
        _client = client;
        _desiredTarget = desiredTarget;
        _targetsToPurchase = targetsToPurchase;
        _exitPoint = exitPoint;
    }

    private void Start()
    {
        _client = GetComponent<Client>();
        _waypointSystem = new WaypointSystem
        {
            Waypoints = _spawnerClients.GetWaypointSystem().Waypoints,
            CurrentWaypointIndex = _spawnerClients.GetWaypointSystem().CurrentWaypointIndex,
            FreeSpaces = _spawnerClients.GetWaypointSystem().FreeSpaces
        };
        _waypoints = _waypointSystem.Waypoints;
        _currentWaypoint = _waypointSystem.CurrentWaypointIndex;

        _startPosition = transform.position;
        _previousPosition = transform.position;
        _initialRotation = transform.rotation;
        _initialScale = transform.localScale;
        SetDesiredTarget();
        _currentState = ClientState.MovingToWaypoints;
    }

    private void Update()
    {
        SwayAnimation();
    }

    private void SwayAnimation()
    {
        if (transform.position != _previousPosition)
        {
            float offsetSway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            Quaternion targetRotation = _initialRotation * Quaternion.Euler(0, 0, offsetSway);
            
            float squash = Mathf.Abs(Mathf.Sin(Time.time * squashSpeed));
            Vector3 newScale = _initialScale + Vector3.up * squash * squashAmount;
        
            transform.localScale = newScale;
            transform.rotation = targetRotation;
        }
        
        _previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            CheckOverlapClient();
            switch (_currentState)
            {
                case ClientState.MovingToTarget:
                    MoveTowardsTarget();
                    break;
                case ClientState.MovingBack:
                    break;
                case ClientState.MovingToFreeSpace:
                    MoveToFreeSpace();
                    break;
                case ClientState.MovingToWaypoints:
                    MoveTowardsWaypoint();
                    break;
                case ClientState.MovingToLastWaypoints:
                    MoveThroughLastWaypoints(_lastWaypoint, _secondLastWaypoint);
                    break;
                case ClientState.MovingToExitPoint:
                    MovingToExitPoint();
                    break;
            }
        }
    }

    private void MoveTowardsWaypoint()
    {
        if (!_reachedEndOfWaypoints || _reachedFreeSpace)
        {
            if (_currentWaypoint >= _waypoints.Count)
            {
                if (!_clientIsBack)
                {
                    _currentFreeSpace = _waypointSystem.ChooseFreeSpace();
                    _currentState = ClientState.MovingToFreeSpace;
                    return;
                }
                else
                {
                    _clientIsBack = false;
                    //_currentState = ClientState.MovingToTarget;
                    return;
                }
            }

            Vector3 targetPosition = _waypoints[_currentWaypoint].position;
            
            float distance = Vector3.Distance(targetPosition, transform.position);
            Vector3 direction = (targetPosition - transform.position).normalized;

            if (distance > 0.1f)
            {
                transform.position += direction * movementSpeed * Time.deltaTime;
            }
            else
            {
               MoveToNextWaypoint();
            }
        }
    }

    private void MoveToNextWaypoint()
    {
        if (_reachedFreeSpace)
        {
            _secondLastWaypoint = _lastWaypoint;
            _lastWaypoint = _waypoints[_currentWaypoint];
            _currentWaypoint++;
        }
        else
        {
            _secondLastWaypoint = _lastWaypoint;
            _lastWaypoint = _waypoints[_currentWaypoint];
            _currentWaypoint++;

            if (_currentWaypoint >= _waypoints.Count)
            {
                _reachedEndOfWaypoints = false;
            }
        }
    }

    private void MoveToFreeSpace()
    {
    if (_waypointSystem.HasAvailableFreeSpaces())
    {
        if (_currentFreeSpace != null)
        {
            Vector3 direction = (_currentFreeSpace.position - transform.position).normalized;
            float distance = Vector2.Distance(_currentFreeSpace.position, transform.position);

            if (distance > 0.1f)
            {
                transform.position += direction * movementSpeed * Time.deltaTime;
            }
            else
            {
                if (!_reachedFreeSpace)
                    _spawnerClients.GetWaypointSystem().MarkFreeSpaceAsOccupied(_currentFreeSpace);
                _reachedFreeSpace = true;

                List<Transform> freeSpacesOrigin = _spawnerClients.GetWaypointSystem().FreeSpaces;

                lock (_purchaseMutex)
                {
                    if (_client.DesiredProduct.Count >= _client.DesiredQuantityProduct)
                    {
                        if (ClientPurchaseSetter.ClientAlreadyMovingToProduct) return;
                        if (!ClientPurchaseSetter.ClientAlreadyMovingToProduct)
                        {
                            StartCoroutine(DelayAfterDestinationFreeSpace());
                            _spawnerClients.GetWaypointSystem().AddFreeSpaceToOtherClients(_currentFreeSpace, freeSpacesOrigin);
                            ClientPurchaseSetter.ClientAlreadyMovingToProduct = true;    
                        }
                        
                    }
                    
                }
            }
        }
        else
        {
            _currentState = ClientState.WaitingAfterDestinationExit;
            Debug.Log("No free spaces available");
        }
    }
    else
    {
        _currentState = ClientState.WaitingAfterDestinationExit;
        Debug.Log("No free spaces available");
    }
    }

    private void MoveThroughLastWaypoints(Transform lastWaypoint, Transform secondLastWaypoint)
    {
        if (_waypoints != null && _waypoints.Count >= 2)
        {
            Vector3 targetPosition = lastWaypoint.position;
            Vector3 direction = (targetPosition - transform.position).normalized;
            float distance = Vector2.Distance(targetPosition, transform.position);

            if (distance > 0.1f && !_passedLastWaypoint)
            {
                transform.position += direction * movementSpeed * Time.deltaTime;
            }
            else
            {
                if (!_passedLastWaypoint)
                {
                    _passedLastWaypoint = true;
                }
                else
                {
                    targetPosition = secondLastWaypoint.position;
                    direction = (targetPosition - transform.position).normalized;
                    distance = Vector2.Distance(targetPosition, transform.position);

                    if (distance > 0.1f)
                    {
                        transform.position += direction * movementSpeed * Time.deltaTime;
                    }
                    else
                    {
                        _currentState = ClientState.MovingToTarget;
                    }
                }
            }
        }
    }

    private void MovingToExitPoint()
    {
        Transform[] targets = { _secondLastWaypoint, _lastWaypoint, _exitPoint };

        if (_waypointSystem.CurrentWaypointIndex < targets.Length)
        {
            Vector3 targetPosition = targets[_waypointSystem.CurrentWaypointIndex].position;
        
            float distance = Vector3.Distance(targetPosition, transform.position);
            Vector3 direction = (targetPosition - transform.position).normalized;

            if (distance > 0.1f)
                transform.position += direction * movementSpeed * Time.deltaTime;
            else
                _waypointSystem.CurrentWaypointIndex++;    
        }
    }

    private bool IsMovedToTarget(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(targetPosition, transform.position);
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (distance > 0.1f)
            transform.position += direction * movementSpeed * Time.deltaTime;
        else
            return true;

        return false;
    }

    private void MoveToPoint(Transform targetPoint)
    {
        Vector3 direction = (targetPoint.position - transform.position).normalized;

        float distance;
        do
        {
            distance = Vector3.Distance(targetPoint.position, transform.position);
            transform.position += direction * (movementSpeed - 0.6f) * Time.deltaTime;
        } 
        while (distance > 0.1f);
    }
    
    private bool HasReachedTarget(Transform target)
    {
        float distance = Vector3.Distance(target.position, transform.position);
        return distance <= 0.1f;
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (_currentTarget.position - transform.position).normalized;
        float distance = Vector2.Distance(_currentTarget.position, transform.position);

        if (distance > 0.1f)
            transform.position += direction * movementSpeed * Time.deltaTime;
        else
        {
            CheckPurchaseProduct();
        }
            
    }

    private void CheckOverlapClient()
    {
        bool isColliding;
        float collisionRadius = 0.25f;
        float avoidanceRadius = 0.25f;
        float avoidanceForce = 1.0f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, collisionRadius);
        
        isColliding = colliders.Any(collider => collider.GetComponent<Client>() != null && collider.GetComponent<Client>() != _client);

        Vector3 avoidanceVector = Vector3.zero;

        foreach (Collider2D collider in colliders)
        {
            Client otherClient = collider.GetComponent<Client>();
            if (otherClient != null && otherClient != _client)
            {
                Vector3 avoidanceDirection = (transform.position - collider.transform.position + Vector3.up * 0.5f).normalized;
                
                avoidanceVector += avoidanceDirection * avoidanceForce;

                isColliding = true;
            }
        }

        if (isColliding)
        {
            transform.position += avoidanceVector * Time.deltaTime;
        }
    }
    
    private Vector3 CalculateAvoidanceDirection()
    {
        float collisionRadius = 0.5f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, collisionRadius);
        
        Vector3 avoidanceDirection = Vector3.zero;

        foreach (Collider2D collider in colliders)
        {
            Client otherClient = collider.GetComponent<Client>();
            if (otherClient != null && otherClient != _client)
            {
                Vector3 relativePosition = collider.transform.position - transform.position;
                Vector3 avoidance = Vector3.Cross(relativePosition, Vector3.forward).normalized;
                
                avoidanceDirection += avoidance;
                _isColliding = true;
            }
        }

        return avoidanceDirection;
    }

    private IEnumerator MoveBackAndResetCurrentClient()
    {
        _currentState = ClientState.MovingBack;
        if (!useExitPoint)
        {
            Vector3 direction = (_startPosition - transform.position).normalized;
            float distance;
            do
            {
                distance = Vector2.Distance(_startPosition, transform.position);
                transform.position += direction * movementSpeed * Time.deltaTime;

                yield return null;
            } while (distance > 0.2f);    
        }
        else
        {
            _reachedCurrentTarget = true;
            _currentState = ClientState.MovingToExitPoint;
        }
        
        yield return new WaitForSeconds(timeBetweenToPurchase);
        ClientPurchaseSetter.ClientAlreadyMovingToProduct = false;

        _spawnerClients.DecreaseClientCount();
        _clientSystem.UnregisterClient(_client);
        _clientPool.AddClientToPool(_client);
    }
    
    private void SetDesiredTarget()
    {
        if (_targetsToPurchase == null || _targetsToPurchase.Length == 0 || _client == null || _spawnerClients == null) return;

        ProductData desiredProduct = DetermineDesiredProduct(_client.ProductsList);
        Dictionary<ProductData, Transform> targetPositions = _spawnerClients.GetDesiredTargetPositions();

        if (desiredProduct != null && targetPositions.ContainsKey(desiredProduct))
        {
            _desiredTarget = targetPositions[desiredProduct];
            _currentTarget = _desiredTarget;
        }

        Debug.Log("current target: " + _currentTarget.name, _currentTarget.gameObject);

        _currentState = ClientState.MovingToTarget;
    }

    private IEnumerator DelayAfterDestinationFreeSpace()
    {
        yield return new WaitForSeconds(delayAfterPurchase);
        _currentState = ClientState.MovingToLastWaypoints;
    }

    private void CheckPurchaseProduct()
    {
        float checkRadius = 0.5f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);

        foreach (Collider2D collider in colliders)
        {
            ProductPurchase currentProduct = collider.GetComponent<ProductPurchase>();
            if (currentProduct != null && currentProduct.IsPurchased)
            {
                StartCoroutine(MoveBackAndResetCurrentClient());
                Debug.Log("Wait for next purchase is started!");
            }
                
        }
    }

    private ProductData DetermineDesiredProduct(List<ProductData> clientProducts)
    {
        foreach (ProductData clientProduct in clientProducts)
        {
            if (clientProduct == _client.DesiredProduct)
            {
                return clientProduct;
            }
        }
        
        Debug.Log("No desired product found, returning the first product in the list.", gameObject);
        return clientProducts[0];
    }

    public void SetIsActive(bool isActive) => _isActive = isActive;

    public float GetSquashSpeed() => squashSpeed;
    public float GetSquashAmount() => squashAmount;
}

[System.Serializable]
public class WaypointSystem
{
    public List<Transform> Waypoints;
    public List<Transform> FreeSpaces;
    public int CurrentWaypointIndex { get; set; }

    public bool HasAvailableFreeSpaces()
    {
        return FreeSpaces.Count > 0;
    }
    
    public Transform ChooseFreeSpace()
    {
        if (FreeSpaces.Count > 0)
        {
            int randomIndex = Random.Range(0, FreeSpaces.Count);
            return FreeSpaces[randomIndex];
        }
        else
            return null;
    }
    
    public void MarkFreeSpaceAsOccupied(Transform freeSpace)
    {
        if (FreeSpaces.Contains(freeSpace))
        {
            FreeSpaces.Remove(freeSpace);
        }
    }

    public void AddFreeSpaceToOtherClients(Transform freeSpace, List<Transform> freeSpaces)
    {
        freeSpaces.Add(freeSpace);
    }    
}

public static class ClientPurchaseSetter
{
    private static bool _clientAlreadyMovingToProduct = false;
    private static readonly object _lockObject = new object();

    public static bool ClientAlreadyMovingToProduct
    {
        get
        {
            lock (_lockObject)
            {
                return _clientAlreadyMovingToProduct;
            }
        }
        set
        {
            lock (_lockObject)
            {
                _clientAlreadyMovingToProduct = value;
            }
        }
    }
}