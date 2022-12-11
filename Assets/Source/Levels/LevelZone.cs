using System.Collections.Generic;
using UnityEngine;

public class LevelZone : MonoBehaviour
{
    [SerializeField] private List<DoorOpener> _doors;
    [SerializeField] private List<Transform> _boxPoints;    
    [SerializeField] private List<Transform> _enemyPoints;

    private int _targetMoney;
    private int _targetKills;
    private bool _isActive;

    public bool IsActive => _isActive;
    public int TargetMoney => _targetMoney;
    public int TargetKills => _targetKills;
    public List<Transform> BoxPoints => _boxPoints;
    public List<Transform> EnemyPoints => _enemyPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            if (_doors != null)
                CloseDoors();

            _isActive = true;
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

    public void Init (int targetMoney, int targetKills)
    {
        _targetMoney = targetMoney;
        _targetKills = targetKills;
        _isActive = false;
    }

    public void Activate()
    {
        _isActive = true;
        OpenDoors();
    }

    public void Deactivate()
    {
        _isActive = false;
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

    public void CloseDoors()
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