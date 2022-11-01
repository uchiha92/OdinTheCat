using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovement : MonoBehaviour
{
    //public bool _turnAround;
    [SerializeField] 
    private EnemyMovementChannel enemyMovementChannel;

   /* private void SetTurnAround(bool value)
    {
        this._turnAround = value;
    }

    public bool GetTurnAround()
    {
        return this._turnAround;
    }*/
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            //SetTurnAround(true);
            enemyMovementChannel.InvokeOnBumping();
        }
        
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            //SetTurnAround(true);
            enemyMovementChannel.InvokeOnBumping();
        }
    }
}
