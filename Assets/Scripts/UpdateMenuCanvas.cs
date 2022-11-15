using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateMenuCanvas : MonoBehaviour
{
    
    [SerializeField] 
    private GameStateChannel gameStateChannel;
    [SerializeField] 
    private Button playButton, creditsButton;

    private void Start()
    {
        gameStateChannel.OnChangeGameState += OnChangeGameState;
    }

    private void OnDestroy()
    {
        if (null != EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        gameStateChannel.OnChangeGameState -= OnChangeGameState;
    }

    private void DisableButtons()
    {
        playButton.interactable = false;
        creditsButton.interactable = false;
    }
    
    private void EnableButtons()
    {
        playButton.interactable = true;
        creditsButton.interactable = true;
        playButton.Select();
    }
    
    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    private void OnChangeGameState(EGameState newGameState)
    {
        switch (newGameState)
        {
            case EGameState.InTheGame:
                DisableButtons();
                break;
            case EGameState.Menu:
                EnableButtons();
                break;
            case EGameState.GameOver:
                DisableButtons();
                break;
        }
    }
}
