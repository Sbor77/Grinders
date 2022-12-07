using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLevelLoader : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;

    private AsyncOperation _asyncLoad;

    public void LoadLevel(int levelIndex)
    {
        gameObject.SetActive(true);

        StartCoroutine(Loader(levelIndex));
    }

    private IEnumerator Loader(int index)
    {
        _asyncLoad = SceneManager.LoadSceneAsync(index);

        while (!_asyncLoad.isDone)
        {
            _progressSlider.value = _asyncLoad.progress;
            yield return null;
        }
    }
}