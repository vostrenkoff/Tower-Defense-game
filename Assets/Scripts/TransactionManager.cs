using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransactionManager : MonoBehaviour
{
    private List<IMoneyObserver> _observers = new List<IMoneyObserver>();

    private EventManager _eventManager;
    private GameObject _buyBasicButton;
    private GameObject _buySlowButton;
    private GameObject _buyAOEButton;
    private int _towerType;

    public static bool BuildAllowence = false;
    public static bool UpgradeAllowence = false;
    public static bool DestroyAllowence = false;
    public static int CurrentMoney;

    public Text MoneyText;
    public GameObject CancelButton;

    public int StartMoney = 200;
    public int BasicTowerPrice = 30;
    public int SlowTowerPrice = 40;
    public int AoeTowerPrice = 70;
    public float UpgradeCostMultiplier = 100f;

    public static int GetCurrentMoney()
    {
        return CurrentMoney;
    }
    private void Start()
    {
        _eventManager = FindObjectOfType<EventManager>();
        CurrentMoney = StartMoney;

        _eventManager.PayEvent += Pay;
        _eventManager.CancelActionsEvent += CancelActions;
        _eventManager.GetMoneyEvent += GetGold;

        _buyBasicButton = GameObject.Find("buyBasicTowerText");
        _buySlowButton = GameObject.Find("buySlowTowerText");
        _buyAOEButton = GameObject.Find("buyAOETowerText");

        _buyBasicButton.GetComponent<Text>().text = "Buy: " + Mathf.Round(BasicTowerPrice);
        _buySlowButton.GetComponent<Text>().text = "Buy: " + Mathf.Round(SlowTowerPrice);
        _buyAOEButton.GetComponent<Text>().text = "Buy: " + Mathf.Round(AoeTowerPrice);
        
        NotifyObservers();
        UpdateShop();
    }

    
    private void UpdateShop()
    {
        if (BasicTowerPrice > CurrentMoney)
            _buyBasicButton.GetComponent<Text>().color = Color.red;
        else
            _buyBasicButton.GetComponent<Text>().color = Color.green;
        if (SlowTowerPrice > CurrentMoney)
            _buySlowButton.GetComponent<Text>().color = Color.red;
        else
            _buySlowButton.GetComponent<Text>().color = Color.green;
        if (AoeTowerPrice > CurrentMoney)
            _buyAOEButton.GetComponent<Text>().color = Color.red;
        else
            _buyAOEButton.GetComponent<Text>().color = Color.green;
    }
    public void BuyBasicTower()
    {
        if (CurrentMoney >= BasicTowerPrice)
        {
            CancelActions();
            BuildAllowence = SwitchTool();
            _towerType = 0;
            _eventManager.OnBuildEvent(_towerType, BasicTowerPrice);
        }
    }
    public void BuySlowTower()
    {
        if (CurrentMoney >= SlowTowerPrice)
        {
            CancelActions();
            BuildAllowence = SwitchTool();
            _towerType = 1;
            _eventManager.OnBuildEvent(_towerType, SlowTowerPrice);
        }
    }
    public void BuyAOETower()
    {
        if (CurrentMoney >= AoeTowerPrice)
        {
            CancelActions();
            BuildAllowence = SwitchTool();
            _towerType = 2;
            _eventManager.OnBuildEvent(_towerType, AoeTowerPrice);
        }
    }
    public void UpgradeTowerTool()
    {
        if (UpgradeAllowence == true)
            return;
        UpgradeAllowence = SwitchTool();
        CancelButton.SetActive(true);
        _eventManager.OnUpgradeEvent(UpgradeCostMultiplier);
    }
    public void DestroyTool()
    {
        DestroyAllowence = SwitchTool();
        CancelButton.SetActive(true);
        _eventManager.OnDestroyEvent();
    }
    public void Pay(float price)
    {
        CurrentMoney -= (int)price;
        CancelActions();
        NotifyObservers();
    }
    public void CancelActions()
    {
        BuildAllowence = false;
        UpgradeAllowence = false;
        DestroyAllowence = false;
        CancelButton.SetActive(false);
        _eventManager.OnStopActionsEvent();
    }
    private bool SwitchTool()
    {
        BuildAllowence = false;
        UpgradeAllowence = false;
        DestroyAllowence = false;
        return true;
    }

    private void GetGold(int amount)
    {
        CurrentMoney += amount;
        NotifyObservers();
    }
    public void AddObserver(IMoneyObserver observer)
    {
        _observers.Add(observer);
    }
    private void NotifyObservers()
    {
        UpdateShop();
        foreach (IMoneyObserver observer in _observers)
        {
            observer.OnMoneyChanged(CurrentMoney);
        }
    }
}
public interface IMoneyObserver
{
    void OnMoneyChanged(float money);
}