using System;
using UnityEngine;

namespace MapScripts {
    public class MotherNatureSwitcher : MonoBehaviour {

        [SerializeField] private GameObject renderer;
        // [SerializeField] private Sprite newSprite;
        
        private void Start() {
            ResourceManager.OnStateChange += (state) => renderer.SetActive(true);
        }

    }
}
