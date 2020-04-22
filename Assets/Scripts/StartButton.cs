using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Control control;
    public Text buttonText;

    private bool active = false;

    public void OnClick()
    {
        if (active)
        {
            buttonText.text = "Start";
            control.StopSim();
            active = false;
        } else
        {
            buttonText.text = "Stop";
            control.StartSim();
            active = true;
        }
    }
}
