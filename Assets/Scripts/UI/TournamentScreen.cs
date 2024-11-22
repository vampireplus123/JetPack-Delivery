using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TournamentScreen : UIScreen
{
    [SerializeField] TextMeshProUGUI m_welcomeText;

    public override void ShowScreen()
    {
        base.ShowScreen();
        
        m_welcomeText.gameObject.SetActive(false);

        if (XanhTournamentSDK.DataManager.Tournament.TournamentInfo == null)
        {
            MenuManager.Instance.ShowLoading();
            XanhTournamentSDK.DataManager.Tournament.GetTournamentData((result) =>
            {
                MenuManager.Instance.HideLoading();
                if (result)
                {
                    var tournamentName = XanhTournamentSDK.DataManager.Tournament.TournamentInfo.Name;
                    m_welcomeText.text = $"WELCOME TO TOURNAMNET\n{tournamentName}";
                    m_welcomeText.gameObject.SetActive(true);
                }
                else
                {
                    // Todo: Show popup try again
                }
            });
        }
        else
        {
            var tournamentName = XanhTournamentSDK.DataManager.Tournament.TournamentInfo.Name;
            m_welcomeText.text = $"WELCOME TO TOURNAMNET\n{tournamentName}";
            m_welcomeText.gameObject.SetActive(true);
        }
    }

    public void OnPlayButtonPressed()
    {
        MenuManager.Instance.StartGame();
    }
    public void OnStandingButtonPressed()
    {
        MenuManager.Instance.ShowStandings();
    }
    public void OnBackButtonPressed()
    {
        
        MenuManager.Instance.BackToHome();
    }
}
