using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _runningSpeed;
    private Rigidbody2D _rigidbody2D;

    [SerializeField] 
    private TriggerMovement triggerMovement;
    [SerializeField] 
    private DamagePlayerTriggerChannel damagePlayerTriggerChannel;

    private void Awake()
    {
        _runningSpeed = 3.0f;   
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //Ignoramos las colisiones entre enemigos
        Physics2D.IgnoreLayerCollision(8, 8, true);
    }

    private void FixedUpdate()
    {
        float currentRunningSpeed = _runningSpeed;
        if (triggerMovement.GetTurnAround())
        {
            currentRunningSpeed = -_runningSpeed;
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        if (GameManager.Instance.GetGameState().Equals(EGameState.InTheGame))
        {
            if (_rigidbody2D.velocity.x < _runningSpeed && _rigidbody2D.velocity.x > -_runningSpeed)
            {
                _rigidbody2D.velocity = new Vector2(currentRunningSpeed, _rigidbody2D.velocity.y);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Si el enemigo colisiona con el jugador, invoca el Action OnDamagePlayer
        if (other.gameObject.CompareTag("Player"))
        {
            damagePlayerTriggerChannel.InvokeOnDamagePlayer();
        }
    }
}
