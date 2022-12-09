using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{    
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private SceneLevelLoader _levelLoaderPanel;
    [SerializeField] private LeadersPanel _leadersPlanel;

    private int _startLevelIndex = 1;    

    private void OnEnable()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
        _continueButton.onClick.AddListener(ContinueGame);
        _leaderboardButton.onClick.AddListener(ShowLeaderboard);
    }

    private void OnDisable()
    {
        _newGameButton.onClick.RemoveListener(StartNewGame);
        _continueButton.onClick.RemoveListener(ContinueGame);
        _leaderboardButton.onClick.RemoveListener(ShowLeaderboard);
    }

    private void Start()
    {
        if (DataHandler.Instance.GetSavedLevel() <= _startLevelIndex)
            _continueButton.interactable = false;

        ActivateLeaderboard();
    }

    private void ActivateLeaderboard()
    {
        if (YandexGamesSdk.IsInitialized)
        {
            Leaderboard.GetEntries(GamesSdk.Instance.LeaderboardName, (result) =>
            {
                if (result.entries.Length > 0)
                    _leaderboardButton.interactable = true;
                else
                    _leaderboardButton.interactable = false;
            });
        }
    }

    private void StartNewGame()
    {
        int muteState = DataHandler.Instance.GetSavedMuteValue();
        float totalVolume = DataHandler.Instance.GetSavedTotalVolume();
        float musicVolume = DataHandler.Instance.GetSavedMusicVolume();
        string language = DataHandler.Instance.GetSavedLanguage();

        DataHandler.Instance.DeleteAllStats();

        int defaultLevel = 1;
        int defaultHealthSkill = 1;
        int defaultSpeedSkill = 1;
        int defaultRadiusSkill = 1;
        int defaultKills = 0;
        int defaultTotalMoney = 0;
        int defaultLevelMoney = 0;

        DataHandler.Instance.SaveLevel(defaultLevel);
        DataHandler.Instance.SaveHealthSkill(defaultHealthSkill);
        DataHandler.Instance.SaveSpeedSkill(defaultSpeedSkill);
        DataHandler.Instance.SaveRadiusSkill(defaultRadiusSkill);
        DataHandler.Instance.SaveKills(defaultKills);
        DataHandler.Instance.SaveTotalMoney(defaultTotalMoney);
        DataHandler.Instance.SaveLevelMoney(defaultLevelMoney);
        DataHandler.Instance.SaveMuteValue(muteState);
        DataHandler.Instance.SaveTotalVolume(totalVolume);
        DataHandler.Instance.SaveMusicVolume(musicVolume);
        DataHandler.Instance.SaveLanguage(language);
        DataHandler.Instance.SaveAllStats();
        
        _levelLoaderPanel.Load(_startLevelIndex);
    }

    private void ContinueGame()
    {
        int currentLevel = DataHandler.Instance.GetSavedLevel();
        _levelLoaderPanel.Load(currentLevel);
    }

    private void ShowLeaderboard()
    {
        _leadersPlanel.gameObject.SetActive(true);
    }
}