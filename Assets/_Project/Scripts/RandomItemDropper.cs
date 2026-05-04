using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemDropper : InteractableObject
{
    protected override void OnInteract()
    {
        if (RunManager.Instance.HasPulledGear) return;
        RunManager.Instance.SetGearPulled();
        RandomDropManager.Instance.GetRandomDrop();
    }
}
