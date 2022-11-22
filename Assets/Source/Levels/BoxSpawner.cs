using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] private List<LevelZone> _zones;
    [SerializeField] private List<int> _maxCounts;
    [SerializeField] private Transform _spawnBoxParent;    
    [SerializeField] private Transform _bigBoxPoint;
    [SerializeField] private Box _boxWithCoinPrefab;
    [SerializeField] private Box _boxWithCrossPrefab;
    [SerializeField] private Box _bigBoxPrefab;
    [SerializeField] private float _boxWithCrossChanceRatio;
    [SerializeField] private LayerMask _boxLayer;    
    [SerializeField] private int _minMoneyAmount;
    [SerializeField] private int _maxMoneyAmount;
    [SerializeField] private int _minHealthAmount;
    [SerializeField] private int _maxHealthAmount;
    [SerializeField] private int _finalBoxMoneyAmount;

    private List<Box>[] _boxArray;
    private LevelZone _currentZone;

    private Box _bigBox;
    //private List<Box> _generatedBoxes = new();        
    private int _currentBoxCount;
    private Vector3 _tempPosition = new Vector3(0, 5, 0);
    private float _respawnDelay = 4f;
    private float _circleOffsetModifier = 1;
    private bool _isStopped;

    public event Action <int> IsPlayerMoneyIncreased;
    public event Action <int> IsBigBoxCollected;

    private void Awake()
    {
        _currentZone = _zones[0];

        _boxArray = new List<Box>[_zones.Count];
        
        GenerateAllBoxes();

        SpawnBox();

        SpawnBigBox();        
    }

    private void OnEnable()
    {
        foreach (var array in _boxArray)
        {
            foreach (var box in array)
            {
                box.IsItemCollected += OnItemCollected;
            }
        }

        /*foreach (var box in _generatedBoxes)
        {            
            box.IsItemCollected += OnItemCollected;
        }  */      

        _bigBox.IsItemCollected += OnBigboxCollected;
    }

    private void OnDisable()
    {
        foreach (var array in _boxArray)
        {
            foreach (var box in array)
            {
                box.IsItemCollected -= OnItemCollected;
            }
        }

        /*foreach (var box in _generatedBoxes)
        {         
            box.IsItemCollected -= OnItemCollected;
        }*/

        _bigBox.IsItemCollected -= OnBigboxCollected;
    }

    public void SetZoneIndex(LevelZone zone)
    {
        _currentZone = zone;

        SpawnBox();
    }

    public void StopSpawn()
    {
        _isStopped = true;
    }

    private void OnBigboxCollected()
    {
        int bigboxCount = 1;

        IsBigBoxCollected?.Invoke(bigboxCount);        
    }

    private void OnItemCollected()
    {
        _currentBoxCount--;

        if (GetMaxBoxCount() > _currentBoxCount)
            DOVirtual.DelayedCall(_respawnDelay, () => SpawnBox());        
    }

    private int GetMaxBoxCount()
    {
        return _maxCounts[GetCurrentZoneIndex()];
    }

    private int GetCurrentZoneIndex()
    {
        int index = -1;

        for (int i = 0; i < _zones.Count; i++)
        {
            if (_zones[i] == _currentZone)
                index = i;
        }

        return index;
    }

    /*private void CalculateMaxBoxesCount()
    {
        foreach (var count in _maxCounts)
        {
            _maxBoxesCount += count;
        }        
    }*/

    private void GenerateAllBoxes()
    {        
        int additive = 10;

        for (int i = 0; i < _zones.Count; i++)
        {
            _boxArray[i] = new();
        }

        //CalculateMaxBoxesCount();

        /*int boxWithCrossCount = Mathf.CeilToInt(GetMaxBoxCount() * _boxWithCrossChanceRatio);

        int count = Mathf.CeilToInt(GetMaxBoxCount() * countMultiplier < GetMaxBoxCount() + countAdditive ? 
            GetMaxBoxCount() * countMultiplier : GetMaxBoxCount() + countAdditive);*/


        for (int i = 0; i < _zones.Count; i++)
        {
            int totalBoxCount = _zones[i].BoxPoints.Count + additive;

            int boxWithCrossCount = Mathf.CeilToInt(totalBoxCount * _boxWithCrossChanceRatio);

            for (int j = 0; j < totalBoxCount; j++)
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

                _boxArray[i].Add(newBox);
            }
        }

        /*for (int i = 0; i < count; i++)
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

            _generatedBoxes.Add(newBox);
        }  */      
    }

    private void SpawnBox()
    {
        var boxes = _boxArray[GetCurrentZoneIndex()];

        if (TryGetInactiveBox(boxes, out Box inactiveBox) && GetMaxBoxCount() > _currentBoxCount &&
            TryGetFreePointToSpawn(_zones[GetCurrentZoneIndex()], out Vector3 freePoint) && _isStopped == false)
        {
            Vector2 randomOffsetPosition = UnityEngine.Random.insideUnitCircle * _circleOffsetModifier;

            inactiveBox.transform.position = freePoint + new Vector3(randomOffsetPosition.x, 0, randomOffsetPosition.y);

            if (inactiveBox.GetComponentInChildren<Coin>(true))
                inactiveBox.ActivateWholeBox(_minMoneyAmount, _maxMoneyAmount);

            if (inactiveBox.GetComponentInChildren<Cross>(true))
                inactiveBox.ActivateWholeBox(_minHealthAmount, _maxHealthAmount);

            _currentBoxCount++;

            if (GetMaxBoxCount() > _currentBoxCount)
                SpawnBox();
        }
    }

    private void SpawnBigBox()
    {
        if (_isStopped == false)
        {
            _bigBox = Instantiate(_bigBoxPrefab, _bigBoxPoint.position, Quaternion.identity, _spawnBoxParent);

            _bigBox.ActivateWholeBox(_finalBoxMoneyAmount, _finalBoxMoneyAmount);
        }
    }

    private bool TryGetFreePointToSpawn(LevelZone zone, out Vector3 freePoint)
    {
        List<Vector3> freePoints = new();

        freePoint = _tempPosition;

        foreach (var point in zone.BoxPoints)
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
   
    private bool TryGetInactiveBox(List<Box> boxes, out Box inactiveBox)
    {
        List<Box> inactiveBoxes = new ();

        inactiveBox = null;        

        foreach (var box in boxes)
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