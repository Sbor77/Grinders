using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<LevelZone> _zones;
    [SerializeField] private List<int> _maxCounts;
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] protected Transform _enemyParent;
    [SerializeField] protected LayerMask _enemyLayer;    
    [SerializeField] private float _respawnTime = 5f;
        
    List<Enemy>[] _enemyArray;
    private LevelZone _currentZone;
    private Vector3 _tempPosition = new Vector3(0, 5, 0);    
    private int _playerKills;
    private int _currentEnemyCount;
    private float _spawnRadiusModifier = 1;
    private bool _isDeactivated;

    public event Action <int> IsPLayerKillsIncreased;

    private void Awake()
    {
        _currentZone = _zones[0];

        _enemyArray = new List<Enemy>[_zones.Count];        

        GenerateAllEnemies();

        SpawnEnemy();
    }

    private void OnEnable()
    {
        foreach (var array in _enemyArray)
        {
            foreach (var enemy in array)
            {
                enemy.IsDeactivated += OnEnemyDeactivated;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var array in _enemyArray)
        {
            foreach (var enemy in array)
            {
                enemy.IsDeactivated -= OnEnemyDeactivated;
            }
        }        
    }

    public void SetZoneIndex(LevelZone zone)
    {
        _currentZone = zone;

        DeactivateEnemies(zone);

        SpawnEnemy();        
    }

    public void Deactivate()
    {
        _isDeactivated = true;

        foreach (var array in _enemyArray)
        {
            foreach (var enemy in array)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnemyDeactivated()
    {
        _playerKills++;

        IsPLayerKillsIncreased?.Invoke(_playerKills);        

        _currentEnemyCount--;

        if (_currentEnemyCount < GetMaxEnemyCount())                    
            DOVirtual.DelayedCall(_respawnTime, SpawnEnemy);        
    }

    private void GenerateAllEnemies()
    {
        for (int i = 0; i < _zones.Count; i++)
        {
            _enemyArray[i] = new();            
        }

        for (int i = 0; i < _zones.Count; i++)
        {
            for (int j = 0; j < _zones[i].EnemyPoints.Count; j++)
            {
                Vector3 position = _zones[i].EnemyPoints[j].transform.position;

                Enemy randomPrefab = GetRandomEnemy(_enemyPrefabs);

                var newEnemy = Instantiate(randomPrefab, position, Quaternion.identity, _enemyParent);

                newEnemy.SetDefaultPosition(position);

                newEnemy.gameObject.SetActive(false);

                _enemyArray[i].Add(newEnemy);
            }
        }
    }    

    private void SpawnEnemy()
    {
        var enemies = _enemyArray[GetCurrentZoneIndex()];

        if (TryGetInactiveEnemy(enemies, out Enemy inactiveEnemy) && _currentEnemyCount < GetMaxEnemyCount() && _isDeactivated == false &&
            TryGetFreePointToSpawn(_zones[GetCurrentZoneIndex()], out Vector3 freePoint))
        {
            Vector2 randomOffsetPosition = UnityEngine.Random.insideUnitCircle * _spawnRadiusModifier;

            inactiveEnemy.transform.position = freePoint + new Vector3(randomOffsetPosition.x, 0, randomOffsetPosition.y);

            inactiveEnemy.Restore();

            inactiveEnemy.gameObject.SetActive(true);

            _currentEnemyCount++;            

            if (_currentEnemyCount < GetMaxEnemyCount())
                SpawnEnemy();
        }
    }

    private void DeactivateEnemies(LevelZone zoneExclusive)
    {
        for (int i = 0; i < _zones.Count; i++)
        {
            if (_zones[i] != zoneExclusive)
            {
                foreach (var enemy in _enemyArray[i])
                {
                    enemy.gameObject.SetActive(false);
                }
            }            
        }
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

    private int GetMaxEnemyCount()
    {
        return _maxCounts[GetCurrentZoneIndex()];
    }

    private bool TryGetFreePointToSpawn(LevelZone zone, out Vector3 freePoint)
    {
        List<Vector3> freePoints = new();

        freePoint = _tempPosition;

        foreach (var point in zone.EnemyPoints)
        {
            if (Physics.CheckSphere(point.position, 0.5f, _enemyLayer) == false)
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

    private bool TryGetInactiveEnemy(List<Enemy> enemies, out Enemy inactiveEnemy)
    {
        List<Enemy> inactiveEnemies = new();

        inactiveEnemy = null;

        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.activeSelf == false)
                inactiveEnemies.Add(enemy);
        }

        if (inactiveEnemies.Count > 0)
        {
            inactiveEnemy = GetRandomEnemy(inactiveEnemies);
        }

        return inactiveEnemy != null;
    }

    private Enemy GetRandomEnemy(List<Enemy> enemies)
    {
        return enemies[UnityEngine.Random.Range(0, enemies.Count)];
    }
}