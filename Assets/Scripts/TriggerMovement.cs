using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovement : MonoBehaviour
{
    private bool _turnAround;

    private void SetTurnAround(bool value)
    {
        _turnAround = value;
    }

    public bool GetTurnAround()
    {
        return _turnAround;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            SetTurnAround(!_turnAround);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            SetTurnAround(!_turnAround);
        }
    }
}
