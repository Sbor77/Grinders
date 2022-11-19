using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private Button _dieButton;
    [SerializeField] private Button _continueButton;

    private void OnEnable()
    {
        _dieButton.onClick.AddListener(OnDieClick);
        _continueButton.onClick.AddListener(OnContinueClick);
    }

    private void OnDisable()
    {
        _dieButton.onClick.RemoveListener(OnDieClick);
        _continueButton.onClick.RemoveListener(OnContinueClick);
    }

    private void OnContinueClick()
    {
        //something to do with advertising
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDieClick()
    {
        DataHandler.Instance.SaveLevel(0);
        DataHandler.Instance.SaveMoney(0);
        DataHandler.Instance.SaveKills(0);
        DataHandler.Instance.SaveHealthSkill(1);
        DataHandler.Instance.SaveSpeedSkill(1);
        DataHandler.Instance.SaveAllStats();

        SceneManager.LoadScene(0);
    }
}
