using TMPro;
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
    [SerializeField] private TMP_Text _totalScore;

    private int _lastLevelIndex = 4;
    private int _introSceneIndex = 0;

    private void OnEnable()
    {
        _skillBuyer.IsStatBought += OnStatBought;
        _exitButton.onClick.AddListener(ExitShop);
        _nextButton.onClick.AddListener(BackToGame);
        _gameEndButton.onClick.AddListener(LoadMainMenu);
    }

    private void OnDisable()
    {
        _skillBuyer.IsStatBought -= OnStatBought;
        _exitButton.onClick.RemoveListener(ExitShop);
        _nextButton.onClick.RemoveListener(BackToGame);
        _gameEndButton.onClick.RemoveListener(LoadMainMenu);
    }

    private void Start()
    {
        _shopPanel.SetActive(true);
        _gameEndPanel.SetActive(false);
        OnStatBought();        
        GamesSdk.Instance.SetLeaderboardScore(DataHandler.Instance.GetSavedTotalScore());
        GamesSdk.Instance.InterstitialAdShow();        
    }

    private void ExitShop()
    {
        _levelLoader.Load(_introSceneIndex);
    }

    private void BackToGame()
    {
        int currentLevel = DataHandler.Instance.GetSavedLevel();
        int completedLevel = DataHandler.Instance.GetSavedCompletedLevel();
        int levelToLoad = currentLevel == completedLevel ? currentLevel + 1 : currentLevel;        

        if (levelToLoad > _lastLevelIndex)
        {
            _gameEndPanel.SetActive(true);
            _totalScore.text = DataHandler.Instance.GetSavedTotalScore().ToString();
            _shopPanel.SetActive(false);
        }
        else
        {
            _levelLoader.Load(levelToLoad);
        }
    }

    private void LoadMainMenu()
    {
        _levelLoader.Load(_introSceneIndex);
    }

    private void OnStatBought()
    {
        DataHandler.Instance.SaveAllStats();
        _statsViewer.Init();
        _skillBuyer.Init();
    }
}