using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private InfoViewer _infoViewer;
    [SerializeField] private Button _continueButton;
    [SerializeField] private TMP_Text _conditionsText;
    [Space]
    [SerializeField] private int _needCoinCollected;
    [SerializeField] private int _needEnemyKilled;
    [SerializeField] private bool _needDestroyBigBox;
    [Space]
    [SerializeField] private TMP_Text _coinsToCollect;
    [SerializeField] private TMP_Text _coinsCount;
    [SerializeField] private TMP_Text _enemiesToKill;
    [SerializeField] private TMP_Text _enemiesCount;
    [SerializeField] private TMP_Text _bigboxToCrush;
    [Space]
    [SerializeField] private TutorialPanel _tutorialPanel;

    public int NeedCoinCollected => _needCoinCollected;

    public int NeedEnemyKilled => _needEnemyKilled;


    private void OnEnable()
    {
        Init();

        _continueButton.onClick.AddListener(StartGame);

        if (_tutorialPanel != null)        
            _tutorialPanel.IsEnded += OnTutorialIsEnded;        
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(StartGame);

        if (_tutorialPanel != null)
            _tutorialPanel.IsEnded += OnTutorialIsEnded;
    }

    private void Start()
    {
        Time.timeScale = 0;

        if (_tutorialPanel != null)
            _tutorialPanel.gameObject.SetActive(true);
        else        
            ShowMissionTargets();
    }

    private void OnTutorialIsEnded()
    {
        ShowMissionTargets();
    }

    private void Init()
    {
        QuestInfo conditions = new QuestInfo(_needCoinCollected, _needEnemyKilled, _needDestroyBigBox);

        _infoViewer.SetQuestCollected(conditions);        
    }

    private void ShowMissionTargets()
    {
        if (_needCoinCollected > 0)
        {
            _coinsToCollect.gameObject.SetActive(true);

            _coinsCount.text = _needCoinCollected.ToString();
        }

        if (_needEnemyKilled > 0)
        {
            _enemiesToKill.gameObject.SetActive(true);

            _enemiesCount.text = _needEnemyKilled.ToString();
        }

        if (_needDestroyBigBox)
            _bigboxToCrush.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        Time.timeScale = 1;

        gameObject.SetActive(false);

        _cameraHandler.ActivateStartScenario();
    }
}

public class QuestInfo
{
    public int NeedCoinCollected { get; private set; }

    public int NeedEnemyKilled { get; private set; }

    public bool NeedDestroyBigBox { get; private set; }

    public QuestInfo(int coinCollected, int enemyKilled, bool bigBox)
    {
        NeedCoinCollected = coinCollected;

        NeedEnemyKilled = enemyKilled;

        NeedDestroyBigBox = bigBox;
    }
}