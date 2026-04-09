using UnityEngine;

public abstract class SO_GenericItem : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _id;

    public string Name => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
    public int Id => _id;

    public abstract void Use(GameObject user);
}
