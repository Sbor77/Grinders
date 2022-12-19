using System.Collections.Generic;
using UnityEngine;

public class LevelZone : MonoBehaviour
{
    [SerializeField] private List<DoorOpener> _doors;
    [SerializeField] private List<Transform> _boxPoints;
    [SerializeField] private List<Transform> _enemyPoints;
    [SerializeField] private int _targetMoney;
    [SerializeField] private int _targetKills;
    [SerializeField] private int _maxBosses;
        
    public int TargetMoney => _targetMoney;
    public int TargetKills => _targetKills;
    public int MaxBosses => _maxBosses;
    public List<Transform> BoxPoints => _boxPoints;
    public List<Transform> EnemyPoints => _enemyPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            if (_doors != null)
                CloseDoors();            
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var enemy in _enemyPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(enemy.transform.position, 0.2f);
        }
    }

    public void AddBossTarget()
    {
        _maxBosses++;
    }    

    public void OpenDoors()
    {
        if (_doors != null)
        {
            foreach (var door in _doors)
            {
                door.Open();
            }
        }
    }

    private void CloseDoors()
    {
        if (_doors != null)
        {
            foreach (var door in _doors)
            {
                door.Close();
            }
        }
    }
}