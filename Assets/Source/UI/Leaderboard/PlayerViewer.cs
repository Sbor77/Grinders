using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _rankText;

    public void SetPlayerStats(int rank, int score)
    {
        _scoreText.text = score.ToString();
        _rankText.text = rank.ToString();
    }
}
