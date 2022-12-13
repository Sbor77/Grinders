using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPanelMeta : MonoBehaviour
{    
    [SerializeField] private InfoViewer _infoViewer;
    [SerializeField] private TMP_Text _regularKillsCount;
    [SerializeField] private TMP_Text _bossKillsCount;
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

    public void Activate()
    {
        Init();
        gameObject.SetActive(true);        
    }

    private void Init()
    {
        _regularKillsCount.text = _infoViewer.CurrentZoneKills.ToString();
        _bossKillsCount.text = _infoViewer.CurrentZoneBossKills.ToString();
        _metaEarnedCoinsCount.text = GetCurrentCoinsEarned().ToString();

        SaveEarnedMoney();
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
        int regularKills = 1;
        int bossKills = 10;
        int value = _infoViewer.CurrentZoneKills * regularKills + _infoViewer.CurrentZoneBossKills * bossKills;
        return value;
    }

    private void ReloadMetaGame()
    {
        GamesSdk.Instance.InterstitialAdShow();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}