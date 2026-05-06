using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemDropper : InteractableObject
{
    protected override void OnInteract()
    {
        if (RunManager.Instance.HasPulledGear)
        {
            PopupMessage.Instance.Show("You already grabbed your gear");
            return;
        }

        if (RandomDropManager.Instance.UnlockedCount == 0)
        {
            PopupMessage.Instance.Show("You won't get anything if you don't buy it first");
            return;
        }

        RunManager.Instance.SetGearPulled();
        RandomDropManager.Instance.GetRandomDrop();
        PopupMessage.Instance.Show("Gear pulled,head into battle");
    }
}
