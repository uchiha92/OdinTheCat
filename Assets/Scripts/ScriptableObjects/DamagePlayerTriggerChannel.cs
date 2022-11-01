using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_DamagePlayerTriggerChannel",menuName = "Data/Channels/DamagePlayerTriggerChannel")]
public class DamagePlayerTriggerChannel : ScriptableObject
{
    public Action OnDamagePlayer;

    public void InvokeOnDamagePlayer()
    {
        OnDamagePlayer?.Invoke();
    }
}
