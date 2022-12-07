using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _playList;
    //[SerializeField] private GamesSdk _sdk;
    
    private int _shopSceneIndex = 5;    
    private int _currentSceneIndex;
    private Coroutine _playJob;
    public bool _isPlaying;

  /*  public static BackgroundMusic Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(this);
        }
    }*/

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        /*if (_sdk != null)
        {
            _sdk.AdVideoOpened += OnPlayAd;
            _sdk.AdVideoClosed += OnStopAd;
            _sdk.InterstitialAdOpened += OnPlayAd;
            _sdk.InterstitialAdClosed += OnStopAd;
        }*/
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;

       /* if (_sdk != null)
        {
            _sdk.AdVideoOpened -= OnPlayAd;
            _sdk.AdVideoClosed -= OnStopAd;
            _sdk.InterstitialAdOpened -= OnPlayAd;
            _sdk.InterstitialAdClosed -= OnStopAd;
        }*/
    }

    private void Start()
    {
        ActivatePlayCoroutine();
    }

    private void OnActiveSceneChanged(Scene replacedScene, Scene newScene)
    {
        if (_playJob != null && newScene.buildIndex != _shopSceneIndex)
        {
            StopCoroutine(_playJob);

            _playJob = null;

            ActivatePlayCoroutine();
        }
    }
/*
    private void OnPlayAd()
    {
        StopActionGamePlay(true);
    }

    private void OnStopAd()
    {
        StopActionGamePlay(false);
    }

    private void StopActionGamePlay(bool stopGame)
    {
        if (stopGame)
            _audioSource.Pause();
        else
            _audioSource.UnPause();

        AudioListener.pause = stopGame;
        AudioListener.volume = stopGame ? 0f : 1f;
        Time.timeScale = stopGame ? 0f : 1f;
    }*/

    private void ActivatePlayCoroutine()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        _isPlaying = true;

        _playJob = StartCoroutine(PlayMusicFromCurrentSceneIndex());
    }

    private IEnumerator PlayMusicFromCurrentSceneIndex()
    {
        yield return null;

        while (_isPlaying)
        {
            bool isFirstCycle = true;

            for (int i = 0; i < _playList.Count; i++)
            {
                if (isFirstCycle)
                {
                    _audioSource.clip = _playList[_currentSceneIndex];

                    i = _currentSceneIndex;

                    isFirstCycle = false;
                }
                else
                {
                    _audioSource.clip = _playList[i];
                }
                                
                _audioSource.Play();

                while (_audioSource.isPlaying)
                {
                    yield return null;
                }

                if (_currentSceneIndex != SceneManager.GetActiveScene().buildIndex || _currentSceneIndex != _shopSceneIndex)
                {
                    _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

                    isFirstCycle = true;
                }

                if (i == _playList.Count - 1)
                    i = -1;
            }
        }
    }    
}