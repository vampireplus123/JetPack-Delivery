using UnityEngine;
using UnityEngine.UI;

public class ImageColorLerping : MonoBehaviour
{
    [SerializeField] Image m_image;
    [SerializeField] Gradient m_colorGradient;
    [SerializeField] float m_duration = 1f;

    private float m_timeCount = 0f;
    private bool m_isFirst = true;

    private void Update()
    {
        // Calculate timeCount
        m_timeCount += Time.deltaTime * (m_isFirst ? 1 : -1);
        
        // Detect cycle
        m_isFirst = (m_timeCount > m_duration || m_timeCount < 0) ? !m_isFirst : m_isFirst;

        // Apply color
        m_image.color = m_colorGradient.Evaluate(m_timeCount / m_duration);
    }
}
