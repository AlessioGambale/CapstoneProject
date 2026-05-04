using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectID
{
    SO_NEEDLE = 0,
    SO_CANDYSTICK = 1,
    SO_TOYCRUSHER = 2,
    SO_SILLYKNIFE = 3,
    SO_IRONSHEARS = 4,
    SO_CRIT = 5,
    SO_DESPERATION = 6,
    SO_HEAL = 7,
    SO_SHIELD = 8,
    SO_WOUND = 9,
    SO_HEALPOTION = 10,
    SO_SCREWDRIVER = 11 ,

    NONE = 100,
}

public interface IIdentificable
{
    public ObjectID ID { get; }
}
