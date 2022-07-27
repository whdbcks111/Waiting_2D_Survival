using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StatusEffect
{

    public StatusEffect(StatusEffectInfo info, int level, float duration)
    {
        this.Level = level;
        this.Duration = duration;
        this.Info = info;
    }

    public StatusEffectInfo Info;
    public float Duration;
    public int Level;
}