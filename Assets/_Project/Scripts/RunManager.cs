using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : GenericSingleton<RunManager>
{
    protected override bool ShouldBeDestroyedOnLoad => false;
    private HashSet<string> _triggeredZones = new HashSet<string>();
    public string ChosenPath { get; private set; }
    public string CurrentPathKnot { get; private set; } = "path_choice_1";
    public bool LastFightWon { get; private set; }
    public bool OrinIntroPlayed { get; private set; }
    public bool HasPulledGear { get; private set; }
    public int FightsWon { get; private set; }
    public bool BossUnlocked { get; private set; }

    public event Action OnPathChosen;
    public event Action OnBossUnlocked;

    public void SetGearPulled() => HasPulledGear = true;
    public void ClearGearPulled() => HasPulledGear = false;
    public void SetOrinIntroPlayed() => OrinIntroPlayed = true;
    public bool IsZoneTriggered(string zoneId) => _triggeredZones.Contains(zoneId);
    public void RegisterZoneTriggered(string zoneId) => _triggeredZones.Add(zoneId);
    public void SetPathKnot(string knot) => CurrentPathKnot = knot;

    public void SetFightWon()
    {
        LastFightWon = true;
        FightsWon++;
    }

    public void ClearFightWon() => LastFightWon = false;

    public void UnlockBoss()
    {
        BossUnlocked = true;
        OnBossUnlocked?.Invoke();
    }

    public void SetChosenPath(string path)
    {
        if (string.IsNullOrEmpty(ChosenPath))
            ChosenPath = path;
        else
            ChosenPath += "_" + path;
        OnPathChosen?.Invoke();
    }

    public string GetFinalPath()
    {
        if (string.IsNullOrEmpty(ChosenPath)) return "";
        string[] steps = ChosenPath.Split('_');
        return steps[steps.Length - 1];
    }

    public void ResetRun()
    {
        _triggeredZones.Clear();
        ChosenPath = "";
        CurrentPathKnot = "path_choice_1";
        LastFightWon = false;
        OrinIntroPlayed = false;
        HasPulledGear = false;
        FightsWon = 0;
        BossUnlocked = false;
    }
}
