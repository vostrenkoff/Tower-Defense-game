using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemyList = new List<GameObject>();
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private float _spawnDelay = 0.8f;
    [SerializeField] private int _newEnemiesEveryWave = 2;
    [SerializeField] private float _breakTime = 20;

    private GameManager _gameManager;
    private List<IWavesObserver> _observers = new List<IWavesObserver>();
    private int _wave = 0;
    private bool _spawnerIsDone = false;
    private float _secondsBeforeStart;
    private int _enemyCount = 0;
    private bool _isBreak = true;

    public float GetSecondsBeforeStart
    {
        get { return _secondsBeforeStart; }
    }
    public bool IsBreak
    {
        get { return _isBreak; }
        set { _isBreak = value; }
    }
    private void Start()
    {
        NotifyObservers();
        _gameManager = FindObjectOfType<GameManager>();
        _secondsBeforeStart = _breakTime;
    }
    private void Update()
    {
        if (_isBreak)
        {
            _secondsBeforeStart -= Time.deltaTime;
            NotifyObservers();
        }

        if (_secondsBeforeStart <= 0)
        {
            StartCoroutine(SpawnEnemy(_enemyCount + _newEnemiesEveryWave));
            _secondsBeforeStart = _breakTime;
            _wave++;
            NotifyObservers();
        }
        if (_spawnerIsDone)
            CheckEnemies();
    }
    IEnumerator SpawnEnemy(int enemyCount)
    {
        _isBreak = false;
        _spawnerIsDone = false;
        _enemyCount += _newEnemiesEveryWave;
        NotifyObservers();
        for (int i = 0; i < enemyCount; i++)
        {
            int randomEnemyID = UnityEngine.Random.Range(0, _enemyList.Count);

            GameObject newEnemy = Instantiate(_enemyList[randomEnemyID]);
            newEnemy.transform.SetParent(_spawnPoint.transform, false);
            newEnemy.GetComponent<Enemy>().ThisEnemy = new EnemyClass(_gameManager.Enemies[randomEnemyID]);

            yield return new WaitForSeconds(_spawnDelay);
        }

        _spawnerIsDone = true;
        _secondsBeforeStart = _breakTime;
        NotifyObservers();
    }
    private void CheckEnemies()
    {
        if (GameObject.FindGameObjectsWithTag("enemy").Length == 0) //  Check if any enemies left
        {
            if (_wave <= 5)
            {
                _isBreak = true;
                NotifyObservers();
            }
        }
        else
        {
            _isBreak = false;
            _secondsBeforeStart = _breakTime;
            NotifyObservers();
        }
    }

    public void AddObserver(IWavesObserver observer)
    {
        _observers.Add(observer);
    }
    private void NotifyObservers()
    {
        if (_observers.Count == 0 || _observers == null) return;
        foreach (IWavesObserver observer in _observers)
        {
            observer.OnWaveChanged(_wave);
            observer.OnBreakChanged(IsBreak);
        }
    }
}
public interface IWavesObserver
{
    void OnWaveChanged(int wave); //sending current wave
    void OnBreakChanged(bool isBreak);
}