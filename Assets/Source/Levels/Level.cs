using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private InfoViewer _infoViewer;
    [SerializeField] private EffectHandler _finalEffects;
    [SerializeField] private DoorOpener _bigboxDoor;

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

        print($"������� �����: {_currentCoins}, ����� ������: {_currentKills}, ��������: {_currentHealth}, ������� ������� ��������? {_isBigboxDestroyed}");
    }
}
