using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _cointText;
    [SerializeField] private Image _coinArrow;
    [SerializeField] private TMP_Text _killText;
    [SerializeField] private Image _killArrow;
    [SerializeField] private TMP_Text _bigboxText;
    [SerializeField] private Image _bigboxArrow;
    [SerializeField] private Image _bigboxOnsceneArrow;

    private void Start()
    {
     
    }

    private void AnimateArrow()
    {
        Vector3 startPosition = _coinArrow.transform.position;
        float targetPositionX = _coinArrow.transform.position.x - 100;
        float targetPositionY = _coinArrow.transform.position.y + 100;
        float time = 1f;
        int loops = 20;

        Vector3 targetPosition = new Vector3(targetPositionX, targetPositionY, _coinArrow.transform.position.z);

        _coinArrow.transform.DOMoveY(targetPositionY, time).SetLoops(loops, LoopType.Yoyo);
    }

    private void AnimateBigboxArrow()
    {
        Vector3 startPosition = _bigboxOnsceneArrow.transform.position;        
        float targetPositionY = _bigboxOnsceneArrow.transform.position.y - 150;
        float time = 0.5f;
        int loops = 30;

        _bigboxOnsceneArrow.transform.DOMoveY(targetPositionY, time).SetLoops(loops, LoopType.Yoyo);


        /*var fadeSequence = DOTween.Sequence();

        for (int i = 0; i < loops; i++)
        {
            fadeSequence.Append(_bigboxOnsceneArrow.DOFade(0, time/2)).SetEase(Ease.OutSine);
            fadeSequence.Append(_bigboxOnsceneArrow.DOFade(1, time)).SetEase(Ease.InSine);
            fadeSequence.AppendInterval(time);
        }*/





    }

    


}
