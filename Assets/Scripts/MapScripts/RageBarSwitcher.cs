using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBarSwitcher : MonoBehaviour {

    [SerializeField] private Color rageColor = new Color(243, 34, 99);
    [SerializeField] private Image slider;
    
    // Start is called before the first frame update
    void Start() {
        ResourceManager.OnStateChange += OnStateChange;
    }

    private void OnStateChange(ResourceManager.State state) {
        if (state != ResourceManager.State.nature) return;
        slider.color = rageColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
