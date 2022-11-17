using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private InfoViewer _infoViewer;
    [SerializeField] private EffectHandler _finalEffects;
    [SerializeField] private DoorOpener _bigboxDoor;
    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private EnemySpawner _enemySpawner;

    private QuestInfo _missionConditions;    
    private int _currentCoins;
    private int _currentKills;
    private float _currentHealth;
    private bool _isBigboxDestroyed;

    private void Start()
    {
        _missionConditions = _infoViewer.MissionConditions;
    }

    private void OnEnable()
    {
        _infoViewer.IsCurrentConditionsChanged += OnCurrentConditionsChanged;
    }

    private void OnDisable()
    {
        _infoViewer.IsCurrentConditionsChanged -= OnCurrentConditionsChanged;
    }

    private void OnCurrentConditionsChanged()
    {
        _currentCoins = _infoViewer.CurrentCoins;

        _currentKills = _infoViewer.CurrentKills;

        _currentHealth = _infoViewer.CurrentHealth;

        _isBigboxDestroyed = _infoViewer.IsBigboxDestroyed;

        if (IsMissionConditionsFulfilled())
        {
            print("Уровень пройден!");

            ShowEndLevelScenario();
        }
    }

    private bool IsMissionConditionsFulfilled()
    {
        bool conditions =
            _currentCoins >= _missionConditions.NeedCoinCollected &&
            _currentKills >= _missionConditions.NeedEnemyKilled;
            //_isBigboxDestroyed == _missionConditions.NeedDestroyBigBox &&
            //_currentHealth > 0;

        return conditions;
    }

    private void ShowEndLevelScenario()
    {
        _cameraHandler.ZoomOutBigboxCamera();

        _enemySpawner.Deactivate();        

        DOVirtual.DelayedCall(2f, () =>
        { 
            _bigboxDoor.Open();
            _finalEffects.PlayAllEffects();
        });
    }
}
