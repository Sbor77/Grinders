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
    [SerializeField] private Box _boxPrefab;
    [SerializeField] private Box _bigBoxPrefab;    
    [SerializeField] private LayerMask _boxLayer;    
    [SerializeField] private int _targetBoxesCount;
    [SerializeField] private int _minMoneyAmount;
    [SerializeField] private int _maxMoneyAmount;
    [SerializeField] private int _finalBoxMoneyAmount;

    private List<Box> _boxes = new List<Box>();    
    private int _playerMoney;
    private int _currentBoxCount;
    private float _respawnDelay = 4f;
    private float _circleOffsetModifier = 1;

    public int PlayerMoney => _playerMoney;    

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

    private void SpawnBox()
    {
        if (TryGetInactiveBox(out Box poolBox) && _targetBoxesCount > _currentBoxCount)
        {
            Vector2 randomOffsetPosition = UnityEngine.Random.insideUnitCircle * _circleOffsetModifier;

            poolBox.transform.position += new Vector3(randomOffsetPosition.x, poolBox.transform.position.y, randomOffsetPosition.y);

            poolBox.ActivateWholeBox(_minMoneyAmount, _maxMoneyAmount);

            _currentBoxCount++;

            if (_targetBoxesCount > _currentBoxCount)            
                SpawnBox();            
        }
    }

    private void SpawnBigBox()
    {
        Box bigBox = Instantiate(_bigBoxPrefab, _bigBoxPoint.position, Quaternion.identity, _spawnBoxParent);

        bigBox.ActivateWholeBox(_finalBoxMoneyAmount, _finalBoxMoneyAmount);
    }


    private void OnCoinCollected(int money)
    {
        _playerMoney += money;

        _currentBoxCount--;

        if (_targetBoxesCount > _currentBoxCount)        
            DOVirtual.DelayedCall(_respawnDelay, SpawnBox);        
    }

    private void GenerateAllBoxes()
    {
        for (int i = 0; i < _boxSpawnPoints.Count; i++)
        {
            var newBox = Instantiate(_boxPrefab, _boxSpawnPoints[i].position, Quaternion.identity, _spawnBoxParent);
            
            newBox.DeactivateWholeBox();            

            _boxes.Add(newBox);
        }        
    }
   
    private bool TryGetInactiveBox(out Box inactiveBox)
    {
        List<Box> inactiveBoxes = new List<Box>();

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