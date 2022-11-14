using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class draft_Spawner : MonoBehaviour
{
    [SerializeField] protected List<SpawnedObject> _spawnedPrefabs;
    [SerializeField] protected List<Transform> _spawnPoints;    
    [SerializeField] protected Transform _spawnParent;
    [SerializeField] protected LayerMask _objectLayer;    
    //[SerializeField] protected float _respawnTime;
    [SerializeField] protected int _spawnObjectsCount;

    protected List<SpawnedObject> _spawnedObjects = new ();

    public int _currentSpawnedObjects;
    private float _spawnRadiusModifier = 1;

    private void Awake()
    {
        GenerateAllObjects();
    }

    private void Start()
    {
        for (int i = 0; i < _spawnObjectsCount; i++)
        {
            Spawn();
        }

        //SpawnBigBox();
    }

   /* private void OnEnable()
    {
        foreach (IActivable spawnedObject in _spawnedObjects)
        {
            spawnedObject.IsActiveStateChanged += OnActiveStateChanged;
        }
    }

    private void OnDisable()
    {
        foreach (IActivable spawnedObject in _spawnedObjects)
        {
            spawnedObject.IsActiveStateChanged -= OnActiveStateChanged;
        }
    }*/
    /*
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
        }*/

    public void Spawn()
    {
        if (TryGetInactiveObject(out SpawnedObject inactiveObject) && _currentSpawnedObjects < _spawnObjectsCount)         
        {
            Vector2 randomOffsetPosition = Random.insideUnitCircle * _spawnRadiusModifier;

            inactiveObject.transform.position += new Vector3(randomOffsetPosition.x, 0, randomOffsetPosition.y);

            //inactiveObject.Activate();

            //inactiveObject.gameObject.SetActive(true);

            _currentSpawnedObjects++;            

            if (_currentSpawnedObjects < _spawnObjectsCount)
                Spawn();
        }
    }

    private void OnActiveStateChanged()
    {
        print("Изменилось состояние объекта спаунера!");

        int activeObjectsCount = GetActiveObjectsCount();

        int inactiveObjectsCount = _spawnedObjects.Count - activeObjectsCount;

        if (_currentSpawnedObjects < activeObjectsCount)
        {
            Spawn();
        }        
    }


    /*
        private void OnCoinCollected()
        {
            _currentBoxCount--;

            if (_targetBoxesCount > _currentBoxCount)
                DOVirtual.DelayedCall(_respawnDelay, SpawnBox);
        }*/

    private void GenerateAllObjects()
    {
        for (int i = 0; i < _spawnObjectsCount; i++)
        {
            SpawnedObject randomPrefab = GetRandomSpawnObject(_spawnedPrefabs);

            var newObject = Instantiate(randomPrefab, _spawnPoints[i].position, Quaternion.identity, _spawnParent);

            //newObject.Deactivate();

            //newObject.gameObject.SetActive(false);

            _spawnedObjects.Add(newObject);
        }
    }

    private bool TryGetInactiveObject(out SpawnedObject inactiveObject)
    {
        List<SpawnedObject> inactiveObjects = new ();

        inactiveObject = null;

        foreach (var spawnedObject in _spawnedObjects)
        {            
          /*  if (spawnedObject.IsActive() == false)
                inactiveObjects.Add(spawnedObject);*/
        }

        if (inactiveObjects.Count > 0)
        {
            inactiveObject = GetRandomSpawnObject(inactiveObjects);
        }

        return inactiveObject != null;
    }

    private SpawnedObject GetRandomSpawnObject(List<SpawnedObject> sourceObjects)
    {
        return sourceObjects[Random.Range(0, sourceObjects.Count)];
    }

    private int GetActiveObjectsCount()
    {
        int count = 0;

        foreach (var spawnedObject in _spawnedObjects)
        {
            /*if (spawnedObject.IsActive() == true)
                count++;*/
        }

        return count;
    }
}