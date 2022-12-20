using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneLoader : SceneLevelLoader
{
    [SerializeField] private Button _button;
    [SerializeField] private int _loadSceneIndex;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadSceneWithSavingProgress);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadSceneWithSavingProgress);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void LoadSceneWithSavingProgress()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        DataHandler.Instance.SaveLevel(level);
        Load(_loadSceneIndex);
    }
}