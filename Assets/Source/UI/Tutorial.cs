using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private JoystickTutorial _joystick;
    [SerializeField] private Image _mouseImage;
    [SerializeField] private Sprite _mouseSprite;
    [SerializeField] private Sprite _mouseLeftButtonSprite;
    [SerializeField] private Sprite _mouseRightButtonSprite;
    [SerializeField] private TMP_Text _moveText;
    [SerializeField] private TMP_Text _attackText;

    private Vector2 _startSize = Vector2.one;
    private Vector2 _clickSize = new Vector2(.7f, .7f);
    
    private void Start()
    {
        if (!DataHandler.Instance.IsMobile())
        {
            StartPCMoveAnimation();
        }
    }

    private void StartPCMoveAnimation()
    {
        Sequence animation = DOTween.Sequence();
        animation.AppendCallback(() => { _mouseImage.sprite = _mouseSprite; });
        animation.AppendInterval(0.5f);
        animation.AppendCallback(() => { _mouseImage.sprite = _mouseLeftButtonSprite; });
        animation.AppendInterval(0.5f);
        animation.AppendCallback(() => { _mouseImage.sprite = _mouseSprite; });
        animation.AppendInterval(0.5f);
        animation.AppendCallback(() => { _mouseImage.sprite = _mouseLeftButtonSprite; });
        animation.Append(_mouseImage.transform.DOLocalMoveY(200f, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
        animation.Append(_mouseImage.transform.DOLocalMoveY(-200f, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
        animation.Append(_mouseImage.transform.DOLocalMoveX(200f, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
        animation.Append(_mouseImage.transform.DOLocalMoveX(-200f, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
        animation.AppendInterval(0.5f);
        animation.AppendCallback(() => { _mouseImage.sprite = _mouseSprite; });
        animation.AppendInterval(1f);
        animation.AppendCallback(() =>
        {
            _joystick.AllowToMove();
            _mouseImage.enabled = false;
        });
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