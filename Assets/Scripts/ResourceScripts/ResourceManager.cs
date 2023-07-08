using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using WeatherEvents;

public class ResourceManager : MonoBehaviour
{

    public enum State
    {
        human, nature
    }

    public State currentState;

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
