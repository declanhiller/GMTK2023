using System;
using UnityEngine;

public class StateSwitcherController : MonoBehaviour {

    [SerializeField] private GameObject text;
    [SerializeField] private float timeToStickAround = 3;
    
    private void Start() {
        ResourceManager.OnStateChange += DisplayBoxOfWords;
    }

    private void DisplayBoxOfWords(ResourceManager.State obj) {
        text.SetActive(true);
        Invoke(nameof(MakeItDissapear), timeToStickAround);
    }

    private void MakeItDissapear() {
        text.SetActive(false);
    }
}