using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{    
    [SerializeField] private Player _player;
    [SerializeField] private InfoViewer _infoViewer;
    [SerializeField] private EffectHandler _finalEffects;
    [SerializeField] private DoorOpener _bigboxDoor;
    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private BoxSpawner _boxSpawner;
    [SerializeField] private DeathPanel _deathPanel;
    [SerializeField] private List<LevelZone> _zones;

    private QuestInfo _missionConditions;
    private int _currentZoneIndex;
    private int _shopSceneIndex = 5;
    private int _introSceneIndex = 0;
    private int _levelOneSceneIndex = 1;
    private int _levelTwoSceneIndex = 2;
    private int _levelThreeSceneIndex = 3;
    private int _levelFourSceneIndex = 4;
    private int _currentCoins;
    private int _currentKills;    
    
    private bool _isBigboxDestroyed;
    private bool _isBigboxDoorOpened;

    private void Start()
    {
        _missionConditions = _infoViewer.MissionConditions;

        if (SceneManager.GetActiveScene().buildIndex == _levelOneSceneIndex)
        {
            SaveDefaultStats();

            _player.Init(DataHandler.Instance.GetSavedHealthSkill(), DataHandler.Instance.GetSavedSpeedSkill());
        }

        InitZones();
    }

    private void OnEnable()
    {
        _infoViewer.IsCurrentConditionsChanged += OnCurrentConditionsChanged;

        _player.IsDied += OnDyingPlayerScreen;        
    }

    private void OnDisable()
    {
        _infoViewer.IsCurrentConditionsChanged -= OnCurrentConditionsChanged;

        _player.IsDied -= OnDyingPlayerScreen;        
    }

    private void OnDyingPlayerScreen()
    {
        _deathPanel.Activate();
    }

    private void SaveDefaultStats()
    {
        int defaultLevel = 1;
        int defaultHealthSkill = 1;
        int defaultSpeedSkill = 1;
        int defaultKills = 0;
        int defaultTotalMoney = 0;
        int defaultLevelMoney = 0;

        DataHandler.Instance.SaveLevel(defaultLevel);

        DataHandler.Instance.SaveHealthSkill(defaultHealthSkill);

        DataHandler.Instance.SaveSpeedSkill(defaultSpeedSkill);

        DataHandler.Instance.SaveKills(defaultKills);

        DataHandler.Instance.SaveTotalMoney(defaultTotalMoney);

        DataHandler.Instance.SaveLevelMoney(defaultLevelMoney);
    }

    private void OnCurrentConditionsChanged()
    {
        _currentCoins = _infoViewer.CurrentCoins;
        DataHandler.Instance.SaveLevelMoney(_currentCoins);

        _currentKills = _infoViewer.CurrentKills;
        DataHandler.Instance.SaveKills(_currentKills);

        _isBigboxDestroyed = _infoViewer.IsBigboxDestroyed;

        ActivateNextZone();

        if (IsBigBoxConditionsFulfilled() && _isBigboxDoorOpened == false)
        {
            float doorOpenDelay = 3f;

            _cameraHandler.ZoomInOutBigboxCamera();

            _enemySpawner.Deactivate();

            _boxSpawner.StopSpawn();

            DOVirtual.DelayedCall(doorOpenDelay, () => _bigboxDoor.Open());

            _isBigboxDoorOpened = true;
        }

        if (_isBigboxDestroyed)
            ShowEndLevelScenario();
    }

    private bool IsBigBoxConditionsFulfilled()
    {
        bool conditions =
            _currentCoins >= _missionConditions.NeedCoinCollected &&
            _currentKills >= _missionConditions.NeedEnemyKilled;

        return conditions;
    }

    private void ShowEndLevelScenario()
    {
        float cameraZoomTime = 2f;
        float openShopDelay = 3f;

        _cameraHandler.ZoomInPlayCamera();

        DOVirtual.DelayedCall(cameraZoomTime, () => _finalEffects.PlayAllEffects());

        DOVirtual.DelayedCall(openShopDelay + _finalEffects.Duration, () => LoadShopScene());        
    }

    private void InitZones()
    {
        int targetKills = _missionConditions.NeedEnemyKilled;
        int targetMoney = _missionConditions.NeedCoinCollected;
        int zoneKills = targetKills / _zones.Count;
        int zoneMoney = targetMoney / _zones.Count;

        for (int i = 0; i < _zones.Count; i++)
        {
            int accumulatedMoney = zoneMoney + (zoneMoney * i);            
            int accumulatedKills = zoneKills + (zoneKills * i);

            if (i == _zones.Count - 1)
            {
                _zones[i].Init(targetMoney, targetKills);
            }
            else
            {
                _zones[i].Init(accumulatedMoney, accumulatedKills);
            }
        }

        _currentZoneIndex = 0;

        ActivateZone(_currentZoneIndex);
    }

    private void ActivateZone(int zoneIndex)
    {
        LevelZone activatedZone = _zones[zoneIndex];

        activatedZone.Activate();
    }

    private void DeactivateZone(int zoneIndex)
    {
        LevelZone deactivatedZone = _zones[zoneIndex];

        deactivatedZone.Deactivate();
    }

    private void ActivateNextZone()
    {
        if (IsZoneCompleted(_currentZoneIndex) && _currentZoneIndex < _zones.Count - 1)
        {
            DeactivateZone(_currentZoneIndex);

            ActivateZone(++_currentZoneIndex);

            _boxSpawner.SetZoneIndex(_zones[_currentZoneIndex]);

            _enemySpawner.SetZoneIndex(_zones[_currentZoneIndex]);
        }
    }

    private bool IsZoneCompleted (int zoneIndex)
    {
        if (_currentKills >= _zones[zoneIndex].TargetKills && _currentCoins >= _zones[zoneIndex].TargetMoney)        
            return true;        
        else        
            return false;        
    }

    private void LoadShopScene()
    {
        SaveProgress();

        SceneManager.LoadScene(_shopSceneIndex);
    }

    private void SaveProgress()
    {
        int level = SceneManager.GetActiveScene().buildIndex + 1;

        int levelMoney = DataHandler.Instance.GetSavedLevelMoney();

        int totalMoney = DataHandler.Instance.GetSavedTotalMoney() + levelMoney;

        int kills = DataHandler.Instance.GetSavedKills();

        DataHandler.Instance.SaveLevel(level);

        DataHandler.Instance.SaveLevelMoney(levelMoney);

        DataHandler.Instance.SaveTotalMoney(totalMoney);

        DataHandler.Instance.SaveKills(kills);

        DataHandler.Instance.SaveAllStats();
    }
}