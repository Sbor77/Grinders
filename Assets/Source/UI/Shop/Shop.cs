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

    private int _introSceneIndex = 0;

    private void Start()
    {        
        GamesSdk.Instance.SetLeaderboardScore(DataHandler.Instance.GetSavedTotalScore());
        OnStatBought();
        GamesSdk.Instance.InterstitialAdShow();
    }

    private void OnEnable()
    {
        _skillBuyer.IsStatBought += OnStatBought;

        _exitButton.onClick.AddListener(CloseShop);

        _nextButton.onClick.AddListener(NextLevel);
    }

    private void OnDisable()
    {
        _skillBuyer.IsStatBought -= OnStatBought;

        _exitButton.onClick.RemoveListener(CloseShop);

        _nextButton.onClick.RemoveListener(NextLevel);
    }

    private void CloseShop()
    {
        _levelLoader.LoadLevel(_introSceneIndex);
    }

    private void NextLevel()
    {
        _levelLoader.LoadLevel(DataHandler.Instance.GetSavedLevel());
    }

    private void OnStatBought()
    {
        DataHandler.Instance.SaveAllStats();

        _statsViewer.Init();

        _skillBuyer.Init();
    }
}