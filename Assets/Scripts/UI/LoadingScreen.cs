using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : UIScreen
{
    [SerializeField] float m_speed;
    [SerializeField] RectTransform m_icon;

    private void Update()
    {
        m_icon.eulerAngles += Vector3.forward * m_speed * Time.deltaTime;
    }
}
