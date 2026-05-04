using UnityEngine;

public enum SoundID
{
    NONE = 0,

    FOOTSTEPS_GRASS = 1,
    FOOTSTEPS_ROCK = 2,
    FOOTSTEPS_WOOD = 3,

    LANDING_GRASS = 10,
    LANDING_ROCK = 11,
    LANDING_WOOD = 12,
    LANDING_WATER = 13,    
}

[System.Serializable]
public class SoundData
{
    public SoundID ID;
    public AudioClip[] Clips;
}
