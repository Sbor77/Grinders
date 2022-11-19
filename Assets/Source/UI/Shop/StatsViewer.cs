using TMPro;
using UnityEngine;

public class StatsViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _stageText;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _killedText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _speedText;
    
    public void Init()
    {
        _stageText.text = $"Level: {DataHandler.Instance.GetSavedLevel()}";
        _goldText.text = $"Money: {DataHandler.Instance.GetSavedMoney()}";
        _killedText.text = $"Killed: {DataHandler.Instance.GetSavedKills()}";
        _healthText.text = $"Health level: {DataHandler.Instance.GetSavedHealthSkill()}";
        _speedText.text = $"Speed level: {DataHandler.Instance.GetSavedSpeedSkill()}";
    }
}