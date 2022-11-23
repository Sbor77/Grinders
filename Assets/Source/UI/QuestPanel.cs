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
    
    private void OnEnable()
    {
        Time.timeScale = 0;

        _continueButton.onClick.AddListener(StartGame);

        Init();
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(StartGame);
    }

    private void Init()
    {
        string ConditionsText = "";

        if (_needCoinCollected > 0)
            ConditionsText += $"Collect {_needCoinCollected} Coins\n";

        if (_needEnemyKilled > 0)
            ConditionsText += $"Kill {_needEnemyKilled} enemies\n";

        if (_needDestroyBigBox)
            ConditionsText += "Destroy big box";

        _conditionsText.text = ConditionsText;

        QuestInfo conditions = new QuestInfo(_needCoinCollected, _needEnemyKilled, _needDestroyBigBox);

        _infoViewer.SetQuestCollected(conditions);        
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