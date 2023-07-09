
using System;
using MapScripts;
using UnityEngine;
using UnityEngine.Serialization;
using WeatherEvents;

namespace Towers {
    public class HealthAttribute : MonoBehaviour {
        [SerializeField] private float health;
        private RageBar rage;
        public Cell cell;
        public Map map;

        public event Action OnZeroHealth;
        public static int towersCount;

        private void Awake()
        {
            towersCount++;
            rage = GameObject.Find("Rage Bar").GetComponent<RageBar>();
        }

        private void Start()
        {
            OnZeroHealth += rage.IncreaseRageBarForTowerDeath;
        }

        public float Health {
            get => health;
            set {
                health = value;
                if(value <= 0) {
                    map.RemoveBuilding(cell);
                    OnZeroHealth?.Invoke();
                    Destroy(gameObject, 0.05f);
                    towersCount--;
                    Debug.Log("Remain tower:" + towersCount);
                }
            } 
        }

        
    }
}
