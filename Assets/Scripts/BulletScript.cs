using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private GameManager _gameManager;

    BulletClass ThisBullet;
    Transform Target;
    public TowerClass ThisTower;
    public int TowerLevel;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if(_gameManager.Bullets != null )
            ThisBullet = _gameManager.Bullets[ThisTower.Type]; // Changing bullet for specific tower
        GetComponent<SpriteRenderer>().sprite = ThisBullet.Sprite;
    }
    private void Update()
    {
        Move();
    }
    public void Destination(Transform target)
    {
        Target = target;
    }
    private void Hit()
    {
        float damage = ThisBullet.Damage + (TowerLevel * 5f);  // Types of attacks
        switch (ThisTower.Type)
        {
            case (int)TowerType.SINGLE_TOWER:
                Target.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
                break;
            case (int)TowerType.SLOW_TOWER:
                Target.GetComponent<Enemy>().TakeSlowDamage(2, 1.5f, damage);
                Destroy(gameObject);
                break;
            case (int)TowerType.AOE_TOWER:
                Target.GetComponent<Enemy>().TakeAOEDamage(2, damage);
                Destroy(gameObject);
                break;
        }
    }
    private void Move()
    {
        if(Target == null) {
            Destroy(gameObject);
            return;
        }
        if(Vector2.Distance(transform.position, Target.position) < 0.1f)
        {
            Hit();
        }    
        
        Vector2 dir = Target.position - transform.position; // Calculate direction and move towards
        transform.Translate(dir.normalized*Time.deltaTime * ThisBullet.Speed);
    }
}
