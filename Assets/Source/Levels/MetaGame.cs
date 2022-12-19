using DG.Tweening;
using Lean.Localization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaGame : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LevelZone _zone;
    [SerializeField] private StartPanelMeta _startPanelMeta;
    [SerializeField] private Button _startButton;    
    [SerializeField] private InfoViewer _infoViewer;        
    [SerializeField] private FinishPanelMeta _finishPanelMeta;    
    [SerializeField] private List<DoorOpener> _doors;

    private int _bossSpawnInterval = 10;
    private float _doorOpenDelay = 1;    
    private int _currentRegularKills;    

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartGame);
        _player.IsDied += OnPlayerDied;
        _infoViewer.IsCurrentConditionsChanged += OnCurrentConditionsChanged;
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartGame);
        _player.IsDied -= OnPlayerDied;
        _infoViewer.IsCurrentConditionsChanged -= OnCurrentConditionsChanged;
    }

    private void Start()
    {
        LeanLocalization.SetCurrentLanguageAll(DataHandler.Instance.GetSavedLanguage());
        _startPanelMeta.Activate();             
        Time.timeScale = 0;        
    }

    private void StartGame()
    {
        Time.timeScale = 1;
        _startPanelMeta.Deactivate();
        DOVirtual.DelayedCall(_doorOpenDelay, OpenDoors);
    }

    private void OnPlayerDied()
    {
        _finishPanelMeta.Activate();        
    }

    private void OnCurrentConditionsChanged()
    {
        _currentRegularKills = _infoViewer.CurrentZoneKills;        

        if (_currentRegularKills % _bossSpawnInterval == 0)                    
            _zone.AddBossTarget();        

        DataHandler.Instance.SaveKills(_currentRegularKills);        
    }

    private void OpenDoors()
    {
        foreach (var door in _doors)
        {
            door.Open();
        }
    }
}