using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Image _handImage;
    [SerializeField] private TMP_Text _moveText;
    [SerializeField] private TMP_Text _attackText;

    private Vector2 _startSize = Vector2.one;
    private Vector2 _clickSize = new Vector2(.7f, .7f);
    
    private void Start()
    {
        if (!DataHandler.Instance.IsMobile())
        {
            Sequence animation = DOTween.Sequence();

            animation.AppendInterval(0.5f);
            animation.Append(_handImage.transform.DOScale(_clickSize, .3f).SetEase(Ease.Linear));
            animation.Append(_handImage.transform.DOLocalMoveY(100f, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
            animation.Append(_handImage.transform.DOLocalMoveY(-100f, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
            animation.Append(_handImage.transform.DOLocalMoveX(100f, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
            animation.Append(_handImage.transform.DOLocalMoveX(-100f, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
            animation.AppendCallback(() => { ChangeText(); });
            animation.AppendInterval(0.5f);
            animation.Append(_handImage.transform.DOScale(_startSize, .3f).SetEase(Ease.Linear));
            animation.AppendInterval(.5f);
            animation.AppendCallback(() => { HideTutorial(); });
        }
    }

    private void ChangeText()
    {
        _moveText.gameObject.SetActive(false);
        _attackText.gameObject.SetActive(true);
    }

    private void HideTutorial()
    {
        gameObject.SetActive(false);
    }
}