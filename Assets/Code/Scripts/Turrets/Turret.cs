using UnityEngine;
using UnityEngine.UI;

public abstract class Turret : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected string turretName;
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] GameObject model;
    [SerializeField] Image thumbnail;

    [Header("Stats")]
    [SerializeField] protected int cost = 1;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float health = 1000f;
    [SerializeField] protected float range = 10f;
    [SerializeField] protected float attackSpeed = 1f;

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