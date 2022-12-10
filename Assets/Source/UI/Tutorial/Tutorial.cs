using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private const float MaxPosition = 200f;

    [SerializeField] private JoystickTutorial _joystick;
    [SerializeField] private Image _mouseImage;
    [SerializeField] private Sprite _mouseSprite;
    [SerializeField] private Sprite _mouseLeftButtonSprite;
    [SerializeField] private Sprite _mouseRightButtonSprite;
    [SerializeField] private Box _box;
    [SerializeField] private Transform _wallDoor;
    [SerializeField] private Transform _finishPoint;
    [SerializeField] private Enemy[] _enemys;
    [SerializeField] private TMP_Text _moveText;
    [SerializeField] private TMP_Text _attackText;
    [SerializeField] private TMP_Text _massAttackText;
    [SerializeField] private TMP_Text _moveComentText;
    [SerializeField] private TMP_Text _attackComentText;
    [SerializeField] private TMP_Text _coinCollectComentText;
    [SerializeField] private TMP_Text _nextAttackComentText;
    [SerializeField] private TMP_Text _finishPointComentText;
    [SerializeField] private TMP_Text _massAttackComentText;

    private int _killedEnemys = 0;

    private void Start()
    {
        if (!DataHandler.Instance.IsMobile())
        {
            StartPCMoveAnimation();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _enemys.Length; i++)
        {
            _enemys[i].Dying -= OnEnemysDead;
        }
    }

    public void OnStayMovePoint()
    {
        //ChangeText();
        _joystick.gameObject.SetActive(false);
        StartPCAttackAnimation();
        _joystick.ChangedClickStatus += OnAttack;
    }

    public void OnStayFinishPoint()
    {
        _joystick.NotAllowedToMove();
        _joystick.AllowToMassAttack();
        ShowComentText(_massAttackComentText);
    }

    public void OnButtonMassAttackActivate()
    {
        _joystick.NotAllowedToAttack();
    }

    private void OnAttack()
    {
        ShowComentText(_coinCollectComentText);
        _box.IsItemCollected += OnItemCollected;
        _joystick.ChangedClickStatus -= OnAttack;
    }

    private void OnItemCollected()
    {
        _wallDoor.DOMoveY(0f, 1f).SetEase(Ease.Linear).SetLoops(1);
        ShowComentText(_nextAttackComentText);
        _box.IsItemCollected -= OnItemCollected;

        for (int i = 0; i < _enemys.Length; i++)
        {
            _enemys[i].Dying += OnEnemysDead;
        }
    }

    private void OnEnemysDead()
    {
        _killedEnemys++;

        if (_killedEnemys >= _enemys.Length)
        {
            ShowComentText(_finishPointComentText);
            ChangeTitleText(_massAttackText);
            _finishPoint.gameObject.SetActive(true);
        }
    }

    private void StartPCMoveAnimation()
    {
        Sequence moveAnimation = DOTween.Sequence();
        moveAnimation.AppendCallback(() => { ShowAnimationMouseClick(_mouseLeftButtonSprite, .5f, 2); });
        moveAnimation.AppendInterval(2f);
        moveAnimation.Append(_mouseImage.transform.DOLocalMoveY(MaxPosition, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
        moveAnimation.Append(_mouseImage.transform.DOLocalMoveY(-MaxPosition, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
        moveAnimation.Append(_mouseImage.transform.DOLocalMoveX(MaxPosition, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
        moveAnimation.Append(_mouseImage.transform.DOLocalMoveX(-MaxPosition, .5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo));
        moveAnimation.AppendInterval(0.5f);
        moveAnimation.AppendCallback(() =>
        {
            _mouseImage.sprite = _mouseSprite;
            _moveComentText.gameObject.SetActive(true);
        });
        moveAnimation.AppendInterval(1f);
        moveAnimation.AppendCallback(() =>
        {
            _joystick.AllowToMove();
            _mouseImage.enabled = false;
        });
    }

    private void StartPCAttackAnimation()
    {
        _mouseImage.enabled = true;
        Sequence attackAnimation = DOTween.Sequence();
        attackAnimation.AppendCallback(() => { ChangeTitleText(_attackText); ShowComentText(_attackComentText); });
        attackAnimation.AppendCallback(() => { ShowAnimationMouseClick(_mouseRightButtonSprite, .5f, 3); });
        attackAnimation.AppendInterval(3f);
        attackAnimation.AppendCallback(() => { _mouseImage.sprite = _mouseSprite; });
        attackAnimation.AppendInterval(.5f);
        attackAnimation.AppendCallback(() =>
        {
            _joystick.gameObject.SetActive(true);
            _joystick.AllowToAttack();
            _mouseImage.enabled = false;
        });
    }

    private void ShowAnimationMouseClick(Sprite clickSprite, float delay, int repeatCount)
    {
        Sequence clickAnimation = DOTween.Sequence();

        for (int i = 0; i < repeatCount; i++)
        {
            clickAnimation.AppendCallback(() => { _mouseImage.sprite = _mouseSprite; });
            clickAnimation.AppendInterval(delay);
            clickAnimation.AppendCallback(() => { _mouseImage.sprite = clickSprite; });
            clickAnimation.AppendInterval(delay);
        }
    }

    private void ChangeTitleText(TMP_Text titleText)
    {
        _moveText.gameObject.SetActive(false);
        _attackText.gameObject.SetActive(false);
        _massAttackText.gameObject.SetActive(false);

        titleText.gameObject.SetActive(true);
    }

    private void ShowComentText(TMP_Text text)
    {
        _moveComentText.gameObject.SetActive(false);
        _attackComentText.gameObject.SetActive(false);
        _coinCollectComentText.gameObject.SetActive(false);
        _nextAttackComentText.gameObject.SetActive(false);
        _finishPointComentText.gameObject.SetActive(false);
        _massAttackComentText.gameObject.SetActive(false);
    
        if (text != null)
            text.gameObject.SetActive(true);
    }

    private void HideTutorial()
    {
        gameObject.SetActive(false);
    }
}