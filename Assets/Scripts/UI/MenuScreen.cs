using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : UIScreen
{
    [SerializeField] Text m_bestScore;

    public void OnStartGameButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");

        MenuManager.Instance.StartGame();
    }
    public void OnSettingButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");

        MenuManager.Instance.OpenSetting();
    }
    public void OnTournamentButtonPressed()
    {
        SoundManager.Instance.PlaySound("Click");

        if (XanhTournamentSDK.DataManager.Tournament.IsJoinedTournament)
        {
            MenuManager.Instance.ShowTournament();
        }
        else
        {
            MenuManager.Instance.EnterCode();
        }
    }

    public void SetHighScore(int highScore)
    {
        m_bestScore.text = "BEST " + highScore;
    }
}
