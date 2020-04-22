using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Start/stop game control
public class StartButton : MonoBehaviour
{
    public Control control;
    public Text buttonText;

    private bool active = false;

    // Method called when button is pressed
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
