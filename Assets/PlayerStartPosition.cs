using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class PlayerStartPosition : MonoBehaviour
{
    private NavMeshObstacle _startPositionObstacle;
    private float _time = 5;

    private void Awake()
    {
        _startPositionObstacle = GetComponent<NavMeshObstacle>();
    }

    private void Start()
    {
        Invoke(nameof(DisableObstacle), _time);
    }

    private void DisableObstacle()
    {
        _startPositionObstacle.enabled = false;
    }
}