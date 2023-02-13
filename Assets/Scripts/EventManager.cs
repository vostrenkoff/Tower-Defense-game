using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public event Action DestroyEvent;
    public event Action<int, int> BuildEvent;
    public event Action<float> UpgradeEvent;
    public event Action StopActionsEvent;

    public event Action<float> PayEvent;
    public event Action<int> GetMoneyEvent;
    public event Action CancelActionsEvent;

    public event Action GameOverEvent;

    public event Action<float> DamageBaseEvent;
    public void OnDestroyEvent() => DestroyEvent?.Invoke();
    public void OnBuildEvent(int towerType, int towerPrice) => BuildEvent?.Invoke(towerType, towerPrice);
    public void OnUpgradeEvent(float costMultiplier) => UpgradeEvent?.Invoke(costMultiplier);
    public void OnStopActionsEvent() => StopActionsEvent?.Invoke();
    public void OnPayEvent(float price) => PayEvent?.Invoke(price);
    public void OnGetMoneyEvent(int amount) => GetMoneyEvent?.Invoke(amount);
    public void OnCancelActionsEvent() => CancelActionsEvent?.Invoke();
    public void OnGameOverEvent() => GameOverEvent?.Invoke();
    public void OnDamageBaseEvent(float damage) => DamageBaseEvent?.Invoke(damage);
}
