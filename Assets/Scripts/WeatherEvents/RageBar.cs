using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WeatherEvents {
    public class RageBar : MonoBehaviour {

        [SerializeField] private float maxNumberOfRagePoints;

        [FormerlySerializedAs("_slider")] [SerializeField] private Slider slider;
        
        private void Start() {
            slider.maxValue = maxNumberOfRagePoints;
            slider.value = slider.maxValue;
        }

        public void AddListener(UnityAction<float> updateButtonsBasedOnRagePoints) {
            slider.onValueChanged.AddListener(updateButtonsBasedOnRagePoints);
        }

        public void DecreaseRageBar(float value) {
            slider.value -= value;
        }
    }
}
