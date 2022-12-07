//using System.Collections;
//using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _place;

    public void Render(string name, string score, string place)
    {
        _name.text = name;
        _score.text = score;
        _place.text = place;
    }
}
