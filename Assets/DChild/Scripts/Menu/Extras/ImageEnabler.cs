using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageEnabler : MonoBehaviour
{
    private Image m_image;

    public void Show() => ChangeAlphaTo(1);

    public void Hide() => ChangeAlphaTo(0);

    private void ChangeAlphaTo(float value)
    {
        var color = m_image.color;
        color.a = value;
        m_image.color = color;
    }

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }
}
