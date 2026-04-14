using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DropItem
{
    [SerializeField] private SO_Ability _abilityItem;
    [SerializeField] private SO_Weapon _weaponItem;
    [SerializeField] private float _dropChance;

    public SO_Weapon WeaponItem => _weaponItem;
    public SO_Ability AbilityItem => _abilityItem;
    public float DropChance => _dropChance;
}

