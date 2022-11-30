using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelCoinsCount;
    [SerializeField] private TMP_Text _levelKillsCount;
    [SerializeField] private TMP_Text _levelScoreCount;
    [SerializeField] private TMP_Text _totalScoreCount;
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
        _levelCoinsCount.text = _infoViewer.CurrentCoins.ToString();

        _levelKillsCount.text = _infoViewer.CurrentKills.ToString();

        _levelScoreCount.text = GetCurrentScore().ToString();

        _totalScoreCount.text = GetTotalScore().ToString();
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