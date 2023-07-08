
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Towers {
    public class HealthAttribute : MonoBehaviour {
        [SerializeField] private float health;

        public event Action OnZeroHealth;
        public static int towersCount;

        public HealthAttribute(float health) {
            this.health = health;
        }

        private void Awake()
        {
            towersCount++;
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
