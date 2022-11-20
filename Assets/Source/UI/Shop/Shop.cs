using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private SkillBuyer _skillBuyer;
    [SerializeField] private StatsViewer _statsViewer;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _nextButton;

    private void Start()
    {
        OnStatBought();
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
        //exit to start screen
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(DataHandler.Instance.GetSavedLevel() + 1);
    }

    private void OnStatBought()
    {
        DataHandler.Instance.SaveAllStats();

        _statsViewer.Init();

        _skillBuyer.Init();
    }
}