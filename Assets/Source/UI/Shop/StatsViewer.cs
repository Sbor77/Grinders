using TMPro;
using UnityEngine;

public class StatsViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentLevel;
    [SerializeField] private TMP_Text _totalCoins;    
    [SerializeField] private TMP_Text _healthLevel;
    [SerializeField] private TMP_Text _speedLevel;
    [SerializeField] private TMP_Text _radiusLevel;

    public void Init()
    {
        _currentLevel.text = DataHandler.Instance.GetSavedLevel().ToString();

        _totalCoins.text = DataHandler.Instance.GetSavedTotalMoney().ToString();

        _healthLevel.text = DataHandler.Instance.GetSavedHealthSkill().ToString();

        _speedLevel.text = DataHandler.Instance.GetSavedSpeedSkill().ToString();

        //_radiusLevel.text = DataHandler.Instance.GetSavedSpeedSkill().ToString();
    }
}