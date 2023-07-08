using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager instance { get; private set; }

    private int wood;
    private int food;
    private int people;

    [Header("Timer")]
    [SerializeField] private int dropWoodWait;
    [SerializeField] private int dropFoodWait;
    [SerializeField] private int populateWait;

    [Header("Factors")]
    [SerializeField] private int dropWoodRate;
    [SerializeField] private int dropFoodRate;
    [SerializeField] private int populateRate;
    [SerializeField] private int populationLimit;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    
    
    
    public void WoodChange(int woodAmount)
    {
        wood += woodAmount;
        Debug.Log("Amount of wood in bank: " + wood);
    }

}
