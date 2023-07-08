using System;
using Enemies;
using MapScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WeatherEvents {
    public class RageBar : MonoBehaviour {

        [SerializeField] private float maxNumberOfRagePoints;

        [SerializeField] private float buildingRagePoints;
        [SerializeField] private float forestDestroyRagePoints;
        [SerializeField] private float animalDestroyRagePoints;
        

        [SerializeField] private Map map;

        public float RagePoints {
            get => slider.value;
            set {
                slider.value = value;
                Debug.Log(slider.value);
            }
        }

        [FormerlySerializedAs("_slider")] [SerializeField] private Slider slider;
        
        
        
        private void Start() {
            slider.maxValue = maxNumberOfRagePoints;
            slider.value = 0;
            map.OnMapEvent += IncreaseRageBar;
            BasicEnemy.OnEnemyDeath += IncreaseRageBarForAnimalDeath;
        }

        private void IncreaseRageBarForAnimalDeath(BasicEnemy obj) {
            RagePoints += animalDestroyRagePoints;
        }

        private void IncreaseRageBar(Map.MapEvent mapEvent) {
            switch (mapEvent) {
                case Map.MapEvent.BuildingPlaced:
                    RagePoints += buildingRagePoints;
                    break;
                case Map.MapEvent.ForestDestroyed:
                    RagePoints += forestDestroyRagePoints;
                    break;
            }
        }

        public void AddListener(UnityAction<float> updateButtonsBasedOnRagePoints) {
            slider.onValueChanged.AddListener(updateButtonsBasedOnRagePoints);
        }
    }
}
