using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] protected Transform _enemyParent;
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] private int _enemyCount;
    [SerializeField] private float _respawnTime = 5f;

    private List<Enemy> _generatedEnemies = new();
    private int _playerKills;
    private int _currentEnemyCount;
    private float _spawnRadiusModifier = 1;
    private bool _isDeactivated;

    public event Action <int> IsPLayerKillsIncreased;

    private void Awake()
    {
        GenerateAllEnemies();
    }

    private void Start()
    {
        SpawnEnemy();
    }

    private void OnEnable()
    {
        foreach (var enemy in _generatedEnemies)
        {
            enemy.IsDeactivated += OnEnemyDeactivated;
        }
    }

    private void OnDisable()
    {
        foreach (var enemy in _generatedEnemies)
        {
            enemy.IsDeactivated -= OnEnemyDeactivated;
        }
    }

    public void Deactivate()
    {
        _isDeactivated = true;

        foreach (var enemy in _generatedEnemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    private void OnEnemyDeactivated()
    {
        _playerKills++;

        IsPLayerKillsIncreased?.Invoke(_playerKills);        

        _currentEnemyCount--;

        if (_currentEnemyCount < _enemyCount)                    
            DOVirtual.DelayedCall(_respawnTime, SpawnEnemy);
        
    }

    private void GenerateAllEnemies()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            Vector3 position = _spawnPoints[i].position;

            Enemy randomPrefab = GetRandomEnemy(_enemyPrefabs);

            var newEnemy = Instantiate(randomPrefab, position, Quaternion.identity, _enemyParent);

            newEnemy.SetDefaultPosition(position);

            newEnemy.gameObject.SetActive(false);

            _generatedEnemies.Add(newEnemy);
        }
    }

    private void SpawnEnemy()
    {
        if (TryGetInactiveEnemy(out Enemy inactiveEnemy) && _currentEnemyCount <_enemyCount && _isDeactivated == false)
        {
            Vector2 randomOffsetPosition = UnityEngine.Random.insideUnitCircle * _spawnRadiusModifier;

            inactiveEnemy.transform.position += new Vector3(randomOffsetPosition.x, 0, randomOffsetPosition.y);

            inactiveEnemy.Restore();

            inactiveEnemy.gameObject.SetActive(true); 

            _currentEnemyCount++;

            if (_currentEnemyCount < _enemyCount)
                SpawnEnemy();
        }
    }

    private bool TryGetInactiveEnemy(out Enemy inactiveEnemy)
    {
        List<Enemy> inactiveEnemies = new();

        inactiveEnemy = null;

        foreach (var enemy in _generatedEnemies)
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