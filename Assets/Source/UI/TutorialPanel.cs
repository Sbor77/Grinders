using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private Image _missionPad;
    [SerializeField] private Image _attackPad;
    [Space]
    [SerializeField] private TMP_Text _targetGuides;
    [SerializeField] private TMP_Text _bigboxGuides;
    [Space]
    [SerializeField] private GameObject _playerGuides;
    [SerializeField] private TMP_Text _walkGuides;
    [SerializeField] private TMP_Text _walkGuidesMobile;
    [SerializeField] private TMP_Text _attackGuides;
    [SerializeField] private TMP_Text _attackGuidesMobile;
    [SerializeField] private TMP_Text _attackGuidesCommon;
    [Space]
    [SerializeField] private Button _nextButton;

    private float _hintDuration = 0.5f;
    private int _hintLoops = -1;
    private int _clickCounts;
    private float _textFadeDuration = 1;
    private float _textPause = 1.5f;

    public event Action IsEnded;

    private void OnEnable()
    {
        _nextButton.onClick.AddListener(OnClickButton);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(OnClickButton);
    }

    private void Start()
    {
        _nextButton.interactable = false;

        DOVirtual.DelayedCall(0.5f, () =>
        {
            _playerGuides.SetActive(true);
            Fade(_targetGuides, 1, 1);
            FadeOut(_missionPad);

            Fade(_bigboxGuides, 1, _textFadeDuration);

            DOVirtual.DelayedCall(_textPause, () => _nextButton.interactable = true);
        });
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    private void OnClickButton()
    {
        _targetGuides.gameObject.SetActive(false);

        _bigboxGuides.gameObject.SetActive(false);
        

        if (_clickCounts >= 1)
        {
            IsEnded?.Invoke();

            Deactivate();
        }

        if (_clickCounts == 0)
        {
            _targetGuides.gameObject.SetActive(false);

            if (DataHandler.Instance.IsMobile())
            {
                Fade(_walkGuidesMobile, 1, _textFadeDuration);

                DOVirtual.DelayedCall(_textPause, () => Fade(_attackGuidesMobile, 1, _textFadeDuration));
            }
            else
            {
                Fade(_walkGuides, 1, _textFadeDuration);

                DOVirtual.DelayedCall(_textPause, () =>
                {
                    Fade(_attackGuides, 1, _textFadeDuration);
                    FadeOut(_attackPad);
                });
            }

            DOVirtual.DelayedCall(_textPause + _textPause, () => Fade(_attackGuidesCommon, 1, _textFadeDuration));

            _clickCounts++;
        }
    }

    private void FadeOut(Image image)
    {
        image.gameObject.SetActive(true);

        image.DOFade(0, _hintDuration).SetLoops(_hintLoops, LoopType.Yoyo).OnComplete(() =>
        {
            image.gameObject.SetActive(false);
        }).SetUpdate(true);
    }

    private void Fade(TMP_Text text, float alpha, float duration)
    {
        text.gameObject.SetActive(true);

        text.DOFade(alpha, _hintDuration).SetUpdate(true).SetEase(Ease.InSine).SetUpdate(true);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}