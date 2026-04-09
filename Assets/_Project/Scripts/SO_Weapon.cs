using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapons")]

public class SO_Weapon : SO_GenericItem
{
    [SerializeField] private int _baseDamage;
    [SerializeField] private StatusType _statusType;
    [SerializeField] private int _statusBuildUp;
    [SerializeField] private SpecialAttackType _specialType;
    [SerializeField] private float _specialDamageMultiplier;
    [SerializeField] private bool _ignoresDefence;
    [SerializeField] private bool _isAOE;

    public int BaseDamage => _baseDamage;
    public StatusType StatusType => _statusType;
    public int StatusBuildUp => _statusBuildUp;
    public SpecialAttackType SpecialType => _specialType;
    public float SpecialDamageMultiplier => _specialDamageMultiplier;
    public bool IgnoreDefence => _ignoresDefence;
    public bool IsAOE => _isAOE;

    public override void Use(GameObject user) { }
}