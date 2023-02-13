using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour 
{
    [SerializeField] private HealthBarController _healthbar;
    private EventManager _eventManager;
    private GameManager _gameManager;
    private float _hp;
    private float _maxHP;

    private void Start()
    {
        _eventManager = FindObjectOfType<EventManager>();
        _gameManager = FindObjectOfType<GameManager>();
        _hp = _gameManager.BaseHealth;
        _maxHP = _hp;

        _eventManager.DamageBaseEvent += TakeDamage;
    }
    private void TakeDamage(float _damage)
    {
        _hp -= _damage;
        _healthbar.UpdateHP(_maxHP, _hp);
        if(_hp < 0)
        {
            _eventManager.OnGameOverEvent();
        }
    }
}