using System;
using TMPro;
using UnityEngine;
using WeatherEvents;

namespace UI {
    public class RageBarDisplayer : MonoBehaviour {

        [SerializeField] private RageBar rageBar;
        private TextMeshProUGUI _text;
        
        private void OnEnable() {
            _text = GetComponent<TextMeshProUGUI>();
            rageBar.AddListener(UpdateRage);
        }

        private void UpdateRage(float value) {
            _text.text = value.ToString();
        }

    }
}
