using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryTimeSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var textComponent = this.gameObject.GetComponent<Text>();

        var timerObjects = GameObject.FindGameObjectsWithTag("Timer");
        if (timerObjects.Length > 0) {
            if (timerObjects.Length == 1) {
                var timer = timerObjects[0].GetComponent<Timer>();
                var minutes = Math.Floor(timer.time / 60);
                var seconds = timer.time - minutes * 60;
                textComponent.text = $"Your time: {minutes}:{seconds.ToString("00.00")}";
            }
        }

    }
}
