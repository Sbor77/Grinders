using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _boxSpawnPoints;
    [SerializeField] private Transform _spawnBoxParent;    
    [SerializeField] private Transform _bigBoxPoint;
    [SerializeField] private Box _boxWithCoinPrefab;
    [SerializeField] private Box _boxWithCrossPrefab;
    [SerializeField] private Box _bigBoxPrefab;
    [SerializeField] private float _boxWithCrossChanceRatio;
    [SerializeField] private LayerMask _boxLayer;    
    [SerializeField] private int _boxesCount;
    [SerializeField] private int _minMoneyAmount;
    [SerializeField] private int _maxMoneyAmount;
    [SerializeField] private int _finalBoxMoneyAmount;

    private List<Box> _boxes = new List<Box>();    
    private int _playerMoney;
    private int _currentBoxCount;
    private Vector3 _tempPosition = new Vector3(0, 5, 0);
    private float _respawnDelay = 4f;
    private float _circleOffsetModifier = 1;

    public event Action<int> IsPlayerMoneyIncreased;

    private void Awake()
    {
        GenerateAllBoxes();
    }

    private void Start()
    {
        SpawnBox();

        SpawnBigBox();
    }

    private void OnEnable()
    {
        foreach (var box in _boxes)
        {
            box.IsCoinCollected += OnCoinCollected;            
        }
    }

    private void OnDisable()
    {
        foreach (var box in _boxes)
        {
            box.IsCoinCollected -= OnCoinCollected;
        }
    }

    private void OnCoinCollected(int money)
    {
        _playerMoney += money;

        IsPlayerMoneyIncreased?.Invoke(_playerMoney);

        _currentBoxCount--;

        if (_boxesCount > _currentBoxCount)        
            DOVirtual.DelayedCall(_respawnDelay, SpawnBox);        
    }

    private void GenerateAllBoxes()
    {
        float countMultiplier = 2;
        float countAdditive = 10;

        int boxWithCrossCount = Mathf.CeilToInt(_boxesCount * _boxWithCrossChanceRatio);

        int count = Mathf.CeilToInt(_boxesCount * countMultiplier < _boxesCount + countAdditive ? _boxesCount * countMultiplier : _boxesCount + countAdditive);

        for (int i = 0; i < count; i++)
        {
            Box boxPrefab;

            if (boxWithCrossCount > 0)
            {
                boxPrefab = _boxWithCrossPrefab;

                boxWithCrossCount--;
            }
            else
            {
                boxPrefab = _boxWithCoinPrefab;
            }            

            var newBox = Instantiate(boxPrefab, _tempPosition, Quaternion.identity, _spawnBoxParent);
            
            newBox.DeactivateWholeBox();            

            _boxes.Add(newBox);
        }        
    }

    private void SpawnBox()
    {
        if (TryGetInactiveBox(out Box inactiveBox) && _boxesCount > _currentBoxCount && TryGetFreePointToSpawn(out Vector3 freePoint))
        {
            Vector2 randomOffsetPosition = UnityEngine.Random.insideUnitCircle * _circleOffsetModifier;

            inactiveBox.transform.position = freePoint + new Vector3(randomOffsetPosition.x, 0, randomOffsetPosition.y);

            inactiveBox.ActivateWholeBox(_minMoneyAmount, _maxMoneyAmount);

            _currentBoxCount++;            

            if (_boxesCount > _currentBoxCount)            
                SpawnBox();            
        }
    }

    private void SpawnBigBox()
    {
        Box bigBox = Instantiate(_bigBoxPrefab, _bigBoxPoint.position, Quaternion.identity, _spawnBoxParent);

        bigBox.ActivateWholeBox(_finalBoxMoneyAmount, _finalBoxMoneyAmount);
    }

    private bool TryGetFreePointToSpawn(out Vector3 freePoint)
    {
        List<Vector3> freePoints = new();

        freePoint = _tempPosition;

        foreach (var point in _boxSpawnPoints)
        {
            if (Physics.CheckSphere(point.position, 2f, _boxLayer) == false)
            {
                freePoints.Add(point.position);
            }
        }

        if (freePoints.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, freePoints.Count);

            freePoint = freePoints[randomIndex];        
        }

        return freePoints.Count > 0;
    }
   
    private bool TryGetInactiveBox(out Box inactiveBox)
    {
        List<Box> inactiveBoxes = new ();

        inactiveBox = null;

        foreach (var box in _boxes)
        {
            if (box.gameObject.activeSelf == false)
                inactiveBoxes.Add(box);
        }      

        if (inactiveBoxes.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, inactiveBoxes.Count);

            inactiveBox = inactiveBoxes[randomIndex];            
        }

        return inactiveBox != null;
    }   
}