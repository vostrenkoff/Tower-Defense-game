using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cells : MonoBehaviour, IMoneyObserver
{
    [SerializeField] private int _cellType;
    private EventManager _eventManager;
    private TransactionManager _transactionManager;
    private EnemySpawner _enemySpawner;
    private float _upgradeCost = 0;
    private float _currentMoney = 0;

    private Text _text;         // Text for tower upgrade price
    private int _getTowerType;
    private int _price;
    private bool _isTaken = false;

    public Color Normal, Selected;
    public GameObject TowerPrefab;
    
    private void Start()
    {
        _transactionManager= FindObjectOfType<TransactionManager>();
        _transactionManager.AddObserver(this);
        _eventManager = FindObjectOfType<EventManager>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _eventManager.BuildEvent += BuildMode;
        _eventManager.UpgradeEvent += UpgradeMode;
        _eventManager.StopActionsEvent += StopActions;

        if (_cellType == 0)
            GetComponent<SpriteRenderer>().color = Color.white;
        else if(_cellType == 1)
            GetComponent<SpriteRenderer>().color = Color.red;

    }
    

    private void OnMouseEnter()
    {
        if(_cellType == 0)
            GetComponent<SpriteRenderer>().color = Color.black;
    }
    private void OnMouseExit() 
    {
        if (_cellType == 0)
            GetComponent<SpriteRenderer>().color = Color.white;
    }
    
    private void OnMouseDown()      // Interactions with cells
    {
        if (_enemySpawner.IsBreak)
        {
            if (!_isTaken && TransactionManager.BuildAllowence == true && _cellType == 0)
            {
                Build();
            }
            else if (_isTaken && TransactionManager.UpgradeAllowence == true)
            {
                UpgradeTower();
            }
            else if (_isTaken && TransactionManager.DestroyAllowence == true)
            {
                DestroyTower();
            }
        }

    }
    public void Build()
    {
            _isTaken = true;
            _eventManager.OnPayEvent(_price);       // Pay for build

            GameObject newTower = Instantiate(TowerPrefab);
            newTower.transform.SetParent(transform, false);
            newTower.transform.position = transform.position;
            newTower.transform.localScale = new Vector2(50f, 50f);
            newTower.GetComponent<Tower>().TowerType = (TowerType)_getTowerType;
    }
    public void UpgradeTower()
    {
        GameObject existingTower = this.gameObject.transform.GetChild(0).gameObject;
        if (_upgradeCost <= _currentMoney && existingTower.GetComponent<Tower>().Level < 2)
        {
            _eventManager.OnPayEvent(_upgradeCost);
            existingTower.GetComponent<Tower>().Upgrade();
            StopActions();
        }
    }

    public void BuildMode(int towerType, int price)  //  Getting type and price from Transaction manager, so script would know what to build
    {
        _getTowerType = towerType;
        _price = price;
    }
    public void UpgradeMode(float upgradeCostMultiplier) 
    {
        if (_isTaken)
        {
            GameObject existingTower = this.gameObject.transform.GetChild(0).gameObject; //Creating text for taken cells
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            _upgradeCost = upgradeCostMultiplier * (existingTower.GetComponent<Tower>().Level + 10 * (int)existingTower.GetComponent<Tower>().TowerType + 1);
            _text = gameObject.AddComponent<Text>();
            _text.font = ArialFont;
            _text.material = ArialFont.material;

            if (_currentMoney < _upgradeCost)
            {
                _text.color = Color.red;
            }
            else
            {
                _text.color = Color.green;
            }
            if (existingTower.GetComponent<Tower>().Level == 2)
            {
                _text.GetComponent<Text>().text = "MAX";
                _text.color = Color.blue;
            }
            else
            {
                _text.GetComponent<Text>().text = "" + _upgradeCost;
            }
        }
    }
    public void DestroyTower()
    {
            GameObject existingTower = this.gameObject.transform.GetChild(0).gameObject;
            int returnAmount = (existingTower.GetComponent<Tower>().Level + (int)existingTower.GetComponent<Tower>().TowerType + 1) * 10;
            
            _isTaken = false;
            _eventManager.OnGetMoneyEvent(returnAmount);
            _eventManager.OnCancelActionsEvent();
            Destroy(existingTower);
    }
    public void StopActions() // Stop upgrade tool
    {
        Destroy(gameObject.GetComponent<Text>());
    }
    public void OnMoneyChanged(float money)
    {
        _currentMoney = money;
    }
}
