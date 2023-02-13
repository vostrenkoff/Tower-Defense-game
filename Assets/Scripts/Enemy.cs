using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthBarController _healthbar;
    private List<GameObject> _waypoints = new List<GameObject>();
    private EventManager _eventManager; 
    private Text _text;
    private bool _isDead = false;
    private bool _isSlowed = false;
    private int _wayInd=0; // current waypoint aim
    private float _maxHP;
    
    public GameObject SpawnPoint;
    public EnemyClass ThisEnemy;

    void Start()
    {
        _eventManager = FindObjectOfType<EventManager>();

        if (ThisEnemy != null)
        {
            _maxHP = ThisEnemy.Health;
            _healthbar.UpdateHP(_maxHP, ThisEnemy.Health);
            _waypoints = GameObject.Find("GameManager").GetComponent<GameManager>().Waypoints;
        }
    }

    void Update()
    {
        if (ThisEnemy != null)
        {
            Move();
            IsAlive();
        }
    }

    private void Move()
    {
        Vector3 direction = _waypoints[_wayInd].transform.position - transform.position;

        if(!_isDead)
            transform.Translate(direction.normalized * Time.deltaTime * ThisEnemy.Speed);
        
        if(Vector3.Distance(transform.position, _waypoints[_wayInd].transform.position) < 0.3f) // Player moves by List of waypoints
        {
            if (_wayInd == _waypoints.Count - 1)
            {
                _eventManager.OnDamageBaseEvent(ThisEnemy.Health);
                Destroy(gameObject);
            }
            else
                _wayInd++;
        }
    }
    public void TakeDamage(float damage)
    {
        if (ThisEnemy != null)
        {
            ThisEnemy.Health -= damage;
            _healthbar.UpdateHP(_maxHP, ThisEnemy.Health);
        }
    }
    public void TakeAOEDamage(float range, float damage)
    {
        TakeDamage(damage);
        foreach(GameObject nearby in GameObject.FindGameObjectsWithTag("enemy")) 
        {
            if(Vector2.Distance(transform.position, nearby.transform.position) < range) {
                nearby.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }
    public void TakeSlowDamage(float dur, float strength, float damage)
    {
        TakeDamage(damage);
        StartCoroutine(IsSlow(dur, strength));
    }
    private void IsAlive()
    {
        if(ThisEnemy != null && ThisEnemy.Health <= 0 && !_isDead) {
            _isDead = true;
            StartCoroutine(Dead());
        }
    }

    IEnumerator IsSlow(float dur, float strength)
    {
        if (!_isSlowed)
        {
            float normalspeed = ThisEnemy.Speed;
            _isSlowed = true;
            ThisEnemy.Speed -= strength;

            yield return new WaitForSeconds(dur);

            ThisEnemy.Speed = normalspeed;
            _isSlowed = false;
        }
    }
    IEnumerator Dead()
    {
        _isDead= true;

        //Create text showing gained gold
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        _text = gameObject.AddComponent<Text>();
        _text.font = ArialFont;
        _text.material = ArialFont.material;
        _text.color = Color.yellow;
        _text.GetComponent<Text>().text = "+" + ThisEnemy.Gold;
        _text.alignment= TextAnchor.MiddleCenter;
        _text.fontSize= 30;

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        _eventManager.OnGetMoneyEvent((int)ThisEnemy.Gold);

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
