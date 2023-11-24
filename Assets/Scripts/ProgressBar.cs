using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : Singleton<ProgressBar>
{
    public Transform bar;
    public Text number;

    public void SetProgressBar(float current, float max)
    {
        float progress = current / max;

        if (progress < 0) progress = 0;
        if (progress > 1.0f) progress = 1.0f;
        bar.localScale = new Vector3(progress, 1, 1);

        number.text = current.ToString();
    }
}
