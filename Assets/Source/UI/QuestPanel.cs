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
    public int TargetCoinCount { get; private set; }
    public int TargetEnemyKills { get; private set; }
    public int TargetBossKills { get; private set; }
    public bool NeedDestroyBigBox { get; private set; }

    public QuestInfo(int collectedCoins, int enemyKills, int bossKills, bool bigBox)
    {
        TargetCoinCount = collectedCoins;
        TargetEnemyKills = enemyKills;
        TargetBossKills = bossKills;
        NeedDestroyBigBox = bigBox;        
    }
}