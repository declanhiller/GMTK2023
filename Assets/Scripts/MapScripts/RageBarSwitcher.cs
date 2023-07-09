using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBarSwitcher : MonoBehaviour {

    [SerializeField] private Color rageColor;
    [SerializeField] private Image slider;
    [SerializeField] private Sprite _rageBarSprite;
    
    // Start is called before the first frame update
    void Start() {
        ResourceManager.OnStateChange += OnStateChange;
    }

    private void OnStateChange(ResourceManager.State state) {
        if (state != ResourceManager.State.nature) return;
        slider.color = rageColor;
        this.GetComponent<Image>().sprite = _rageBarSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
