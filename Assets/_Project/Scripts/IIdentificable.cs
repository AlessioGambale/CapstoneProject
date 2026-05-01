using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectID
{
    SO_BIGPOTION = 0,
    SO_BIGSWORD = 1,
    SO_TELEPORT = 2,
    SO_SCREWDRIVER = 3,

    NONE = 100,
}

public interface IIdentificable
{
    public ObjectID ID { get; }
}
