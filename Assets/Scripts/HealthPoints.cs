using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


public class HealthPoints : MonoBehaviour
{
    private PlayerController _playerController;
    private Vector3 _thisPosition;
    private float _initialPointPositionX;
    private Animator _animator;
    private AudioSource _audioSource;
    
    [SerializeField]
    private Image healthPoint;
    [SerializeField]
    private List<Image> currentHealthPoints;
    [SerializeField] 
    private HealthPlayerChannel healthPlayerChannel;
    [SerializeField] 
    private GameStateChannel gameStateChannel;
    
    private void Start()
    {
        //inicializamos atributos y nos suscribimos a los Action de los Channels
        _playerController = PlayerController.Instance;
        currentHealthPoints = new List<Image>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _thisPosition = transform.position;
        _initialPointPositionX = _thisPosition.x - 280;
        healthPlayerChannel.OnHealthDecrease += OnHealthDecrease;
        healthPlayerChannel.OnHealthIncrease += OnHealthIncrease;
        healthPlayerChannel.OnLowHealth += OnLowHealth;
        healthPlayerChannel.OnHighHealth += OnHighHealth;
        gameStateChannel.OnChangeGameState += OnChangeGameState;
        GenerateHealthPoints(); //Generamos un GO por cada punto de vida máxima del jugador
    }

    private void OnDestroy()
    {
        //Nos desuscribimos de los Action de los channels
        healthPlayerChannel.OnHealthDecrease -= OnHealthDecrease;
        healthPlayerChannel.OnHealthIncrease -= OnHealthIncrease;
        healthPlayerChannel.OnLowHealth -= OnLowHealth;
        healthPlayerChannel.OnHighHealth -= OnHighHealth;
        gameStateChannel.OnChangeGameState -= OnChangeGameState;
    }

    private void GenerateHealthPoints()
    {
        //Definimos una variable que almacenará el punto para pintar el GO de salud, lo inicializamos
        float currentHealthPointPositionX = _initialPointPositionX;
        //Iteramos el número de puntos de salud máxima
        for (var i = 0; _playerController.GetMaxHealth() > i; i++)
        {
            //Instanciamos una Imagen con el prefab de healthPoint y le seteamos el transform
            Image point = (Image) Instantiate(healthPoint);
            point.transform.SetParent(transform, false);
            point.transform.position =
                new Vector3( currentHealthPointPositionX, _thisPosition.y, _thisPosition.z);
            //Añadimos la imagen a la lista 
            currentHealthPoints.Add(point);
            //Establecemos el punto para pintar el siguiente healthPoint
            currentHealthPointPositionX = currentHealthPointPositionX + 80;
        }
    }
    
    private void EnableHealthAlarm()
    {
        //animamos los puntos de salud y reproducimos la alarma en caso de peligro
        _animator.SetBool("isInDanger", true);
        _audioSource.Play();
    }
    
    private void DisableHealthAlarm()
    {
        //desactivamos la alarma y su animación
        _animator.SetBool("isInDanger", false);
        _audioSource.Stop();
    }

    private void OnHealthDecrease()
    {
        //desactivamos la imagen con índice correspondiente al punto de salud que acaba de perder el jugador
        currentHealthPoints[_playerController.GetHealthPlayer()].enabled = false;
    }

    private void OnHealthIncrease(int value)
    {
        //recibimos por parámetro los puntos de salud a restaurar
        //definimos una variable para el índice del punto de salud y un bool para saber si hemos llegado al máximo
        int healthIndex;
        bool canIncrease;
        //iteramos los puntos de salud a restaurar
        for (var i = 0; value > i; i++)
        {
            //Establecemos el valor del índice
            healthIndex = _playerController.GetHealthPlayer() - (value - i);
            //Establecemos el bool con el resultado de evaluar si los puntos de salud son mayores al índice
            canIncrease =  currentHealthPoints.Count > healthIndex;
            if (canIncrease)
            {
                //sí el indice es menor o igual a los puntos de salud, activamos la imagen
                currentHealthPoints[healthIndex].enabled = true;
            }
        }
    }

    private void OnHighHealth()
    {
        DisableHealthAlarm();
    }
    
    private void OnLowHealth()
    {
        EnableHealthAlarm();
    }
    
    private void OnChangeGameState(EGameState newGameState)
    {
        switch (newGameState)
        {
            case EGameState.InTheGame:
                DisableHealthAlarm();
                break;
            case EGameState.Menu:
                DisableHealthAlarm();
                break;
            case EGameState.GameOver:
                DisableHealthAlarm();
                break;
        }
    }
}
