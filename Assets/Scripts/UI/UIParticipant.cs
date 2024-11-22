using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIParticipant : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_rank;
    [SerializeField] TextMeshProUGUI m_name;
    [SerializeField] TextMeshProUGUI m_score;

    public void Initialize(int rank, string name, int score)
    {
        m_rank.text = $"{rank}.";
        m_name.text = name;
        m_score.text = score.ToString();
    }
}
