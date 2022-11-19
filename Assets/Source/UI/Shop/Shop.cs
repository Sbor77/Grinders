using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private SkillBuyer _buyer;
    [SerializeField] private StatsViewer _viewer;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _nextButton;

    private const string Level = "SceneLevel";

    private void Start()
    {
        OnStatBought();
    }

    private void OnEnable()
    {
        _buyer.IsStatBought += OnStatBought;
        _exitButton.onClick.AddListener(CloseShop);
        _nextButton.onClick.AddListener(NextLevel);
    }

    private void OnDisable()
    {
        _buyer.IsStatBought -= OnStatBought;
        _exitButton.onClick.RemoveListener(CloseShop);
        _nextButton.onClick.RemoveListener(NextLevel);
    }

    private void CloseShop()
    {

    }

    private void NextLevel()
    {
        SceneManager.LoadScene(Level + (DataHandler.Instance.GetSavedLevel() + 1));
    }

    private void OnStatBought()
    {
        DataHandler.Instance.SaveAllStats();
        _viewer.Init();
        _buyer.Init();
    }
}