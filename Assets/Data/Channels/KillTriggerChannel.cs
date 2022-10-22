using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/KillTrigger/KillTriggerChannel")]
public class KillTriggerChannel : ScriptableObject
{
    public delegate void OnDiedCallback(KillTrigger killTrigger);
    public OnDiedCallback OnDied;

    public void RaiseDied(KillTrigger killTrigger)
    {
        OnDied?.Invoke(killTrigger);
    }
}
