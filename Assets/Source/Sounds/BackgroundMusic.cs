using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _playList;    
    
    public static BackgroundMusic Instance { get; private set; }

    public bool IsPlaying { get; private set; }

    public int CurrentTrackIndex { get; private set; }

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
    }

    private void Start()
    {
        IsPlaying = true;

        int clipIndex = DataHandler.Instance.GetSavedLevel();        

        StartCoroutine(PlayMusicFrom(clipIndex));
    }

    private IEnumerator PlayMusicFrom (int clipIndex)
    {
        yield return null;

        while (IsPlaying)
        {
            bool isFirstCycle = true;

            for (int i = 0; i < _playList.Count; i++)
            {
                if (isFirstCycle)
                {
                    _audioSource.clip = _playList[clipIndex];                    

                    i = clipIndex;

                    isFirstCycle = false;
                }
                else
                {
                    _audioSource.clip = _playList[i];                    
                }
                
                _audioSource.Play();

                while(_audioSource.isPlaying)
                {
                    yield return null;
                }

                if (i == _playList.Count - 1)
                    i = -1;
            }
        }
    }
}