using System.Collections.Generic;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    [System.Serializable]
    public class WeaponVisual
    {
        public SO_Weapon _weaponSO;
        public GameObject _weaponObject;
    }

    [SerializeField] private List<WeaponVisual> _weapons;

    private void Start()
    {
        CombatManager.Instance.OnCombatStarted += ShowWeapon;
        CombatManager.Instance.OnCombatVictory += HideWeapon;
        CombatManager.Instance.OnCombatDefeat += HideWeapon;
        HideAll();
    }

    private void ShowWeapon()
    {
        HideAll();
        SO_Weapon current = InventoryManager.Instance.CurrentWeapon;
        if (current == null) return;

        var visual = _weapons.Find(w => w._weaponSO == current);
        if (visual != null)
            visual._weaponObject.SetActive(true);
    }

    private void HideWeapon()
    {
        HideAll();
    }

    private void HideAll()
    {
        foreach (var w in _weapons)
            if (w._weaponObject != null)
                w._weaponObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (CombatManager.Instance == null) return;
        CombatManager.Instance.OnCombatStarted -= ShowWeapon;
        CombatManager.Instance.OnCombatVictory -= HideWeapon;
        CombatManager.Instance.OnCombatDefeat -= HideWeapon;
    }
}
