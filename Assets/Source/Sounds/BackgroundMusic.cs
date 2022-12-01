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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {

        }
    }

    private IEnumerator PlayMusicFrom (int clipIndex)
    {
        bool firstCycle = true;

        yield return null;

        while (IsPlaying)
        {
            for (int i = 0; i < _playList.Count; i++)
            {
                if (i == clipIndex && firstCycle)
                {
                    _audioSource.clip = _playList[clipIndex];

                    firstCycle = false;
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
