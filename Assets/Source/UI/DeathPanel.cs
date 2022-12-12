using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private Button _dieButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private ArenaIcon _arenaIcon;
    [SerializeField] private ShopIcon _shopIcon;

    private bool _rewarded;
    private int _dieSceneIndex;


    private void OnEnable()
    {
        _dieButton.onClick.AddListener(OnDieClick);
        _continueButton.onClick.AddListener(OnContinueClick);
        
        GamesSdk.Instance.Rewarded += OnRewarded;
        GamesSdk.Instance.AdVideoClosed += OnContinueWithReward;        
    }

    private void OnDisable()
    {
        _dieButton.onClick.RemoveListener(OnDieClick);
        _continueButton.onClick.RemoveListener(OnContinueClick);
        
        GamesSdk.Instance.Rewarded -= OnRewarded;
        GamesSdk.Instance.AdVideoClosed -= OnContinueWithReward;        
    }

    public void Activate(int dieSceneIndex = 0)
    {
        _arenaIcon?.Deactivate();
        _shopIcon?.Deactivate();

        _dieSceneIndex = dieSceneIndex;
        gameObject.SetActive(true);
        _rewarded = false;
    }

    private void OnDieClick()
    {
        int totalScore = DataHandler.Instance.GetSavedTotalScore();

        DataHandler.Instance.DeleteAllStatsWithExcludes();
        DataHandler.Instance.SaveTotalScore(totalScore);
        SceneManager.LoadScene(_dieSceneIndex);
    }

    private void OnContinueClick()
    {
        GamesSdk.Instance.VideoAdShow();
    }

    private void OnRewarded()
    {
        _rewarded = true;
    }

    private void OnContinueWithReward()
    {
        if (_rewarded)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}