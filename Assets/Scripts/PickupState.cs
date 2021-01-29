using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PickupState : MonoBehaviour
{
    public int progression = 0;
    public int progressionThreshold = 0;
    public int cheatPoints = 0;
    public int cheatPointThreshold = 0;
    
    public Dictionary<string, TimedPowerupEffect> activePowerupEffects = new Dictionary<string, TimedPowerupEffect>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PickupState loaded");
        activePowerupEffects.Add("POWERUP1", new TimedPowerupEffect(8, 1));
        activePowerupEffects.Add("POWERUP2", new TimedPowerupEffect(5, 2));
        activePowerupEffects.Add("POWERUP3", new TimedPowerupEffect(2, 3));
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        updatePowerUpState(Time.deltaTime);
        foreach (var powerup in activePowerupEffects)
        {
            Debug.Log(powerup.Key + ":" + powerup.Value.printState());
        }
    }

    private void updatePowerUpState(float timePassed) {
        foreach (var powertype in activePowerupEffects.Keys)
        {
            var powerTimeLeft = activePowerupEffects[powertype].timeLeft - timePassed;
            if (powerTimeLeft <= 0)
            {
                activePowerupEffects.Remove(powertype);
            } else {
                activePowerupEffects[powertype].timeLeft = powerTimeLeft;
            }
        }
    }
}
