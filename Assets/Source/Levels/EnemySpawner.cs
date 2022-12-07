using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<LevelZone> _zones;
    [SerializeField] private List<int> _maxCounts;
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private Transform _enemyParent;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _respawnTime;
    [Space]
    [SerializeField] private ParticleSystem _burstPrefab;
    [SerializeField] private ParticleSystem _trailPrefab;
    [SerializeField] private Transform _effectsParent;
    [SerializeField] private int _effectsCount;
    [SerializeField] private AudioSource _trailSound;

    List<ParticleSystem> _bursts;
    List<ParticleSystem> _trails;
    private float _startTrailHeight = 25;
    private float _startBurstHeight = 2.75f;
    private float _trailDuration = 0.3f;
    private float _burstDuration = 0.5f;

    List<Enemy>[] _enemyArray;
    private LevelZone _currentZone;
    private int _currentZoneIndex;
    private int _playerKills;
    private float _spawnRadiusModifier = 1;
    private bool _isDeactivated;

    public event Action <int> IsPLayerKillsIncreased;

    private void Awake()
    {
        _bursts = new List<ParticleSystem>();

        _trails = new List<ParticleSystem>();

        _currentZoneIndex = 0;

        _currentZone = _zones[_currentZoneIndex];

        _enemyArray = new List<Enemy>[_zones.Count];

        GenerateAllEffects();

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

    public void SetZoneIndex(int index)
    {
        _currentZoneIndex = index;

        _currentZone = _zones[_currentZoneIndex];

        DeactivateEnemiesInRestZones();

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

        if (GetCurrentEnemyCount() < GetMaxEnemyCount())
            DOVirtual.DelayedCall(_respawnTime - (_trailDuration + _burstDuration), SpawnEnemy);

        DeactivateEnemiesInRestZones();
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

                //newEnemy.SetDefaultPosition(position);

                newEnemy.gameObject.SetActive(false);

                _enemyArray[i].Add(newEnemy);
            }
        }
    }

    private void SpawnEnemy()
    {
        var enemies = _enemyArray[_currentZoneIndex];

        if (TryGetInactiveEnemy(enemies, out Enemy inactiveEnemy) && GetCurrentEnemyCount() < GetMaxEnemyCount() && _isDeactivated == false)
        {
            Vector3 freePoint = GetFreePointToSpawn(_currentZone);
                        
            Vector2 randomOffsetPosition = UnityEngine.Random.insideUnitCircle * _spawnRadiusModifier;

            Vector3 spawnPosition = freePoint  + new Vector3(randomOffsetPosition.x, 0, randomOffsetPosition.y);

            inactiveEnemy.transform.position = spawnPosition;

            ShowSpawnEffects(spawnPosition);

            DOVirtual.DelayedCall(_burstDuration + _trailDuration, () =>
            { 
                inactiveEnemy.gameObject.SetActive(true);
                inactiveEnemy.Restore();

                if (GetCurrentEnemyCount() < GetMaxEnemyCount())
                    SpawnEnemy();
            });

        }
    }

    private void DeactivateEnemiesInRestZones()
    {
        for (int i = 0; i < _zones.Count; i++)
        {
            if (i != _currentZoneIndex)
            {
                foreach (var enemy in _enemyArray[i])
                {
                    enemy.gameObject.SetActive(false);
                }
            }
        }
    }

    private int GetCurrentEnemyCount()
    {
        int count = 0;

        foreach (var enemy in _enemyArray[_currentZoneIndex])
        {
            if (enemy.gameObject.activeSelf == true)
                count++;
        }

        return count;
	}

    private int GetMaxEnemyCount()
    {
        return _maxCounts[_currentZoneIndex];
    }

    private Vector3 GetFreePointToSpawn(LevelZone zone)
    {
        Vector3 freePosition = GetRandomPosition(zone.EnemyPoints);
                
        List<Vector3> freePositions = new();

        foreach (var point in zone.EnemyPoints)
        {
            if (Physics.CheckSphere(point.position, 0.5f, _enemyLayer) == false)
                freePositions.Add(point.position);
        }

        if (freePositions.Count > 0)
            freePosition = freePositions[UnityEngine.Random.Range(0, freePositions.Count)];

        return freePosition;
    }

    private Vector3 GetRandomPosition (List<Transform> points)
    {
        int randomIndex = UnityEngine.Random.Range(0, points.Count);

        return points[randomIndex].position;
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

    private void GenerateAllEffects()
    {
        GenerateEffects(_burstPrefab, _bursts, _effectsCount);

        GenerateEffects(_trailPrefab, _trails, _effectsCount);
    }

    private void GenerateEffects(ParticleSystem effectPrefab, List<ParticleSystem> sourceList, int count)
    {
        for (int  i = 0;  i < count;  i++)
        {
            var newEffect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity, _effectsParent);

            newEffect.gameObject.SetActive(false);

            sourceList.Add(newEffect);
        }
    }

    private void ShowSpawnEffects(Vector3 position)
    {
        ParticleSystem trail = _trails.Find(x => x.gameObject.activeSelf == false);

        ParticleSystem burst = _bursts.Find(x => x.gameObject.activeSelf == false);
        
        trail.transform.position = new Vector3(position.x, _startTrailHeight, position.z);

        burst.transform.position = new Vector3(position.x, _startBurstHeight, position.z);
        
        Sequence effectSequence = DOTween.Sequence();

        effectSequence.AppendCallback(() => 
        {
            trail.gameObject.SetActive(true);
            _trailSound.Play();
        });
        effectSequence.Append(trail.transform.DOMoveY(0, _trailDuration));
        effectSequence.AppendInterval(_trailDuration);
        effectSequence.AppendCallback(() =>
        {            
            trail.gameObject.SetActive(false);
            burst.gameObject.SetActive(true);
        });
        effectSequence.AppendInterval(_burstDuration + 1);
        effectSequence.AppendCallback(() => burst.gameObject.SetActive(false));
        effectSequence.AppendCallback(() => effectSequence.Kill());
    }
}