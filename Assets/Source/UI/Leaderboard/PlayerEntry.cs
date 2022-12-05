using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _positionText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void SetPlayer(Leader player)
    {
        _positionText.text = player.Ranks.ToString();
        _nameText.text = player.Names;
        _scoreText.text = player.Scores.ToString();
    }
}
