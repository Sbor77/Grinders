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

    private int _currentLevel;
    private int _ShopScene = 0;
    private int _levelOneScene = 1;
    private int _levelTwoScene = 2;
    private int _levelThreeScene = 3;
    private int _levelFourScene = 4;
    private string _SceneShop = "SceneShop";
    private QuestInfo _missionConditions;    
    private int _currentCoins;
    private int _currentKills;
    private float _currentHealth;
    private bool _isBigboxDestroyed;
    private bool _isBigboxDoorOpened;
    
    private void Start()
    {
        _missionConditions = _infoViewer.MissionConditions;

        LoadInitialStats();
    }

    private void OnEnable()
    {
        _infoViewer.IsCurrentConditionsChanged += OnCurrentConditionsChanged;
    }

    private void OnDisable()
    {
        _infoViewer.IsCurrentConditionsChanged -= OnCurrentConditionsChanged;
    }

    private void LoadInitialStats()
    {
        int level = DataHandler.Instance.GetSavedLevel();

        if (level == 0)
        {
            level = 1;
            DataHandler.Instance.SaveLevel(level);
        }

        int money = 0;
        DataHandler.Instance.SaveMoney(money);

        int kills = 0;
        DataHandler.Instance.SaveKills(kills);
    }

    private void OnCurrentConditionsChanged()
    {
        _currentCoins = _infoViewer.CurrentCoins;
        DataHandler.Instance.SaveMoney(_currentCoins);

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

        float cameraZoomTime = 2f;

        _cameraHandler.ZoomInPlayCamera();

        DOVirtual.DelayedCall(cameraZoomTime, () => _finalEffects.PlayAllEffects());
        DOVirtual.DelayedCall(_finalEffects.Duration, () => LoadShopScene());
    }

    private void LoadShopScene()
    {
        SaveProgress();
        SceneManager.LoadScene(_SceneShop);
    }

    private void SaveProgress()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        int money = DataHandler.Instance.GetSavedMoney() + _currentCoins;
        int kills = DataHandler.Instance.GetSavedKills() + _currentKills;

        DataHandler.Instance.SaveLevel(level);
        DataHandler.Instance.SaveMoney(money);
        DataHandler.Instance.SaveKills(kills);
        DataHandler.Instance.SaveAllStats();
    }
}