using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{
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

    private float _hintDuration = 0.5f;
    private int _hintLoops = 8;

    private void Start()
    {
        Invoke (nameof(ShowSideHints), 0.5f);

        //AnimateFade(_coinPad, 0, _hintDuration, _hintLoops);
    }

    private void ShowSideHints()
    {        
        float interval = _hintDuration * _hintLoops + 1;        

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
        hintSequence.AppendCallback(() => Time.timeScale = 1);




        /*AnimateMissionHint(_coinArrow, _coinText);

        DOVirtual.DelayedCall(interval, () => AnimateMissionHint(_killArrow, _killText)).SetUpdate(true);

        DOVirtual.DelayedCall(interval * 2 , () =>
            {
                AnimateMissionHint(_bigboxArrow, _bigboxText);
                AnimateFade(_bigboxMark, 0, _hintTime, _hintLoops);
            }).SetUpdate(true); 

        DOVirtual.DelayedCall(interval * 3, () => Time.timeScale = 1).SetUpdate(true);    */
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
}