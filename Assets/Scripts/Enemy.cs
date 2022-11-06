using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _runningSpeed;
    private Rigidbody2D _rigidbody2D;

    [SerializeField] 
    private TriggerMovement triggerMovement;

    private void Awake()
    {
        this._runningSpeed = 3.0f;   
        this._rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float currentRunningSpeed = this._runningSpeed;
        if (triggerMovement.GetTurnAround())
        {
            currentRunningSpeed = -this._runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else
        {
            this.transform.eulerAngles = Vector3.zero;
        }

        if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            if (this._rigidbody2D.velocity.x < this._runningSpeed && this._rigidbody2D.velocity.x > -this._runningSpeed)
            {
                this._rigidbody2D.velocity = new Vector2(currentRunningSpeed, this._rigidbody2D.velocity.y);
            }
        }
    }
}
