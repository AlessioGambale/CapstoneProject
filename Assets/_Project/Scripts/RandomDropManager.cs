using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDropManager : GenericSingleton<RandomDropManager>
{
    protected override bool ShouldBeDestroyedOnLoad => false;

    [SerializeField] private List<SO_Weapon> _weapons;
    [SerializeField] private List<SO_Ability> _abilities;

    private List<SO_Weapon> _unlockedWeapons = new List<SO_Weapon>();
    private List<SO_Ability> _unlockedAbilities = new List<SO_Ability>();

    public void UnlockWeapon(SO_Weapon weapon)
    {
        if (!_unlockedWeapons.Contains(weapon))
            _unlockedWeapons.Add(weapon);
    }

    public void UnlockAbility(SO_Ability ability)
    {
        if (!_unlockedAbilities.Contains(ability))
            _unlockedAbilities.Add(ability);
    }

    public void GetRandomDrop()
    {
        GetRandomAbilty();
        GetRandomWeapon();
    }

    public void GetRandomAbilty()
    {
        if (_unlockedAbilities.Count == 0) return;
        int random = Random.Range(0, _unlockedAbilities.Count);
        InventoryManager.Instance.AddItem(_unlockedAbilities[random]);
    }

    public void GetRandomWeapon()
    {
        if (_unlockedWeapons.Count == 0) return;
        int random = Random.Range(0, _unlockedWeapons.Count);
        InventoryManager.Instance.AddItem(_unlockedWeapons[random]);
    }
}
