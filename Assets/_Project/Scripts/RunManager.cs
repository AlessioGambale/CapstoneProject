using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : GenericSingleton<RunManager>
{
    protected override bool ShouldBeDestroyedOnLoad => false;

    private HashSet<string> _triggeredZones = new HashSet<string>();
    public bool LastFightWon { get; private set; }
    public void SetFightWon() => LastFightWon = true;
    public void ClearFightWon() => LastFightWon = false;

    public bool IsZoneTriggered(string zoneId) => _triggeredZones.Contains(zoneId);

    public void RegisterZoneTriggered(string zoneId) => _triggeredZones.Add(zoneId);

    public void ResetRun()
    {
        _triggeredZones.Clear();
    }
}
