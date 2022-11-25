using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private QuestPanel _questPanel;
    [Space]
    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private Image _coinArrow;
    [SerializeField] private Image _coinPad;
    [Space]
    [SerializeField] private TMP_Text _killText;
    [SerializeField] private Image _killArrow;
    [SerializeField] private Image _killPad;
    [Space]
    [SerializeField] private TMP_Text _bigboxText;
    [SerializeField] private Image _bigboxArrow;
    [SerializeField] private Image _bigboxPad;
    [SerializeField] private Image _bigboxMark;
    [Space]
    [SerializeField] private GameObject _playerGuides;
    [SerializeField] private Button _okButton;

    private float _hintDuration = 0.5f;
    private int _hintLoops = 8;

    private void Start()
    {
        Invoke (nameof(ShowSideHints), 0.5f);

        //AnimateFade(_coinPad, 0, _hintDuration, _hintLoops);
    }

    private void OnEnable()
    {
        _okButton.onClick.AddListener(OnClickButton);
    }

    private void OnDisable()
    {
        _okButton.onClick.RemoveListener(OnClickButton);
    }

    private void OnClickButton()
    {
        Time.timeScale = 1;

        _playerGuides.gameObject.SetActive(false);
    }

    private void ShowSideHints()
    {        
        float interval = _hintDuration * _hintLoops + 1;

        SetText();

        Time.timeScale = 0;

        Sequence hintSequence = DOTween.Sequence();

        hintSequence.SetUpdate(true);

        hintSequence.AppendCallback(() => 
        { 
            AnimateMissionHint(_coinArrow, _coinText);
            FadeOut(_coinPad, 0);
        });
        hintSequence.AppendInterval(interval);
        hintSequence.AppendCallback(() =>
        {
            AnimateMissionHint(_killArrow, _killText);
            FadeOut(_killPad, 0);
        });
        hintSequence.AppendInterval(interval);
        hintSequence.AppendCallback(() =>
        {
            AnimateMissionHint(_bigboxArrow, _bigboxText);
            FadeOut(_bigboxPad, 0);
            FadeOut(_bigboxMark, 0);
        });
        hintSequence.AppendInterval(interval);
        hintSequence.AppendCallback(() => _playerGuides.gameObject.SetActive(true));

        //hintSequence.AppendCallback(() => Time.timeScale = 1);
    }

    private void AnimateMissionHint(Image arrow, TMP_Text text)
    {
        Vector3 startPosition = arrow.transform.position;
        float offsetX = 150;
        float offsetY = 120;

        arrow.gameObject.SetActive(true);

        text.gameObject.SetActive(true);

        Vector3 targetPosition = new Vector3(startPosition.x + offsetX, startPosition.y + offsetY, 0);

        arrow.transform.DOMove(targetPosition, _hintDuration).SetLoops(_hintLoops, LoopType.Yoyo).OnComplete(() =>
        {
            arrow.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            arrow.transform.position = startPosition;
        }).SetUpdate(true);
    }

    private void FadeOut(Image image, float endAlpha)
    {
        image.gameObject.SetActive(true);

        image.DOFade(endAlpha, _hintDuration).SetLoops(_hintLoops, LoopType.Yoyo).OnComplete(() =>
        {
            image.gameObject.SetActive(false);
        }).SetUpdate(true);
    }

    private void SetText()
    {
        _coinText.text = $"Crush small boxes and collect {_questPanel.NeedCoinCollected.ToString()} coins";

        _killText.text = $"Kill {_questPanel.NeedEnemyKilled.ToString()} enemies";
    }
}