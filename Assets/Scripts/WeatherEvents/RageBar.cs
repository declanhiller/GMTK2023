using System;
using System.Collections;
using System.Threading;
using Enemies;
using MapScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Towers;
using UI;

namespace WeatherEvents {
    public class RageBar : MonoBehaviour {

        [SerializeField] private float maxNumberOfRagePoints;

        [SerializeField] private float buildingRagePoints;
        [SerializeField] private float forestDestroyRagePoints;
        [SerializeField] private float animalDestroyRagePoints;
        [SerializeField] private float towerDestroyRagePoints;

        [SerializeField] private float regrowCostPoints;
        [SerializeField] private float PlacingCostPoints;


        [SerializeField] private Map map;

        private Coroutine _countdownCoroutine;

        public static RageBar Instance { get; private set; }

        public float RagePoints {
            get => slider.value;
            set {

                if (ForestDragger.Instance != null && MonsterDragger.Instance != null) {
                    if (value < ForestDragger.Instance.price && value < MonsterDragger.Instance.price) {
                        ResourceManager.instance.NatureGameOver();
                    }
                }

                slider.value = value;
            }
        }
        

        [SerializeField] private Slider slider;

        private void Awake() {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start() {
            slider.maxValue = maxNumberOfRagePoints;
            slider.value = 0;
            map.OnMapEvent += ChangeRageBar;
            BasicEnemy.OnEnemyDeath += IncreaseRageBarForAnimalDeath;
        }

        private void IncreaseRageBarForAnimalDeath(BasicEnemy obj) {
            if (ResourceManager.instance.CurrentState == ResourceManager.State.human)
                RagePoints += animalDestroyRagePoints;
        }

        private void ChangeRageBar(Map.MapEvent mapEvent) {
            switch (mapEvent) {
                case Map.MapEvent.BuildingPlaced:
                    RagePoints += buildingRagePoints;
                    break;
                case Map.MapEvent.ForestDestroyed:
                    RagePoints += forestDestroyRagePoints;
                    break;
                case Map.MapEvent.PlacingWolf:
                    break;
                case Map.MapEvent.RegrowTree:
                    break;
            }
        }

        public void IncreaseRageBarForTowerDeath() {
            if (ResourceManager.instance.CurrentState == ResourceManager.State.nature) {
                RagePoints += towerDestroyRagePoints;
            }
        }

        public void AddListener(UnityAction<float> updateButtonsBasedOnRagePoints) {
            slider.onValueChanged.AddListener(updateButtonsBasedOnRagePoints);
        }
    }
}
