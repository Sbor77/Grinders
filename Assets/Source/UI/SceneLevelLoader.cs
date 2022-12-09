using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLevelLoader : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;

    private AsyncOperation _asyncLoad;

    public void Load(int levelIndex)
    {
        gameObject.SetActive(true);

        StartCoroutine(LoadScene(levelIndex));
    }

    private IEnumerator LoadScene(int index)
    {
        _asyncLoad = SceneManager.LoadSceneAsync(index);

        while (!_asyncLoad.isDone)
        {
            _progressSlider.value = _asyncLoad.progress;
            yield return null;
        }
    }
}