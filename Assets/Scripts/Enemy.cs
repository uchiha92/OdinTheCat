using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _runningSpeed;
    private Vector2 _localScale;
    private Rigidbody2D _rigidbody2D;

     
    //public TriggerMovement triggerMovement;
    [SerializeField] 
    private EnemyMovementChannel enemyMovementChannel;

    private void Awake()
    {
        this._runningSpeed = 3.0f;   
        this._rigidbody2D = GetComponent<Rigidbody2D>();
        this._localScale = this.transform.localScale;
        this.enemyMovementChannel.OnBumping += OnBumping;
    }

    private void OnDestroy()
    {
        this.enemyMovementChannel.OnBumping -= OnBumping;
    }

   private bool IsFacingRight()
   {
       return this.transform.localScale.x > Mathf.Epsilon;
   }

    private void FixedUpdate()
    {
        /*float currentRunningSpeed = this._runningSpeed;
        if (triggerMovement._turnAround)
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
        }*/

        this._rigidbody2D.velocity = 
            IsFacingRight() ? 
                new Vector2(this._runningSpeed, 0f) : 
                new Vector2(-this._runningSpeed, 0f);
    }

    private void OnBumping()
    {
        Vector2 localScale = this.transform.localScale;
        this.transform.localScale = new Vector2(-(Mathf.Sign(this._rigidbody2D.velocity.x)), localScale.y);
    }
}
