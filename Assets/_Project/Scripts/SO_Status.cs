using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Status")]

public class SO_Status : SO_Effect
{
    [SerializeField] private StatusType _statusType;
    [SerializeField] private float _duration; //quanti turni dura lo stato
    [SerializeField] private float _value;  //% danno/riduzione difesa

    public StatusType StatusType => _statusType;
    public float Duration => _duration;
    public float Value => _value;
    public override void Apply(GameObject user)
    {
       
    }
}
