using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Serialization;
using WeatherEvents;

public class ResourceManager : MonoBehaviour
{

    public enum State
    {
        human, nature
    }

    public bool _gameHasEnded;

    public State CurrentState {
        get => _currentState;

        set {
            _currentState = value;
            Debug.Log("State Changed");
            OnStateChange?.Invoke(value);
        }
    }

    private State _currentState;

    public static event Action<State> OnStateChange;

    public static ResourceManager instance { get; private set; }

    private int health;
    public int startingMaxHealth;

    private int wood;
    
    public static event Action<int> OnWoodChange;
    
    public int Wood {
        get => wood;
        set {
            wood = value;
            OnWoodChange?.Invoke(value);
        }
    }
    public static event Action OnColonialLose;
    public static event Action OnNatureLose;
    public static event Action OnNatureWin;
    


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        health = startingMaxHealth;
    }

    public static event Action<float> OnHealthChange;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            OnHealthChange?.Invoke(value);
            if (value > 0) return;
            health = 0;
            Debug.Log("Colonial Lose");
            if (CurrentState == State.human) {
                if (_gameHasEnded) return;
                OnColonialLose?.Invoke();
                _gameHasEnded = true;
            }
            else {
                if (_gameHasEnded) return;
                OnNatureWin?.Invoke();
                _gameHasEnded = true;
            }
            
        }
    }

    public void NatureGameOver() {
        if (_gameHasEnded) return;
        OnNatureLose?.Invoke();
        _gameHasEnded = false; //game can still be won
    }
}
