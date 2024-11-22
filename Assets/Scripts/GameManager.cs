using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Initializing,
    Playing,
    Pausing,
    End
}

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

    [Header("Game Properties")]
    [SerializeField] GameState m_state;
    [SerializeField] Vector2 m_vertical;
    [SerializeField] Vector2 m_horizontal;
    
    [Header("Game Preferences")]
    [SerializeField] Transform m_mapHolder;
    [SerializeField] Character m_playerPrefab;
    [SerializeField] GameObject m_packagePrefab;
    [SerializeField] GameObject m_obstaclePrefab;

    [Header("Game Data")]
    [SerializeField] int m_score;
    [SerializeField] int m_bestScore;
    [SerializeField] int m_spawnAmount;
    [SerializeField] float m_currentHeight;
    [SerializeField] float m_spawnHeightDiff;

    private int m_visualID = 0;
    private Character m_player;

    public GameState State => m_state;
    public Vector2 Vertical => m_vertical;
    public Vector2 Horizontal => m_horizontal;

    private void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;

        CalculateScreenSize();
    }
    private void Update()
    {
        if (m_state != GameState.Playing) return;

        if (m_player.transform.position.y > m_currentHeight / 2)
        {
            SpawnMap();
        }
    }

    private void ResetGameData()
    {
        m_score = 0;
        m_currentHeight = 0;
        Camera.main.transform.position = new Vector3(0, 0, -10);

        for (int i = 0; i < m_mapHolder.childCount; i++)
        {
            Destroy(m_mapHolder.GetChild(i).gameObject);
        }
    }
    private void CalculateScreenSize()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
        var upperRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        m_vertical = new Vector2(bottomLeft.y, upperRight.y);
        m_horizontal = new Vector2(bottomLeft.x, upperRight.x);
    }
    private void SpawnMap()
    {
        for (int i = 0; i < m_spawnAmount; i++)
        {
            var obstacle = Instantiate(m_obstaclePrefab,
                                       new Vector3(0f, m_currentHeight, 0f),
                                       Quaternion.identity,
                                       m_mapHolder);
            var package = Instantiate(m_packagePrefab,
                                      new Vector3(0f, m_currentHeight + m_spawnHeightDiff / 2, 0f),
                                      Quaternion.identity,
                                      m_mapHolder);
            m_currentHeight += m_spawnHeightDiff;
        }
    }
    private void UpdatePlayerInfo()
    {
        var updatedPlayerInfo = new XanhTournamentSDK.Participant()
        {
            ID = XanhTournamentSDK.DataManager.Tournament.PlayerInfo.ID,
            Name = XanhTournamentSDK.DataManager.Tournament.PlayerInfo.Name,
            Score = m_bestScore
        };
        MenuManager.Instance.ShowLoading();
        XanhTournamentSDK.DataManager.Tournament.UpdatePlayerInfo(updatedPlayerInfo, (result) =>
        {
            MenuManager.Instance.HideLoading();
            if (result)
            {
                ResetGameData();
                MenuManager.Instance.EndGame();
            }
            else
            {
                // Todo: Show popup try again
            }
        });
    }

    public void SetCharacterVisualID(int characterVisualID)
    {
        m_visualID = characterVisualID;
    }
    public void ScorePoint()
    {
        m_score++;
        m_bestScore = (m_bestScore < m_score) ? m_score : m_bestScore;

        SoundManager.Instance.PlaySound("Score");
        MenuManager.Instance.SetScore(m_score, m_bestScore);
    }

    public void StartGame()
    {
        Debug.LogWarning("Start Game");

        ResetGameData();
        m_state = GameState.Playing;
        m_mapHolder.gameObject.SetActive(true);

        m_player = Instantiate(m_playerPrefab, m_mapHolder);
        m_player.InitializeCharacter(m_visualID);
        SpawnMap();
    }
    public void PauseGame()
    {
        Debug.LogWarning("Pause Game");

        m_state = GameState.Pausing;
        m_mapHolder.gameObject.SetActive(false);
    }
    public void ResumeGame()
    {
        Debug.LogWarning("Resume Game");

        m_state = GameState.Playing;
        m_mapHolder.gameObject.SetActive(true);
    }
    public void EndGame()
    {
        Debug.LogWarning("End Game");

        m_state = GameState.End;
        m_mapHolder.gameObject.SetActive(false);

        if (XanhTournamentSDK.DataManager.Tournament.TournamentInfo != null)
        {
            if (XanhTournamentSDK.DataManager.Tournament.PlayerInfo == null)
            {
                MenuManager.Instance.ShowLoading();
                XanhTournamentSDK.DataManager.Tournament.GetPlayerData((result) =>
                {
                    MenuManager.Instance.HideLoading();
                    if (result)
                    {
                        UpdatePlayerInfo();
                    }
                    else
                    {
                        // Todo: Show popup try again
                    }
                });
            }
            else
            {
                UpdatePlayerInfo();
            }
        }
        else
        {
            ResetGameData();
            MenuManager.Instance.EndGame();
        }
    }
}
