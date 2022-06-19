using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    private Image HealthBarImage;

    public void SetHealthBarValue(float value)
    {
        HealthBarImage.fillAmount = value;
        if (HealthBarImage.fillAmount < 0.2f)
            SetHealthBarColor(Color.red);
        else if (HealthBarImage.fillAmount < 0.4f)
            SetHealthBarColor(Color.yellow);
        else
            SetHealthBarColor(Color.green);
    }

    public float GetHealthBarValue()
    {
        return HealthBarImage.fillAmount;
    }

    public void SetHealthBarColor(Color healthColor)
    {
        HealthBarImage.color = healthColor;
    }

    private void Awake()
    {
        HealthBarImage = GetComponent<Image>();
    }
}