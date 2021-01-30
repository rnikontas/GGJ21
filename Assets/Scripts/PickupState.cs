using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static TimedPowerupEffect;

public class PickupState : MonoBehaviour
{
    public int progression = 0;
    public int progressionThreshold = 0;
    public int cheatPoints = 0;
    public int cheatPointThreshold = 0;
    
    Dictionary<PowerUpName, TimedPowerupEffect> activePowerupEffects = new Dictionary<PowerUpName, TimedPowerupEffect>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PickupState initiated");
    }

    public void addTimedPowerUpEffect(PowerUpName name, TimedPowerupEffect timedPowerupEffect) {
        activePowerupEffects[name] = timedPowerupEffect;
    }

    public TimedPowerupEffect getTimedPowerUpEffect(PowerUpName name) {
        try
        {
            return activePowerupEffects[name];
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
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
