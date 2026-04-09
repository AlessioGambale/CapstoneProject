using UnityEngine;

[CreateAssetMenu(menuName = "Stats/PlayerStats")]
public class SO_Stats : ScriptableObject
{
    [SerializeField] private int _maxHP;
    [SerializeField] private int _attack;
    [SerializeField] private float _defencePercent; 
    [SerializeField] private int _luck;   
    [SerializeField] private int _maxAP; 
    [SerializeField] private int _maxEP;
    [SerializeField] private int _speed;  

    public int MaxHP => _maxHP;
    public int Attack => _attack;
    public float DefencePercent => _defencePercent;
    public int Luck => _luck;   
    public float MaxAP => _maxAP;
    public int MaxEP => _maxEP;
    public int Speed => _speed;
}
