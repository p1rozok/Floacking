


using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class FlockingFish : MonoBehaviour
{
    [Header("Obstacle Avoidance")]
    public float obstacleAvoidanceDistance = 1.0f;
    public float obstacleAvoidanceWeight = 1.0f;

    public Vector2 velocity;

    private FishData _fishData;
    private string _species;
    private SpatialGrid _spatialGrid;
    private Rect _habitatArea;
    private float _bufferZone;
    private Vector2 _acceleration;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _previousPosition;
    private int _obstacleLayerMask;
    private bool _isInitialized = false;
    private FishManager _fishManager;
    private bool ToggleCatch = true; 

    private Transform _baitTransform;
    private PresenterBait _baitInstance;
    private Bait _baitData;
    private float _detectionRadius;
    private float _attractionWeight;
    private string _preferredBaitType;

    [SerializeField] private returnPosition returnPositionScript;


    private Vector2[] _cachedNeighborPositions = new Vector2[10];
    private Vector2[] _cachedNeighborVelocities = new Vector2[10];
    private int _neighborCount = 0;

    private float _neighborUpdateTimer = 0f;
    private float _neighborUpdateInterval = 0.5f; 

    public bool IsInitialized => _isInitialized;

    public void Initialize(FishData data, SpatialGrid grid, Rect habitatArea, float bufferZone, FishTickManager fishTickManager, FishManager fishManager)
    {
        if (data == null || grid == null || fishTickManager == null || fishManager == null)
        {
            return;
        }

        _fishData = data;
        _spatialGrid = grid;
        _habitatArea = habitatArea;
        _bufferZone = bufferZone;

        _detectionRadius = _fishData.detectionRadius;
        _attractionWeight = _fishData.attractionWeight;
        _preferredBaitType = _fishData.preferredBaitType;

        gameObject.SetActive(true);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = _fishData.fishSprite;
            _spriteRenderer.color = _fishData.color;
        }

        velocity = Random.insideUnitCircle.normalized * _fishData.maxSpeed;
        _acceleration = Vector2.zero;
        _species = _fishData.species;
        _previousPosition = transform.position;

        _spatialGrid.AddFish(this);

        _obstacleLayerMask = LayerMask.GetMask("Obstacle");

        fishTickManager.RegisterFish(this);

        _fishManager = fishManager;

        _isInitialized = true;
    }

    private void Start()
    {
        ToggleMiniGame.OnCath.AddListener(() =>
        {
            ToggleCatch = false;
        });
        
        ToggleMiniGame.OnWin.AddListener(() =>
        {
            ToggleCatch = true;
        });
        
    }

    private void Update()
    {
        if (!_isInitialized) return;
        

        UpdateNeighbors();
        
        UpdateBehavior();
    }

    public void UpdateNeighbors()
    {
        _neighborUpdateTimer += Time.deltaTime;
        if (_neighborUpdateTimer >= _neighborUpdateInterval)
        {
            _neighborUpdateTimer = 0f;
            UpdateNeighborsPositions();
        }
    }

    private void UpdateNeighborsPositions()
    {
        if (_spatialGrid == null || !_isInitialized)
        {
            return;
        }

        List<FlockingFish> neighbors = _spatialGrid.GetNeighbors(this, _fishData.neighborDistance);
        _neighborCount = 0;

        foreach (var neighbor in neighbors)
        {
            if (neighbor != null && neighbor != this && neighbor.transform != null && neighbor._species == _species)
            {
                if (_neighborCount >= _cachedNeighborPositions.Length)
                {
                    break;
                }

                _cachedNeighborPositions[_neighborCount] = neighbor.transform.position;
                _cachedNeighborVelocities[_neighborCount] = neighbor.velocity;
                _neighborCount++;
            }
        }
    }

    private void UpdateBehavior()
    {
        if (_fishData == null) return;

        _acceleration = Vector2.zero;

        UpdateBaitInfo();
        if(ToggleCatch)
        {
            MoveTowardsBait();
        }
        
        CalculateFlock();
        KeepInBounds();
        ObstacleAvoidance();

        ApplyMovement();
    }

    private void UpdateBaitInfo()
    {
        Transform newBaitTransform = returnPositionScript.ReturnTransformPositionBait();

        if (_baitTransform != newBaitTransform)
        {
            _baitTransform = newBaitTransform;

            if (_baitTransform != null)
            {
                _baitInstance = _baitTransform.GetComponent<PresenterBait>();
                if (_baitInstance != null)
                {
                    _baitData = _baitInstance.baitData;
                }
                else
                {
                    _baitData = null;
                }
            }
            else
            {
                _baitInstance = null;
                _baitData = null;
            }
        }
    }

    private void MoveTowardsBait()
    {
        if (_baitTransform == null || _baitData == null)
        {
            return;
        }

        if (_baitData.BaitType != _preferredBaitType)
        {
            return;
        }

        Vector3 directionToBait = _baitTransform.position - transform.position;
        float distanceSqr = directionToBait.sqrMagnitude;

        if (distanceSqr < _detectionRadius * _detectionRadius)
        {
            Vector2 desiredVelocity = directionToBait.normalized * _fishData.maxSpeed;
            Vector2 steering = desiredVelocity - velocity;
            steering = Vector2.ClampMagnitude(steering, _fishData.maxForce);
            steering *= _attractionWeight;

            _acceleration += steering;
        }
    }

    private void CalculateFlock()
    {
        if (_neighborCount == 0)
        {
            return;
        }

        Vector2 alignment = Vector2.zero;
        Vector2 cohesion = Vector2.zero;
        Vector2 separation = Vector2.zero;

        for (int i = 0; i < _neighborCount; i++)
        {
            Vector2 neighborPosition = _cachedNeighborPositions[i];
            Vector2 neighborVelocity = _cachedNeighborVelocities[i];
            Vector2 toNeighbor = (Vector2)transform.position - neighborPosition;
            float distanceSqr = toNeighbor.sqrMagnitude;

            alignment += neighborVelocity;
            cohesion += neighborPosition;

            if (distanceSqr > 0 && distanceSqr < _fishData.avoidanceRadius * _fishData.avoidanceRadius)
            {
                Vector2 awayFromNeighbor = toNeighbor / distanceSqr;
                separation += awayFromNeighbor;
            }
        }

        alignment /= _neighborCount;
        cohesion /= _neighborCount;

        alignment = alignment.normalized * _fishData.maxSpeed - velocity;
        alignment = Vector2.ClampMagnitude(alignment, _fishData.maxForce);

        cohesion = (cohesion - (Vector2)transform.position).normalized * _fishData.maxSpeed - velocity;
        cohesion = Vector2.ClampMagnitude(cohesion, _fishData.maxForce);

        separation = separation.normalized * _fishData.maxSpeed - velocity;
        separation = Vector2.ClampMagnitude(separation, _fishData.maxForce);

        _acceleration += alignment * _fishData.alignmentWeight;
        _acceleration += cohesion * _fishData.cohesionWeight;
        _acceleration += separation * _fishData.separationWeight;
    }

    private void ObstacleAvoidance()
    {
        float rayDistance = obstacleAvoidanceDistance;
        Vector2 direction = velocity.normalized;
        Vector2 rayOrigin = (Vector2)transform.position + direction * 0.1f;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayDistance, _obstacleLayerMask);
        if (hit.collider != null)
        {
            Vector2 avoidForce = Vector2.Reflect(direction, hit.normal).normalized * _fishData.maxForce * obstacleAvoidanceWeight;
            _acceleration += avoidForce;
        }
    }

    private void KeepInBounds()
    {
        Vector2 position = transform.position;
        Vector2 toCenter = Vector2.zero;

        float xMin = _habitatArea.xMin + _bufferZone;
        float xMax = _habitatArea.xMax - _bufferZone;
        float yMin = _habitatArea.yMin + _bufferZone;
        float yMax = _habitatArea.yMax - _bufferZone;

        if (position.x < xMin)
        {
            toCenter.x = xMin - position.x;
        }
        else if (position.x > xMax)
        {
            toCenter.x = xMax - position.x;
        }

        if (position.y < yMin)
        {
            toCenter.y = yMin - position.y;
        }
        else if (position.y > yMax)
        {
            toCenter.y = yMax - position.y;
        }

        if (toCenter != Vector2.zero)
        {
            Vector2 desired = toCenter.normalized * _fishData.maxSpeed;
            Vector2 steer = desired - velocity;
            steer = Vector2.ClampMagnitude(steer, _fishData.maxForce * 2f);
            _acceleration += steer;
        }
    }

    private void ApplyMovement()
    {
        velocity += _acceleration * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, _fishData.maxSpeed);
        Vector2 newPosition = (Vector2)transform.position + velocity * Time.deltaTime;
        _previousPosition = transform.position;
        transform.position = newPosition;
        _spatialGrid?.UpdateFishCell(this, _previousPosition);

        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipY = velocity.x < 0;
        }

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + _fishData.spriteRotation);
    }

    public void RemoveFish()
    {
        if (_isInitialized)
        {
            _isInitialized = false;

            if (_spatialGrid != null)
            {
                _spatialGrid.RemoveFish(this);
            }

            if (_fishManager != null)
            {
                _fishManager.RemoveFish(this);
            }

            FishTickManager.Instance?.UnregisterFish(this);

            PopulationManager.Instance?.DecreasePopulation(1f);

            Destroy(gameObject);
        }
    }
}


