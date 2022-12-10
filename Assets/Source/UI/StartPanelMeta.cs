using UnityEngine;

public class StartPanelMeta : MonoBehaviour 
{ 
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}