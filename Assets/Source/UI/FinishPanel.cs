using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private Button _shopButton;
    [SerializeField] private InfoViewer _infoViewer;

    private int _shopSceneIndex = 5;
    
    private void OnEnable()
    {
        _shopButton.onClick.AddListener(LoadShopScene);
    }

    private void OnDisable()
    {
        _shopButton.onClick.RemoveListener(LoadShopScene);
    }

    public void Init()
    {
        string statsLevel = $"Level money - {_infoViewer.CurrentCoins.ToString()}\n" +
                            $"Kills enemy - {_infoViewer.CurrentKills.ToString()}\n" +
                            $"Level Score - {GetCurrentScore()}\n " +
                            $"Total Score - {GetTotalScore()}";
        _infoText.text = statsLevel;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private int GetTotalScore()
    {
        return 0;
    }

    private int GetCurrentScore()
    {
        return 0;
    }

    private void LoadShopScene()
    {
        SceneManager.LoadScene(_shopSceneIndex);
    }
}
