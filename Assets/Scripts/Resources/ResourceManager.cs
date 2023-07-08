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

    [Header("Dropping event")]
    [SerializeField] private int dropWoodAmount;
    [SerializeField] private int dropFoodAmount;

    [Header("UI")]
    [SerializeField] private TextMeshPro woodDisplay;
    [SerializeField] private TextMeshPro foodDisplay;

    private void Awake()
    {
        Timer woodTimer = new Timer(dropWoodWait, this, WoodDrop);
        Timer foodTimer = new Timer(dropFoodWait, this, FoodDrop);
        Timer populationTimer = new Timer(populateWait, this, IncreasePopulation);
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (wood < 0) WoodDepleted();
        if (food < 0) FoodDepleted();
    }

    public void WoodDrop()
    {
        wood -= dropWoodAmount;
        //woodDisplay.text = wood.ToString();
    }

    public void FoodDrop()
    {
        food -= dropFoodAmount;
        //foodDisplay.text = food.ToString();
    }

    public void FoodChange(int foodAmount)
    {
        food += foodAmount;
        foodDisplay.text = food.ToString();
    }

    public void WoodChange(int woodAmount)
    {
        wood += woodAmount;
        Debug.Log("wood:" + wood);
        //woodDisplay.text = wood.ToString();
    }

    public void IncreasePopulation()
    {
        people*= populateRate;
    }

    public void DepletionIncrease()
    {
        if(people > populationLimit)
        {
            dropFoodAmount *= dropFoodRate;
            dropWoodAmount *= dropWoodRate;
        }
    }

    public void FoodDepleted()
    {

    }

    public void WoodDepleted()
    {

    }

}
