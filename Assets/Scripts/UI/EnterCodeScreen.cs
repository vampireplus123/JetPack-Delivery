using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterCodeScreen : UIScreen
{
    [SerializeField] TextMeshProUGUI m_error;
    [SerializeField] TMP_InputField m_codeInput;
    [SerializeField] TMP_InputField m_nameInput;
    public void OnJoinButtonPressed()
    {
        if (m_codeInput.text.Trim() == "")
        {
            m_codeInput.transform.Find("WarningText").gameObject.SetActive(true);
            return;
        }
        if (m_nameInput.text.Trim() == "")
        {
            m_nameInput.transform.Find("WarningText").gameObject.SetActive(true);
            return;
        }

        m_codeInput.transform.Find("WarningText").gameObject.SetActive(false);
        m_nameInput.transform.Find("WarningText").gameObject.SetActive(false);

        MenuManager.Instance.ShowLoading();
        m_error.gameObject.SetActive(false);

        XanhTournamentSDK.DataManager.Tournament.JoinTournament(m_codeInput.text.Trim(), m_nameInput.text.Trim(), (result) =>
        {
            MenuManager.Instance.HideLoading();
            if (result)
            {
                MenuManager.Instance.ShowTournament();
            }
            else
            {
                m_error.gameObject.SetActive(true);
                m_error.text = "Failed to join Tournament!\nPlease try again";
            }
        });
    }
}
