using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_EnemyMovementChannel",menuName = "Data/Channels/EnemyMovementChannel")]
public class EnemyMovementChannel : ScriptableObject
{
    public Action OnBumping;

    public void InvokeOnBumping()
    {
        OnBumping?.Invoke();
    }
}
