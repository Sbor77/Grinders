using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Lean.Localization;

public class Level : MonoBehaviour
{    
    [SerializeField] private Player _player;
    [SerializeField] private InfoViewer _infoViewer;
    [SerializeField] private EffectHandler _finalEffects;
    [SerializeField] private DoorOpener _bigboxDoor;
    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private BoxSpawner _boxSpawner;
    [SerializeField] private FinishPanel _finishPanel;
    [SerializeField] private DeathPanel _deathPanel;
    [SerializeField] private List<LevelZone> _zones;
    [SerializeField] private List<Transform> _playerStartPoints;

    private QuestInfo _missionConditions;
    private int _currentZoneIndex;    
    private int _levelOneSceneIndex = 1;
    private int _currentCoins;
    private int _currentKills;    
    private bool _isBigboxDestroyed;
    private bool _isBigboxDoorOpened;
    private bool _isCheated;

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

    private void Start()
    {
        LeanLocalization.SetCurrentLanguageAll(DataHandler.Instance.GetSavedLanguage());
        _missionConditions = _infoViewer.MissionConditions;

        if (SceneManager.GetActiveScene().buildIndex == _levelOneSceneIndex)
            SaveDefaultStats();

        _player.Init(DataHandler.Instance.GetSavedHealthSkill(), DataHandler.Instance.GetSavedSpeedSkill());

        InitZones();
        InitInfoViewer();
        SetPlayerPosition();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            _isCheated = true;
    }

    private void InitInfoViewer()
    {
        int targetCoins = _zones[_currentZoneIndex].TargetMoney;
        int targetKills = _zones[_currentZoneIndex].TargetKills;
        bool bigboxActive = _currentZoneIndex == _zones.Count - 1;

        QuestInfo conditions = new QuestInfo(targetCoins, targetKills, bigboxActive);

        _infoViewer.SetCurrentZoneTargets(conditions);
    }

    private void SetPlayerPosition()
    {
        if (_playerStartPoints != null)
        {
            int currentZoneIndex = DataHandler.Instance.GetSavedCurrentZone();
            _player.transform.position = _playerStartPoints[currentZoneIndex].position;
        }
    }

    private void OnDyingPlayerScreen()
    {
        _deathPanel.Activate();
    }

    private void SaveDefaultStats()
    {
        int defaultTotalScore = 0;
        int defaultLevel = 1;
        int defaultHealthSkill = 1;
        int defaultSpeedSkill = 1;
        int defaultRadiusSkill = 1;
        int defaultKills = 0;
        int defaultTotalMoney = 0;
        int defaultLevelMoney = 0;

        DataHandler.Instance.SaveTotalScore(defaultTotalScore);
        DataHandler.Instance.SaveLevel(defaultLevel);
        DataHandler.Instance.SaveHealthSkill(defaultHealthSkill);
        DataHandler.Instance.SaveSpeedSkill(defaultSpeedSkill);
        DataHandler.Instance.SaveRadiusSkill(defaultRadiusSkill);
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

            DOVirtual.DelayedCall(doorOpenDelay, () => 
            {
                _bigboxDoor.Open();
                _boxSpawner.UnshadeBigbox();
            });

            _isBigboxDoorOpened = true;
        }

        if (_isBigboxDestroyed)
            ShowEndLevelScenario();
    }

    private bool IsBigBoxConditionsFulfilled()
    {
        bool conditions =
            _currentCoins >= _missionConditions.NeedCoinCollected &&
            _currentKills >= _missionConditions.NeedEnemyKilled || _isCheated;

        return conditions;
    }

    private void ShowEndLevelScenario()
    {
        float cameraZoomTime = 2f;
        float openShopDelay = 3f;

        SaveProgress();
        _cameraHandler.ZoomInPlayCamera();

        DOVirtual.DelayedCall(cameraZoomTime, () => _finalEffects.PlayAllEffects());

        DOVirtual.DelayedCall(openShopDelay + _finalEffects.Duration, () =>
        {
            _finishPanel.Init();
            _finishPanel.Activate();
        });
    }

    private void InitZones()
    {        

        _currentZoneIndex = DataHandler.Instance.GetSavedCurrentZone();

        ActivateZone(_currentZoneIndex);
    }

    /*private void InitZones()
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
                _zones[i].Init(targetMoney, targetKills);
            else
                _zones[i].Init(accumulatedMoney, accumulatedKills);
        }

        _currentZoneIndex = DataHandler.Instance.GetSavedCurrentZone();
        ActivateZone(_currentZoneIndex);        
    }*/

    private void ActivateZone(int zoneIndex)
    {
        LevelZone activatedZone = _zones[zoneIndex];
        activatedZone.Activate();

        for (int i = 0; i < _zones.Count; i++)
        {
            if (i != zoneIndex)
                DeactivateZone(i);
        }

        _boxSpawner.SetZoneIndex(_currentZoneIndex);
        _enemySpawner.SetZoneIndex(_currentZoneIndex);
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
            _boxSpawner.SetZoneIndex(_currentZoneIndex);
            _enemySpawner.SetZoneIndex(_currentZoneIndex);
        }        

        DataHandler.Instance.SaveCurrentZone(_currentZoneIndex);
    }

    private bool IsZoneCompleted (int zoneIndex)
    {
        if (_currentKills >= _zones[zoneIndex].TargetKills && _currentCoins >= _zones[zoneIndex].TargetMoney || _isCheated)        
            return true;
        else
            return false;
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