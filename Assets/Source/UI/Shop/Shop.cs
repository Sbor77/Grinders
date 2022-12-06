using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{    
    [SerializeField] private SkillBuyer _skillBuyer;
    [SerializeField] private StatsViewer _statsViewer;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private SceneLevelLoader _levelLoader;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _gameEndPanel;
    [SerializeField] private Button _gameEndButton;

    private int _lastLevelIndex = 4;
    private int _introSceneIndex = 0;

    private void Start()
    {
        _shopPanel.SetActive(true);

        _gameEndPanel.SetActive(false);

        GamesSdk.Instance.SetLeaderboardScore(DataHandler.Instance.GetSavedTotalScore());

        OnStatBought();

        GamesSdk.Instance.InterstitialAdShow();
    }

    private void OnEnable()
    {
        _skillBuyer.IsStatBought += OnStatBought;

        _exitButton.onClick.AddListener(CloseShop);

        _nextButton.onClick.AddListener(LoadNextLevel);

        _gameEndButton.onClick.AddListener(LoadMainMenu);
    }

    private void OnDisable()
    {
        _skillBuyer.IsStatBought -= OnStatBought;

        _exitButton.onClick.RemoveListener(CloseShop);

        _nextButton.onClick.RemoveListener(LoadNextLevel);

        _gameEndButton.onClick.RemoveListener(LoadMainMenu);
    }

    private void CloseShop()
    {
        _levelLoader.LoadLevel(_introSceneIndex);
    }

    private void LoadNextLevel()
    {
        int nextLevel = DataHandler.Instance.GetSavedLevel();

        if (nextLevel >= _lastLevelIndex)
        {
            _gameEndPanel.SetActive(true);

            _shopPanel.SetActive(false);
        }
        else
        {
            _levelLoader.LoadLevel(DataHandler.Instance.GetSavedLevel());
        }
    }

    private void LoadMainMenu()
    {
        _levelLoader.LoadLevel(_introSceneIndex);
    }

    private void OnStatBought()
    {
        DataHandler.Instance.SaveAllStats();

        _statsViewer.Init();

        _skillBuyer.Init();
    }
}