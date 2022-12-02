using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private Button _dieButton;
    [SerializeField] private Button _continueButton;

    private bool _rewarded;

    public void Activate()
    {
        gameObject.SetActive(true);
        _rewarded = false;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _dieButton.onClick.AddListener(OnDieClick);
        _continueButton.onClick.AddListener(OnContinueClick);
        GamesSdk.Instance.Rewarded += OnRewarded;
        GamesSdk.Instance.AdVideoClosed += OnContinueWithRewarded;
    }

    private void OnDisable()
    {
        _dieButton.onClick.RemoveListener(OnDieClick);
        _continueButton.onClick.RemoveListener(OnContinueClick);
        GamesSdk.Instance.Rewarded -= OnRewarded;
        GamesSdk.Instance.AdVideoClosed -= OnContinueWithRewarded;
    }

    private void OnContinueClick()
    {
        GamesSdk.Instance.VideoAdShow();
    }

    private void OnRewarded()
    {
        _rewarded = true;
    }

    private void OnContinueWithRewarded()
    {
        if (_rewarded)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDieClick()
    {
        DataHandler.Instance.SaveLevel(1);
        DataHandler.Instance.SaveLevelMoney(0);
        DataHandler.Instance.SaveKills(0);
        DataHandler.Instance.SaveHealthSkill(1);
        DataHandler.Instance.SaveSpeedSkill(1);
        DataHandler.Instance.SaveAllStats();

        SceneManager.LoadScene(0);
    }
}
