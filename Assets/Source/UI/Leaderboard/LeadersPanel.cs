using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;
using TMPro;

public class LeadersPanel : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _autorizationButton;
    [SerializeField] private Button _closeAutorizationButton;
    [SerializeField] private GameObject _autorizationPanel;
    [SerializeField] private GameObject _content;
    [SerializeField] private PlayerEntry _template;
    [SerializeField] private TextMeshProUGUI _playerScore;
    [SerializeField] private TextMeshProUGUI _playerPlace;

    private int _maxViews = 5;
    private string _anonymous = "Anonymous";
    private string _zero = "0";

    private void OnEnable()
    {
        if (PlayerAccount.IsAuthorized)
        {
            ShowLeaders();
            _autorizationPanel.SetActive(false);
        }

        _okButton.onClick.AddListener(HideLeaderboard);
        _autorizationButton.onClick.AddListener(OnAutorizationClick);
        _closeAutorizationButton.onClick.AddListener(HideLeaderboard);
    }

    private void OnDisable()
    {
        _okButton.onClick.RemoveListener(HideLeaderboard);
        _autorizationButton.onClick.RemoveListener(OnAutorizationClick);
        _closeAutorizationButton.onClick.RemoveListener(HideLeaderboard);

        foreach (Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowLeaders()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        if (PlayerAccount.IsAuthorized)
        {
            Leaderboard.GetEntries(GamesSdk.Instance.LeaderboardName, (result) =>
            {
                foreach (var entry in result.entries)
                {
                    string name = entry.player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = _anonymous;

                    int score = entry.score;
                    int place = entry.rank;

                    if (place > _maxViews)
                        break;

                    AddLeader(name, score, place);
                }
            });

            Leaderboard.GetPlayerEntry(GamesSdk.Instance.LeaderboardName, (result) =>
            {
                if (result == null)
                {                    
                    _playerPlace.text = _zero;
                    _playerScore.text = _zero;
                }
                else
                {
                    _playerPlace.text = result.rank.ToString();
                    _playerScore.text = result.score.ToString();
                }
            });
        }
    }

    private void OnAutorizationClick()
    {
        PlayerAccount.Authorize();
    }

    private void HideLeaderboard()
    {
        gameObject.SetActive(false);
    }

    private void AddLeader(string name, int score, int place)
    {
        var view = Instantiate(_template, _content.transform);
        view.Show(name, score.ToString(), place.ToString());
    }
}