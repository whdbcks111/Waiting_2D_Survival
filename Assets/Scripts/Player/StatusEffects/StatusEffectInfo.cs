using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StatusEffectInfo
{
    public static readonly StatusEffectInfo Blood = new("과다출혈", eff =>
    {
        Player.Instance.HealthPoint -= Time.deltaTime * 0.5f * eff.Level;
    });
    public static readonly StatusEffectInfo Hungry = new("배고픔", eff =>
    {
        Player.Instance.MovePower *= Mathf.Pow(0.9f, eff.Level);
        Player.Instance.Blind = true;
    });
    public static readonly StatusEffectInfo StomachAche = new("배탈", eff =>
    {
        Player.Instance.Food -= Time.deltaTime * 2f * eff.Level;
        Player.Instance.Water -= Time.deltaTime * 3f * eff.Level;
    });

    private StatusEffectInfo(string name, Action<StatusEffect> action)
    {
        Name = name;
        Action = action;
    }

    public string Name;
    public Action<StatusEffect> Action;
}