using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager instance { get; private set; }

    private int wood;

    public int Wood {
        get => wood;
        set {
            wood = value;
            Debug.Log("Amount of Wood in bank: " + value);
        }
    }


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}
