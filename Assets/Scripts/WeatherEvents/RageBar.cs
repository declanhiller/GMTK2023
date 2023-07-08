using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WeatherEvents {
    public class RageBar : MonoBehaviour {

        [SerializeField] private float maxNumberOfRagePoints;

        [SerializeField] private Slider _slider;
        
        private void Start() {
            _slider.maxValue = maxNumberOfRagePoints;
            _slider.value = _slider.maxValue;
        }

        public void AddListener(UnityAction<float> updateButtonsBasedOnRagePoints) {
            _slider.onValueChanged.AddListener(updateButtonsBasedOnRagePoints);
        }

        public void DecreaseRageBar(float value) {
            _slider.value -= value;
        }
    }
}
