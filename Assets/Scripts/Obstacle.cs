using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float m_slideSpeed;
    [SerializeField] float m_slideDuration;

    [SerializeField] Vector2 m_leftMovement;
    [SerializeField] Vector2 m_rightMovement;
    
    [SerializeField] Transform m_left;
    [SerializeField] Transform m_right;

    private bool m_isClosing;
    private float m_timeCount = 0;

    public float SlideSpeed
    {
        get => m_slideSpeed;
        set => m_slideSpeed = value;
    }
    public float SlideDuration
    {
        get => m_slideDuration;
        set => m_slideDuration = value;
    }

    private void Start()
    {
        var startRatio = Random.value;
        m_timeCount = m_slideDuration * startRatio;
        m_isClosing = (startRatio < 0.5f) ? true : false;
    }
    private void Update()
    {
        if (m_isClosing)
        {
            if (m_timeCount < m_slideDuration)
            {
                m_left.localPosition = Vector2.Lerp(new Vector2(m_leftMovement.x, 0), new Vector2(m_leftMovement.y, 0), m_timeCount / m_slideDuration);
                m_right.localPosition = Vector2.Lerp(new Vector2(m_rightMovement.x, 0), new Vector2(m_rightMovement.y, 0), m_timeCount / m_slideDuration);
                m_timeCount += Time.deltaTime * m_slideSpeed;
            }
            else
            {
                m_timeCount = 0;
                m_isClosing = false;
            }
        }
        else
        {
            if (m_timeCount < m_slideDuration)
            {
                m_left.localPosition = Vector2.Lerp(new Vector2(m_leftMovement.y, 0), new Vector2(m_leftMovement.x, 0), m_timeCount / m_slideDuration);
                m_right.localPosition = Vector2.Lerp(new Vector2(m_rightMovement.y, 0), new Vector2(m_rightMovement.x, 0), m_timeCount / m_slideDuration);
                m_timeCount += Time.deltaTime * m_slideSpeed;
            }
            else
            {
                m_timeCount = 0;
                m_isClosing = true;
            }
        }
    }
}
