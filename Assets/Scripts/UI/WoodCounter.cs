using System;
using TMPro;
using UnityEngine;

namespace UI {
    public class WoodCounter : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI woodText;
        private void Start() {
            void Converter(int wood) => woodText.text = wood.ToString();
            ResourceManager.OnWoodChange += Converter;
            ResourceManager.OnStateChange += (state => {
                ResourceManager.OnWoodChange -= Converter;
                Destroy(gameObject);
            });
        }
    }
}
