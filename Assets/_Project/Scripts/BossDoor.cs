using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private void Start()
    {
        if (RunManager.Instance == null) return;
        RunManager.Instance.OnBossUnlocked += Open;
        if (RunManager.Instance.BossUnlocked) Open();
    }

    private void Open() => gameObject.SetActive(false);

    private void OnDestroy()
    {
        if (RunManager.Instance != null)
            RunManager.Instance.OnBossUnlocked -= Open;
    }
}
