
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

        public static event Action OnZeroHealth;
        public static int towersCount;

        private void Awake()
        {
            towersCount++;
            rage = GameObject.Find("Rage Bar").GetComponent<RageBar>();
        }

        private void Start()
        {
            OnZeroHealth += rage.IncreaseRageBarForTowerDeath;
            ResourceManager.OnColonialLose += DestroyItself;
        }

        private void OnDestroy() {
            ResourceManager.OnColonialLose -= DestroyItself;
        }

        private void DestroyItself() {
            Destroy(gameObject);
        }

        public float Health {
            get => health;
            set {
                health = value;
                if(value <= 0) {
                    switch (ResourceManager.instance.CurrentState)
                    {
                        case ResourceManager.State.human:
                            map.RemoveBuilding(cell);
                            break;
                        case ResourceManager.State.nature:
                            map.RegrowForest(cell.cellPosition);
                            break;
                        default:
                            break;
                    }
                    
                    OnZeroHealth?.Invoke();
                    Destroy(gameObject, 0.05f);
                    towersCount--;
                    Debug.Log("Remain tower:" + towersCount);
                }
            } 
        }

        
    }
}
