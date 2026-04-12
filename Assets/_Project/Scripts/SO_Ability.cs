using UnityEngine;

[CreateAssetMenu(menuName = "Item/Ability")]

public class SO_Ability : SO_GenericItem
{
    [SerializeField] private AbilityType _abilityType;
    [SerializeField] private float _value;
    [SerializeField] private int _apCost;
    [SerializeField] private float _duration;


    public AbilityType AbilityType => _abilityType;
    public float Value => _value;
    public int ApCost => _apCost;
    public float Duration => _duration;

    public override void Use(GameObject user)
    {

    }
}
