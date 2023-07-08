
using System;
using UnityEngine;
using UnityEngine.Serialization;
using WeatherEvents;

namespace Towers {
    public class HealthAttribute : MonoBehaviour {
        [SerializeField] private float health;
        private RageBar rage;

        public event Action OnZeroHealth;
        public static int towersCount;

        public HealthAttribute(float health) {
            this.health = health;
        }

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
                    OnZeroHealth?.Invoke();
                    Destroy(gameObject, 0.05f);
                    towersCount--;
                    Debug.Log("Remain tower:" + towersCount);
                }
            } 
        }

        
    }
}
