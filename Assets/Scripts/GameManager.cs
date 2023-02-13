using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    SINGLE_TOWER,
    SLOW_TOWER,
    AOE_TOWER
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private float _baseHealthOnStart = 300;
    [SerializeField] private float _enemiesHealthMultiplier = 1;
    [SerializeField] private float _enemiesGoldMultiplier = 1;
    [SerializeField] private float _enemiesSpeedMultiplier = 1;
    
    private EventManager _eventManager;
    private Sprite basicLVL1;
    private Sprite slowLVL1;
    private Sprite aoeLVL1;


    public List<TowerClass> Towers = new List<TowerClass>();
    public List<BulletClass> Bullets = new List<BulletClass>();
    public List<EnemyClass> Enemies = new List<EnemyClass>();
    public List<GameObject> Waypoints = new List<GameObject>();
    public GameObject GameOverText;
    public GameObject PlayerWonText;

    private void Awake() // Adding here every type of tower, enemy or bullet that will be used in game
    {
        _eventManager = FindObjectOfType<EventManager>(); 

        basicLVL1 = Resources.Load<Sprite>("lvl1");
        slowLVL1 = Resources.Load<Sprite>("slowlvl1");
        aoeLVL1 = Resources.Load<Sprite>("aoelvl1");

        Towers.Add(new TowerClass(1, 8, basicLVL1,0));
        Towers.Add(new TowerClass(1, 6, slowLVL1,1));
        Towers.Add(new TowerClass(2, 4, aoeLVL1,2));
        
        Enemies.Add(new EnemyClass(10 * _enemiesHealthMultiplier, 12 * _enemiesGoldMultiplier, 4 * _enemiesSpeedMultiplier));
        Enemies.Add(new EnemyClass(30 * _enemiesHealthMultiplier, 19 * _enemiesGoldMultiplier, 3 * _enemiesSpeedMultiplier));
        Enemies.Add(new EnemyClass(60 * _enemiesHealthMultiplier, 30 * _enemiesGoldMultiplier, 2 * _enemiesSpeedMultiplier));

        Bullets.Add(new BulletClass(12, 12, "bulletlvl1"));
        Bullets.Add(new BulletClass(20, 8, "bulletlvl2"));
        Bullets.Add(new BulletClass(8, 6, "bulletlvl3"));

    }
   
    public float BaseHealth
    {
        get { return _baseHealthOnStart; }
    }
    
    
}
    public class TowerClass
    {
        public Sprite Sprite;
        public float Cooldown, Range, CurrCooldown = 0;
        public int Type;
        public TowerClass(float cooldown, float range, Sprite sprite, int type)
        {
            Type = type;
            Cooldown = cooldown;
            Range = range;
            Sprite = sprite;
        }    
    }
    public class BulletClass
    {
        public float Speed;
        public Sprite Sprite;
        public int Damage;
        public BulletClass(float speed, int damage, string spritePath)
        {
            this.Speed = speed;
            Damage = damage;
            Sprite= Resources.Load<Sprite>(spritePath);
        }

    }
    public class EnemyClass
    {
        public float Health, Speed, Gold;
        public EnemyClass(float health,float gold ,float speed)
        {
            Health = health;
            Speed = speed;
            Gold = gold;
        }
        public EnemyClass(EnemyClass other) // Needed for AOE tower attack
        {
            Health = other.Health;
            Speed = other.Speed;
            Gold = other.Gold;
        }
    }





