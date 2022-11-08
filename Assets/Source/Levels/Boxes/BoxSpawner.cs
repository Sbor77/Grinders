using DG.Tweening;
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
    [SerializeField] private float _minDistance;
    [SerializeField] private int _targetBoxesCount;
    [SerializeField] private int _minMoneyAmount;
    [SerializeField] private int _maxMoneyAmount;
    [SerializeField] private int _finalBoxMoneyAmount;

    private float _respawnDelay = 4f;
    private List<Box> _boxes = new List<Box>();
    private int _currentBoxCount;
    private float _circleOffsetModifier = 1;

    public int CurrentBoxes => _currentBoxCount;

    private void Awake()
    {
        GenerateAllBoxes();
    }

    private void Start()
    {
        for (int i = 0; i < _boxSpawnPoints.Count; i++)
        {
            SpawnBox();
        }

        SpawnBigBox();
    }    

    private void OnEnable()
    {
        foreach (var box in _boxes)
        {
            box.IsCrushedBoxDeactivated += IsCrushedBoxDeactivated;            
        }
    }

    private void OnDisable()
    {
        foreach (var box in _boxes)
        {
            box.IsCrushedBoxDeactivated -= IsCrushedBoxDeactivated;
        }
    }

    public void SpawnBox()
    {
        if (TryGetInactiveBox(out Box poolBox) && _targetBoxesCount > _currentBoxCount)
        {
            Vector2 randomOffsetPosition = Random.insideUnitCircle * _circleOffsetModifier;

            poolBox.transform.position += new Vector3(randomOffsetPosition.x, 0, randomOffsetPosition.y);

            poolBox.Activate(_minMoneyAmount, _maxMoneyAmount);

            _currentBoxCount++;            
        }
    }

    private void SpawnBigBox()
    {
        Box bigBox = Instantiate(_bigBoxPrefab, _bigBoxPoint.position, Quaternion.identity, _spawnBoxParent);

        bigBox.Activate(_finalBoxMoneyAmount, _finalBoxMoneyAmount);
    }


    private void IsCrushedBoxDeactivated()
    {
        _currentBoxCount--;

        if (_targetBoxesCount > _currentBoxCount)
        {
            DOVirtual.DelayedCall(_respawnDelay, () => SpawnBox());
        }
    }

    private void GenerateAllBoxes()
    {
        for (int i = 0; i < _boxSpawnPoints.Count; i++)
        {
            var newBox = Instantiate(_boxPrefab, _boxSpawnPoints[i].position, Quaternion.identity, _spawnBoxParent);
            
            newBox.Deactivate();

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
            int randomIndex = Random.Range(0, inactiveBoxes.Count);

            inactiveBox = inactiveBoxes[randomIndex];            
        }

        return inactiveBox != null;
    }
}