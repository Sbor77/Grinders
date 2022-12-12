using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPanelMeta : MonoBehaviour
{    
    [SerializeField] private InfoViewer _infoViewer;
    [SerializeField] private TMP_Text _metaKillsCount; 
    [SerializeField] private TMP_Text _metaEarnedCoinsCount;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _againButton;

    private int _shopSceneIndex = 5;
    private int _introSceneIndex = 0;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(Exit);
        _againButton.onClick.AddListener(ReloadMetaGame);
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(Exit);
        _againButton.onClick.RemoveListener(ReloadMetaGame);
    }

    public void Init()
    {
        _metaKillsCount.text = _infoViewer.LevelKills.ToString();
        _metaEarnedCoinsCount.text = GetCurrentCoinsEarned().ToString();

        SaveEarnedMoney();
    }

    public void Activate()
    {
        Init();
        gameObject.SetActive(true);        
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Exit()
    {
        int currentLevel = DataHandler.Instance.GetSavedLevel();
        int sceneToLoad = currentLevel == _introSceneIndex ? currentLevel : _shopSceneIndex;

        SceneManager.LoadScene(sceneToLoad);
    }

    private void SaveEarnedMoney()
    {
        int value = DataHandler.Instance.GetSavedTotalMoney() + GetCurrentCoinsEarned();
        DataHandler.Instance.SaveTotalMoney(value);        
    }

    private int GetCurrentCoinsEarned()
    {        
        int killToCoinIndex = 1;
        int value = _infoViewer.LevelKills * killToCoinIndex;
        return value;
    }

    private void ReloadMetaGame()
    {
        GamesSdk.Instance.InterstitialAdShow();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}