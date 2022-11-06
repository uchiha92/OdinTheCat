using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthPoints : MonoBehaviour
{
    private PlayerController _playerController;
    private Vector3 _thisPosition;
    private float _initialPointPositionX;
    
    [SerializeField]
    private Image healthPoint;
    [SerializeField]
    private List<Image> currentHealthPoints;
    [SerializeField] 
    private HealthPlayerChannel healthPlayerChannel;
    
    private void Start()
    {
        _playerController = PlayerController.Instance;
        currentHealthPoints = new List<Image>();
        _thisPosition = this.transform.position;
        _initialPointPositionX = _thisPosition.x - 50;
        healthPlayerChannel.OnHealthDecrease += OnHealthDecrease;
        healthPlayerChannel.OnHealthIncrease += OnHealthIncrease;
        GenerateHealthPoints();
    }

    private void OnDestroy()
    {
        healthPlayerChannel.OnHealthDecrease -= OnHealthDecrease;
        healthPlayerChannel.OnHealthIncrease -= OnHealthIncrease;
    }

    private void GenerateHealthPoints()
    {
        float currentHealthPointPositionX = _initialPointPositionX;
        for (var i = 0; this._playerController.GetMaxHealth() > i; i++)
        {
            Image point = (Image) Instantiate(healthPoint);
            point.transform.SetParent(this.transform, false);
            point.transform.position =
                new Vector3( currentHealthPointPositionX, _thisPosition.y, _thisPosition.z);
            currentHealthPoints.Add(point);
            currentHealthPointPositionX = currentHealthPointPositionX + 20;
        }
    }

    private void OnHealthDecrease()
    {
        currentHealthPoints[_playerController.GetHealthPlayer()].enabled = false;
    }

    private void OnHealthIncrease(int value)
    {
        int healthIndex;
        bool canIncrease;
        for (var i = 0; value > i; i++)
        {
            healthIndex = _playerController.GetHealthPlayer() - (value - i);
            canIncrease =  currentHealthPoints.Count > healthIndex;
            if (canIncrease)
            {
                currentHealthPoints[healthIndex].enabled = true;
            }
        }
    }
}
