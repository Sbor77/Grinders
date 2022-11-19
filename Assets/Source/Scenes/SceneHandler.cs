using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{


    private void Start()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public void LoadScene (int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        print($"Перешли из сцены c индексом: {current.buildIndex} в сцену с индексом: {next.buildIndex}");
    }


}
