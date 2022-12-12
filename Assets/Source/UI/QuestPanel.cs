using System.Collections.Generic;
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
    [SerializeField] private bool _needDestroyBigBox;
    [Space]
    [SerializeField] private TMP_Text _coinsToCollect;
    [SerializeField] private TMP_Text _coinsCount;
    [SerializeField] private TMP_Text _enemiesToKill;
    [SerializeField] private TMP_Text _enemiesCount;
    [SerializeField] private TMP_Text _bigboxToCrush;
    [Space]    
    [SerializeField] private GameObject _infoPanel;

    private int _needCoinCollected;
    private int _needEnemyKilled;

    public int NeedCoinCollected => _needCoinCollected;

    public int NeedEnemyKilled => _needEnemyKilled;


    private void OnEnable()
    {
        _continueButton.onClick.AddListener(StartGame);  
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(StartGame);        
    }

    private void Start()
    {
        Init();

        Time.timeScale = 0;

        
        //ShowMissionTargets();
        
    }

    private void Init()
    {
        

        QuestInfo conditions = new QuestInfo(_needCoinCollected, _needEnemyKilled, _needDestroyBigBox);

        

        //_infoViewer.SetQuestCollected(conditions);
    }

    private void ShowMissionTargets()
    {
        ActivateInfoPanel();

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

    private void ActivateInfoPanel()
    {
        _infoPanel.gameObject.SetActive(true);
    }

    private void DeactivateInfoPanel()
    {
        _infoPanel.gameObject.SetActive(false);
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