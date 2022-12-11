using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopIcon : MonoBehaviour
{
    [SerializeField] private Button _button;

    private int _shopSceneIndex = 5;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadShopScene);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadShopScene);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void SavePlaceToComeback()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        DataHandler.Instance.SaveLevel(level);
    }

    private void LoadShopScene()
    {
        SavePlaceToComeback();
        SceneManager.LoadScene(_shopSceneIndex);
    }
}