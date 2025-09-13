using UnityEngine;
using UnityEngine.UI;

public abstract class Turret : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected string turretName;
    [SerializeField] GameObject model;
    [SerializeField] Image thumbnail;

    [Header("Stats")]
    [SerializeField] protected int cost;
    [SerializeField] protected float damage;
    [SerializeField] protected float health;
    [SerializeField] protected float attackSpeed;

    public string TurretName
    {
        get => turretName;
        set => turretName = value;
    }

    public GameObject Model
    {
        get => model;
        set => model = value;
    }

    public Image Thumbnail
    {
        get => thumbnail;
        set => thumbnail = value;
    }

    public int Cost
    {
        get => cost;
        set => cost = value;
    }

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    public float Health
    {
        get => health;
        set => health = value;
    }

    public float AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
    }
}
