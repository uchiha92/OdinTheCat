using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_KillPlayerChannel",menuName = "Data/Channels/KillPlayerChannel")]
public class KillPlayerChannel : ScriptableObject
{
    public Action OnDead;

    public void InvokeOnDead()
    {
        OnDead?.Invoke();
    }
}
