using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
    }
    public void UpdateHP(float health, float currenntHP)
    {
        _healthbarSprite.fillAmount = currenntHP / health;
    }
    
}
