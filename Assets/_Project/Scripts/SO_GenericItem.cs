using UnityEngine;

public abstract class SO_GenericItem : ScriptableObject, IIdentificable
{
    [Header("Info")]
    [SerializeField] private ObjectID _id;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    [SerializeField][Min(0)] private int _buyPrice;
    [SerializeField][Min(0)] private int _sellPrice;

    [Header("Options")]
    [SerializeField] private bool _isStackable = true;
    [SerializeField] private bool _isConsumable = true;

    public ObjectID ID { get => _id; }
    public Sprite Icon => _icon;
    public string Name => _name;
    public int BuyPrice => _buyPrice;
    public int SellPrice => _sellPrice;
    public string Description => _description;
    public bool IsStackable => _isStackable;
    public bool IsConsumable => _isConsumable;


    public abstract void Use(GameObject user);
}
