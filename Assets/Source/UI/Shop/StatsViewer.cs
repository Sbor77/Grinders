using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _stageText;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _killedText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _speedText;

    public void Init(StatsInfo info)
    {
        _stageText.text = $"Stage: {info.Stage}";
        _goldText.text = $"Gold: {info.Golds}";
        _killedText.text = $"Killed: {info.Kills}";
        _healthText.text = $"Health level: {info.Health}";
        _speedText.text = $"Speed level: {info.MoveSpeed}";
    }
}
