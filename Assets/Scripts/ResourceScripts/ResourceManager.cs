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

    public State CurrentState {
        get => _currentState;

        set {
            _currentState = value;
            OnStateChange?.Invoke(value);
        }
    }

    private State _currentState;

    public static event Action<State> OnStateChange;

    public static ResourceManager instance { get; private set; }

    private int health;

    private int wood;

    public int Wood {
        get => wood;
        set {
            wood = value;
            Debug.Log("Amount of Wood in bank: " + value);
        }
    }
    public Action onLoseEvent;
    


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public int Health
    {
        get => health;
        set
        {
            health = value;
            Debug.Log("Player current health: " + health);
            if (value <= 0)
            {
                onLoseEvent?.Invoke();
            }
        }
    }
}
