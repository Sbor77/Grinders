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

    private QuestInfo _missionConditions;
    private int _currentLevel;
    private int _ShopSceneIndex = 0;
    private int _levelOneSceneIndex = 1;
    private int _levelTwoSceneIndex = 2;
    private int _levelThreeSceneIndex = 3;
    private int _levelFourSceneIndex = 4;
    private int _currentCoins;
    private int _currentKills;
    private float _currentHealth;
    private bool _isBigboxDestroyed;
    private bool _isBigboxDoorOpened;

    private void Start()
    {
        _missionConditions = _infoViewer.MissionConditions;

        if (SceneManager.GetActiveScene().buildIndex == _levelOneSceneIndex)                    
            SaveDefaultStats();        
    }

    private void OnEnable()
    {
        _infoViewer.IsCurrentConditionsChanged += OnCurrentConditionsChanged;

        _player.IsDied += OnDyingPlayerScreen;

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        _infoViewer.IsCurrentConditionsChanged -= OnCurrentConditionsChanged;

        _player.IsDied -= OnDyingPlayerScreen;

        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnDyingPlayerScreen()
    {
        _deathPanel.Activate();
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        //print($"Перешли из сцены c индексом: {current.buildIndex} в сцену с индексом: {next.buildIndex}");
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

        if (IsBigBoxConditionsFulfilled() && _isBigboxDoorOpened == false)
        {
            float doorOpenDelay = 5.5f;

            _cameraHandler.ZoomInOutBigboxCamera();

            _enemySpawner.Deactivate();

            _boxSpawner.StopSpawn();

            _isBigboxDoorOpened = true;

            DOVirtual.DelayedCall(doorOpenDelay, () => _bigboxDoor.Open());
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
        DataHandler.Instance.SaveAllStats();

        // uncomment after testing

        /*float cameraZoomTime = 2f;

        _cameraHandler.ZoomInPlayCamera();

        DOVirtual.DelayedCall(cameraZoomTime, () => _finalEffects.PlayAllEffects());

        DOVirtual.DelayedCall(_finalEffects.Duration, () => LoadShopScene());*/

        
        // delete after testing
        LoadShopScene();
    }

    private void LoadShopScene()
    {
        SaveProgress();

        SceneManager.LoadScene(_ShopSceneIndex);
    }

    private void SaveProgress()
    {
        int level = SceneManager.GetActiveScene().buildIndex;

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