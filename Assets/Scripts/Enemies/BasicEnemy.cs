using System;
using System.Collections;
using Towers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Enemies {
    public class BasicEnemy : MonoBehaviour {

        public static event Action<BasicEnemy> OnEnemyDeath;
        
        public float speed;
        [FormerlySerializedAs("attackSpeed")] public float attackDuration;
        public float damage;

        [SerializeField] private LayerMask towerMask;

        [SerializeField] private float maxHealth;

        public float Health {
            get => maxHealth;
            set {
                maxHealth = value;
                if (value <= 0) {
                    OnEnemyDeath?.Invoke(this);
                    Destroy(gameObject);
                }
            }
        }

        [NonSerialized] public Track track;


        private float _timer;

        private bool _currentlyAttacking;
        private HealthAttribute _currentAttackingTower;
        private float _attackTimer;
        

        private void Start() {
            StartCoroutine(TowerCheck());
        }

        private IEnumerator TowerCheck() {
            while (true) {
                if (_currentlyAttacking) yield break;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, track.Direction, 0.4f, towerMask);
                if (hit.transform != null) {
                    _currentlyAttacking = true;
                    _currentAttackingTower = hit.transform.GetComponent<HealthAttribute>();
                    _attackTimer = attackDuration; //so the enemy attacks when immediatley geting to a tower
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Update() {
            
            if (_currentlyAttacking) {
                if (_currentAttackingTower == null || _currentAttackingTower.Health <= 0) {
                    Debug.Log("Tower doesn't exist anymore, proceed");
                    _currentlyAttacking = false;
                    return;
                }
                if (_attackTimer >= attackDuration) {
                    Debug.Log("Deal Damage");
                    _currentAttackingTower.Health -= damage;
                    _attackTimer = 0;
                }
                ;
                _attackTimer += Time.deltaTime;
                return;
            }
            
            transform.Translate(speed * track.Direction * Time.deltaTime);
            
        }
    }
}
