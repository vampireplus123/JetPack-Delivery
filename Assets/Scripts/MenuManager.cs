using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static MenuManager m_instance;
    public static MenuManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MenuManager>();
            }

            return m_instance;
        }
    }

    [SerializeField] MenuScreen m_menuScreen;
    [SerializeField] PauseScreen m_pauseScreen;
    [SerializeField] IngameScreen m_ingameScreen;
    [SerializeField] SettingScreen m_settingScreen;
    [SerializeField] EndGameScreen m_endGameScreen;
    [SerializeField] LoadingScreen m_loadingScreen;
    [SerializeField] StandingScreen m_standingScreen;
    [SerializeField] EnterCodeScreen m_enterCodeScreen;
    [SerializeField] TournamentScreen m_tournamentScreen;
    [SerializeField] RegisterMember m_registrerScreen;
    [Space]
    [SerializeField] Image m_menuBackground;
    [SerializeField] Image m_ingameBackground;

    private void Awake()
    {
    }

    #region Menu UI    
    private void HideAllScreen()
    {
        m_menuScreen.HideScreen();
        m_pauseScreen.HideScreen();
        m_ingameScreen.HideScreen();
        m_settingScreen.HideScreen();
        m_endGameScreen.HideScreen();
        m_loadingScreen.HideScreen();
        m_standingScreen.HideScreen();
        m_enterCodeScreen.HideScreen();
        m_tournamentScreen.HideScreen();
        m_registrerScreen.HideScreen();
    }

    public void StartGame()
    {
        HideAllScreen();
        m_ingameScreen.ShowScreen();
        m_ingameScreen.SetScoreText(0);
        GameManager.Instance.StartGame();

        m_menuBackground.enabled = false;
        m_ingameBackground.enabled = true;
    }
    public void OpenSetting()
    {
        HideAllScreen();
        m_settingScreen.ShowScreen();
    }
    public void PauseGame()
    {
        HideAllScreen();
        m_pauseScreen.ShowScreen();
        GameManager.Instance.PauseGame();

        m_menuBackground.enabled = true;
        m_ingameBackground.enabled = false;
    }
    public void ResumeGame()
    {
        HideAllScreen();
        m_ingameScreen.ShowScreen();
        GameManager.Instance.ResumeGame();

        m_menuBackground.enabled = false;
        m_ingameBackground.enabled = true;
    }
    public void ShowLoading()
    {
        m_loadingScreen.ShowScreen();
    }
    public void HideLoading()
    {
        m_loadingScreen.HideScreen();
    }
    public void EndGame()
    {
        HideAllScreen();
        m_endGameScreen.ShowScreen();

        m_menuBackground.enabled = true;
        m_ingameBackground.enabled = false;
    }
    public void BackToHome()
    {
        HideAllScreen();
        m_menuScreen.ShowScreen();
    }
    #endregion

    public void EnterCode()
    {
        HideAllScreen();
        m_enterCodeScreen.ShowScreen();
    }
    public void ShowTournament()
    {
        HideAllScreen();
        m_tournamentScreen.ShowScreen();
    }
    public void ShowStandings()
    {
        HideAllScreen();
        m_standingScreen.ShowScreen();
    }
    public void ShowRegister()
    {
        HideAllScreen();
        m_registrerScreen.ShowScreen();
    }

    #region Ingame 
    public void SetScore(int score, int highScore)
    {
        m_ingameScreen.SetScoreText(score);
        m_menuScreen.SetHighScore(highScore);
        m_endGameScreen.SetScoreText(score, highScore);
    }
    public void SetCharacterVisualID(int visualID)
    {
        GameManager.Instance.SetCharacterVisualID(visualID);
    }
    #endregion
}
