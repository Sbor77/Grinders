using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private InfoViewer _infoViewer;
    [SerializeField] private Button _continueButton;    
    [SerializeField] private TMP_Text _zonesCount;    
    [SerializeField] private GameObject _infoPanel;    

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(StartGame);  
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(StartGame);        
    }

    public void SetZonesCount(int value)
    {
        _zonesCount.text = value.ToString();
        Time.timeScale = 0;
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