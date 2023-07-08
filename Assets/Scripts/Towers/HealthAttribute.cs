
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Towers {
    public class HealthAttribute : MonoBehaviour {
        [SerializeField] private float health;

        public event Action OnZeroHealth;

        public HealthAttribute(float health) {
            this.health = health;
        }

        public float Health {
            get => health;
            set {
                if(value <= 0) {
                    OnZeroHealth?.Invoke();
                    Destroy(gameObject, 0.05f);
                }
            } 
        }

        
    }
}
