using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPanelMeta : MonoBehaviour
{    
    [SerializeField] private TMP_Text _metaKillsCount; 
    [SerializeField] private TMP_Text _metaEarnedCoinsCount;
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
        _metaKillsCount.text = _infoViewer.CurrentKills.ToString();
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

    private void SaveEarnedMoney()
    {
        int value = DataHandler.Instance.GetSavedTotalMoney() + GetCurrentCoinsEarned();
        DataHandler.Instance.SaveTotalMoney(value);        
    }

    private int GetCurrentCoinsEarned()
    {        
        int killToCoinIndex = 1;

        int value = _infoViewer.CurrentKills * killToCoinIndex;
        return value;
    }

    private void LoadShopScene()
    {
        SceneManager.LoadScene(_shopSceneIndex);
    }
}