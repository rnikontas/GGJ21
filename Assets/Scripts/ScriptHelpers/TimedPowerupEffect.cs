using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TimedPowerupEffect
{
  public float timeLeft = 10;
  public int strength = 1;

  public TimedPowerupEffect()
  {
  }

  public TimedPowerupEffect(float timeLength, int strength)
  {
    this.timeLeft = timeLength;
    this.strength = strength;
  }

  public string printState()
  {
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("time: " + timeLeft);
    sb.AppendLine("strength: " + strength);
    return sb.ToString();
  }
}
