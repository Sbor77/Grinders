using TMPro;
using UnityEngine;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _score;    

    public void Render(string name, string score, string place)
    {
        _name.text = $"{place} {name}";
        _score.text = score;        
    }
}