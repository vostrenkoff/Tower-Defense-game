using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Tower : MonoBehaviour
{
    private GameManager _gameManager;
    private List<List<string>> spriteLookup = new List<List<string>>();

    public TowerClass ThisTower;
    public TowerType TowerType;
    public GameObject BulletPref;
    public int Level;
    public Sprite[] Sprites;
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        ThisTower = _gameManager.Towers[(int)TowerType];
        GetComponent<SpriteRenderer>().sprite = ThisTower.Sprite;

        spriteLookup.Add(new List<string> { "Tower0Level0", "lvl1" });
        spriteLookup.Add(new List<string> { "Tower0Level1", "lvl2" });
        spriteLookup.Add(new List<string> { "Tower0Level2", "lvl3" });
        spriteLookup.Add(new List<string> { "Tower1Level0", "slowlvl1" });
        spriteLookup.Add(new List<string> { "Tower1Level1", "slowlvl2" });
        spriteLookup.Add(new List<string> { "Tower1Level2", "slowlvl3" });
        spriteLookup.Add(new List<string> { "Tower2Level0", "aoelvl1" });
        spriteLookup.Add(new List<string> { "Tower2Level1", "aoelvl2" });
        spriteLookup.Add(new List<string> { "Tower2Level2", "aoelvl3" });

    }
    private void Update()
    {
        if(CanShoot())
        {
            Search();
        }
        if(ThisTower.CurrCooldown > 0)
        {
            ThisTower.CurrCooldown -= Time.deltaTime; 
        }
        
    }
    bool CanShoot()
    {
        if(ThisTower.CurrCooldown <= 0)
        {
            return true;
        }
        return false;
    }

    void Search()
    {
        GameObject target = null;
        float nearestTargetFloat = Mathf.Infinity;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            float currDistance = Vector2.Distance(transform.position, enemy.transform.position);
            if (currDistance < nearestTargetFloat && currDistance <= ThisTower.Range)
            {
                target = enemy;
                nearestTargetFloat = currDistance;
            }
        }
        if (target != null)
        {
            if (target.GetComponent<Enemy>().ThisEnemy.Health > 0)
            {
                Shoot(target.transform);
            }
        }
    }

    void Shoot(Transform enemyPos)
    {
        ThisTower.CurrCooldown = ThisTower.Cooldown;

        GameObject _bullet = Instantiate(BulletPref);
        _bullet.transform.position = transform.position;
        _bullet.GetComponent<BulletScript>().Destination(enemyPos);
        _bullet.GetComponent<BulletScript>().TowerLevel= Level;
        _bullet.GetComponent<BulletScript>().ThisTower = ThisTower;
    }
    public void Upgrade()
    {
        if (Level < 2)
        {
            Level += 1;
            GetSpriteForTower(ThisTower.Type, Level);
        }
        
    }
   
    public void GetSpriteForTower(int towerType, int towerLevel) // When upgrading tower, sprite changes
    {
        string key = $"Tower{towerType}Level{towerLevel}";
        foreach (var item in spriteLookup)
        {
            if (item[0] == key)
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(item[1]);
            }
        }
    }
}

       