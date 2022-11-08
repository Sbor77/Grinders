using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Transform> _enemySpawnPoints;

    [SerializeField] private int _enemiesMaxCount;

    [SerializeField] private BoxSpawner _boxSpawner;

    
}
