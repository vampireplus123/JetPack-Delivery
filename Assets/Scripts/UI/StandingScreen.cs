using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StandingScreen : UIScreen
{
    [SerializeField] int m_leaderCount;
    [SerializeField] Transform m_holder;
    [SerializeField] GameObject m_title;
    [SerializeField] TextMeshProUGUI m_error;
    [SerializeField] UIParticipant m_participantPrefab;

    private void ResetStandings()
    {
        for (int i = 0; i < m_holder.childCount; i++)
        {
            Destroy(m_holder.GetChild(i).gameObject);
        }
    }

    [ContextMenu("Show")]
    public override void ShowScreen()
    {
        base.ShowScreen();

        ResetStandings();
        m_title.SetActive(false);
        m_error.gameObject.SetActive(false);

        // Show loading screen
        MenuManager.Instance.ShowLoading();

        // Get participants data
        XanhTournamentSDK.DataManager.Tournament.GetTournamentStanding((result) =>
        {
            MenuManager.Instance.HideLoading();
            if (result)
            {
                m_title.SetActive(true);
                var standing = XanhTournamentSDK.DataManager.Tournament.Standings;
                var leaderboardAmount = Mathf.Min(m_leaderCount, standing.Count);
                for (int i = 0; i < leaderboardAmount; i++)
                {
                    var uiParticipant = Instantiate(m_participantPrefab, m_holder);
                    uiParticipant.Initialize(i + 1, standing[i].Name, standing[i].Score);
                }
            }
            else
            {
                m_error.gameObject.SetActive(true);
                m_error.text = "Load Standings failed. Please try again!";
            }
        });
    }
    public void OnBackButtonPressed()
    {
        // Get back to UITournament
        MenuManager.Instance.ShowTournament();
    }
}
