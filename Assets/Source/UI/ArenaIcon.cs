using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaIcon : MonoBehaviour
{
    [SerializeField] private Button _button;

    private int _metaSceneIndex = 7;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadArenaScene);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadArenaScene);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void LoadArenaScene()
    {
        SceneManager.LoadScene(_metaSceneIndex);
    }
}