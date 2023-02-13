using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IMoneyObserver, IWavesObserver
{
    [SerializeField] private GameObject _timeText;
    [SerializeField] private GameObject _waveText;
    [SerializeField] private GameObject _moneyText;
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _gameOverText;

    private TransactionManager _transactionManager;
    private EnemySpawner _enemySpawner;
    private GameManager _gameManager;
    private EventManager _eventManager;
    private bool _isBreak = true;
    private void Start()
    {
        _transactionManager = FindObjectOfType<TransactionManager>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _gameManager = FindObjectOfType<GameManager>();
        _eventManager = FindObjectOfType<EventManager>();

        _transactionManager.AddObserver(this);
        _enemySpawner.AddObserver(this);

        _eventManager.GameOverEvent += GameOverFunct;
    }
    private void Update()
    {
        if (_isBreak)
        {
            _timeText.GetComponent<Text>().text = "Time left: " + Mathf.Round(_enemySpawner.GetSecondsBeforeStart);
        }
        else
            _timeText.GetComponent<Text>().text = "";
    }
    
    public void OnWaveChanged(int wave)
    {
        if (_isBreak)
            _waveText.GetComponent<Text>().text = "Time to build!";
        else
        {
            _waveText.GetComponent<Text>().text = "Wave number: " + wave;
        }
        if(wave > 5 && _gameManager.BaseHealth > 0)
        {
            _winText.SetActive(true);
        }
        
    }
    public void OnBreakChanged(bool isBreak)
    {
        _isBreak= isBreak;
    }
    public void OnMoneyChanged(float money)
    {
        _moneyText.GetComponent<Text>().text = "Money " + money.ToString();
    }
    private void GameOverFunct()
    {
        _gameOverText.SetActive(true);
    }
    
}