using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private const float MaxPosition = 200f;
    private const float Delay = 0.5f;
    private const int HandRepeat = 3;
    private const int MouseRepeat = 2;

    [SerializeField] private JoystickTutorial _joystick;
    [SerializeField] private Image _mouseImage;
    [SerializeField] private Image _handImage;
    [SerializeField] private Sprite _mouseSprite;
    [SerializeField] private Sprite _mouseLeftButtonSprite;
    [SerializeField] private Sprite _mouseRightButtonSprite;
    [Space]
    [SerializeField] private Box _box;
    [SerializeField] private Transform _wallDoor;
    [SerializeField] private Transform _finishPoint;
    [SerializeField] private Enemy[] _enemies;
    [Space]
    [SerializeField] private TMP_Text _moveText;
    [SerializeField] private TMP_Text _attackText;
    [SerializeField] private TMP_Text _massAttackText;
    [SerializeField] private TMP_Text _moveComentText;
    [SerializeField] private TMP_Text _mobileMoveComentText;
    [SerializeField] private TMP_Text _attackComentText;
    [SerializeField] private TMP_Text _mobileAttackComentText;
    [SerializeField] private TMP_Text _coinCollectComentText;
    [SerializeField] private TMP_Text _nextAttackComentText;
    [SerializeField] private TMP_Text _finishPointComentText;
    [SerializeField] private TMP_Text _massAttackPcComentText;
    [SerializeField] private TMP_Text _massAttackComentText;
    [Space]
    [SerializeField] private AreaAttack _massAttack;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private GameObject _firstPathArrow;
    [SerializeField] private GameObject _secondPathArrow;
    [SerializeField] private GameObject _endScreen;

    private Vector3 _textScaleSize = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 _handScaleSize = new Vector3(0.8f, 0.8f, 1f);
    private int _killedEnemies = 0;
    private int _levelOneSceneIndex = 1;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(LoadLevelOne);
        _playButton.onClick.AddListener(LoadLevelOne);
        _massAttack.IsActivated += OnMassAttackActivated;
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(LoadLevelOne);
        _playButton.onClick.RemoveListener(LoadLevelOne);
        _massAttack.IsActivated -= OnMassAttackActivated;

        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemies[i].IsDied -= OnEnemiesDead;
        }
    }

    private void Start()
    {
        if (!DataHandler.Instance.IsMobile())
        {
            _mouseImage.gameObject.SetActive(true);
            _handImage.gameObject.SetActive(false);
            StartPCMoveAnimation();
        }
        else
        {
            _mouseImage.gameObject.SetActive(false);
            _handImage.gameObject.SetActive(true);
            _handImage.enabled = false;
            StartMobileMoveAnimation();
        }
    }

    public void OnStayMovePoint()
    {
        _firstPathArrow.SetActive(false);
        _joystick.enabled = false;
        _joystick.AttackButtonClick += OnAttack;

        if (DataHandler.Instance.IsMobile())
            StartMobileAttackAnimation();
        else
            StartPCAttackAnimation();
    }

    public void OnStayFinishPoint()
    {
        _joystick.DisableMoving();
        _joystick.EnableMassAttacking();

        if (DataHandler.Instance.IsMobile())
            StartMobileMassAttackAnimation();
        else
            ChangeComentText(_massAttackPcComentText);
    }

    public void OnButtonMassAttackActivate()
    {
        _joystick.DisableAttacking();
    }

    private void LoadLevelOne()
    {
        SceneManager.LoadScene(_levelOneSceneIndex);
    }

    private void OnMassAttackActivated()
    {
        _massAttackComentText.gameObject.SetActive(false);
        _massAttackPcComentText.gameObject.SetActive(false);
        _massAttackText.gameObject.SetActive(false);

        float endScreenDelay = HandRepeat / Delay;
        DOVirtual.DelayedCall(endScreenDelay, () => _endScreen.SetActive(true));
    }

    private void OnAttack()
    {
        _attackText.gameObject.SetActive(false);
        ChangeComentText(_coinCollectComentText);
        _box.IsItemCollected += OnItemCollected;
        _joystick.AttackButtonClick -= OnAttack;
    }

    private void OnItemCollected()
    {
        float doorHieght = 1f;

        _wallDoor.DOMoveY(0f, doorHieght).SetEase(Ease.Linear).SetLoops(1);
        _attackText.gameObject.SetActive(true);
        ChangeComentText(_nextAttackComentText);
        _secondPathArrow.SetActive(true);
        _box.IsItemCollected -= OnItemCollected;

        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemies[i].IsDied += OnEnemiesDead;
        }
    }

    private void OnEnemiesDead()
    {
        _killedEnemies++;

        if (_killedEnemies >= _enemies.Length)
        {
            _secondPathArrow.SetActive(false);
            ChangeTitleText(_massAttackText);
            DOVirtual.DelayedCall(Delay, () => _finishPoint.gameObject.SetActive(true));

            if (DataHandler.Instance.IsMobile() == false)
                ChangeComentText(_finishPointComentText);
        }
    }

    private void StartMobileMoveAnimation()
    {
        Sequence moveAnimation = DOTween.Sequence();       

        moveAnimation.AppendCallback(() =>
        {
            _mouseImage.sprite = _mouseSprite;
            ChangeComentText(_mobileMoveComentText);
            _firstPathArrow.SetActive(true);
        });
        moveAnimation.AppendInterval(Delay);
        moveAnimation.AppendCallback(() =>
        {
            _joystick.EnableMoving();
            _joystick.enabled = true;
        });
    }

    private void StartPCMoveAnimation()
    {
        Sequence moveAnimation = DOTween.Sequence();

        moveAnimation.AppendCallback(() => ShowAnimationMouseClick(_mouseLeftButtonSprite, MouseRepeat));
        moveAnimation.AppendInterval(MouseRepeat);
        moveAnimation.Append(_mouseImage.transform.DOLocalMoveY(MaxPosition, Delay).SetEase(Ease.Linear).SetLoops(MouseRepeat, LoopType.Yoyo));
        moveAnimation.Append(_mouseImage.transform.DOLocalMoveY(-MaxPosition, Delay).SetEase(Ease.Linear).SetLoops(MouseRepeat, LoopType.Yoyo));
        moveAnimation.Append(_mouseImage.transform.DOLocalMoveX(MaxPosition, Delay).SetEase(Ease.Linear).SetLoops(MouseRepeat, LoopType.Yoyo));
        moveAnimation.Append(_mouseImage.transform.DOLocalMoveX(-MaxPosition, Delay).SetEase(Ease.Linear).SetLoops(MouseRepeat, LoopType.Yoyo));
        moveAnimation.AppendInterval(Delay);
        moveAnimation.AppendCallback(() =>
        {
            _mouseImage.sprite = _mouseSprite;
            ChangeComentText(_moveComentText);
            _firstPathArrow.SetActive(true);
        });
        moveAnimation.AppendInterval(1f);
        moveAnimation.AppendCallback(() =>
        {
            _joystick.EnableMoving();
            _mouseImage.enabled = false;
        });
    }

    private void StartPCAttackAnimation()
    {
        _mouseImage.enabled = true;
        Sequence attackAnimation = DOTween.Sequence();

        attackAnimation.AppendCallback(() => 
        {
            ChangeTitleText(_attackText); 
            ChangeComentText(_attackComentText); 
        });
        attackAnimation.AppendCallback(() => ShowAnimationMouseClick(_mouseRightButtonSprite, HandRepeat));
        attackAnimation.AppendInterval(HandRepeat);
        attackAnimation.AppendCallback(() => _mouseImage.sprite = _mouseSprite);
        attackAnimation.AppendInterval(Delay);
        attackAnimation.AppendCallback(() =>
        {
            _joystick.enabled = true;
            _joystick.EnableAttacking();
            _mouseImage.enabled = false;
        });
    }

    private void StartMobileAttackAnimation()
    {
        _handImage.transform.position = _joystick.AttackButtonPosition;
        _handImage.enabled = true;
        Sequence attackAnimation = DOTween.Sequence();

        attackAnimation.AppendCallback(() => 
        { 
            ChangeTitleText(_attackText); 
            ChangeComentText(_mobileAttackComentText); 
        });
        attackAnimation.AppendCallback(() => ShowAnimationHandClick(_handImage, HandRepeat));
        attackAnimation.AppendInterval(HandRepeat / Delay);
        attackAnimation.AppendCallback(() =>
        {
            _joystick.EnableAttacking();
            _handImage.enabled = false;
            _joystick.enabled = true;
        });
    }

    private void StartMobileMassAttackAnimation()
    {
        _handImage.transform.position = _joystick.MassAttackButtonPosition;
        _handImage.enabled = true;
        Sequence attackAnimation = DOTween.Sequence();

        attackAnimation.AppendCallback(() => 
        { 
            ChangeTitleText(_massAttackText);
            ChangeComentText(_massAttackComentText);
        });
        attackAnimation.AppendCallback(() => ShowAnimationHandClick(_handImage, HandRepeat));
        attackAnimation.AppendInterval(HandRepeat / Delay);
        attackAnimation.AppendCallback(() =>
        {
            _joystick.EnableAttacking();
            _handImage.enabled = false;
            _joystick.enabled = true;
        });        
    }

    private void ShowAnimationMouseClick(Sprite clickSprite, int repeatCount)
    {
        Sequence clickAnimation = DOTween.Sequence();

        for (int i = 0; i < repeatCount; i++)
        {
            clickAnimation.AppendCallback(() => _mouseImage.sprite = _mouseSprite);
            clickAnimation.AppendInterval(Delay);
            clickAnimation.AppendCallback(() => _mouseImage.sprite = clickSprite);
            clickAnimation.AppendInterval(Delay);
        }
    }

    private void ShowAnimationHandClick(Image _handImage, int repeatCount)
    {
        int repeateAction = (int)(repeatCount / Delay);
        _handImage.transform.DOScale(_handScaleSize, Delay).SetEase(Ease.Linear).SetLoops(repeateAction, LoopType.Yoyo);
    }

    private void ChangeTitleText(TMP_Text titleText)
    {
        _moveText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);
        _attackText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);
        _massAttackText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);

        DOVirtual.DelayedCall(Delay, () => ShowTitleText(titleText));
    }

    private void ShowTitleText(TMP_Text titleText)
    { 
        _moveText.gameObject.SetActive(false);
        _attackText.gameObject.SetActive(false);
        _massAttackText.gameObject.SetActive(false);

        titleText.gameObject.SetActive(true);
        titleText.transform.DOScale(Vector3.one, Delay).SetEase(Ease.Linear).SetLoops(1);
    }

    private void ChangeComentText(TMP_Text text)
    {
        _moveComentText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);
        _attackComentText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);
        _mobileAttackComentText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);
        _coinCollectComentText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);
        _nextAttackComentText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);
        _finishPointComentText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);
        _massAttackPcComentText.transform.DOScale(_textScaleSize, Delay).SetEase(Ease.Linear).SetLoops(1);

        DOVirtual.DelayedCall(Delay, () => ShowText(text));
    }

    private void ShowText(TMP_Text text)
    {
        _moveComentText.gameObject.SetActive(false);
        _mobileMoveComentText.gameObject.SetActive(false);
        _mobileAttackComentText.gameObject.SetActive(false);
        _attackComentText.gameObject.SetActive(false);
        _coinCollectComentText.gameObject.SetActive(false);
        _nextAttackComentText.gameObject.SetActive(false);
        _finishPointComentText.gameObject.SetActive(false);
        _massAttackPcComentText.gameObject.SetActive(false);
        _massAttackComentText.gameObject.SetActive(false);

        if (text != null)
        {
            text.gameObject.SetActive(true);
            text.transform.DOScale(Vector3.one, Delay).SetEase(Ease.Linear).SetLoops(1);
        }
    }
}