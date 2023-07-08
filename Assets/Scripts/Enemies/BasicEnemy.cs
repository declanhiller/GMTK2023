using System;
using System.Collections;
using Towers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Enemies {
    public class BasicEnemy : MonoBehaviour {
        
        public float speed;
        [FormerlySerializedAs("attackSpeed")] public float attackDuration;
        public float damage;

        [SerializeField] private LayerMask towerMask;

        private Track _track;


        private float _timer;

        private bool _currentlyAttacking;
        private HealthAttribute _currentAttackingTower;
        private float _attackTimer;
        

        private void Start() {
            _track = TrackLayout.Instance.GetRandomTrack();
            StartCoroutine(TowerCheck());
        }

        private IEnumerator TowerCheck() {
            while (true) {
                if (_currentlyAttacking) yield break;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, _track.Direction, 1f, towerMask);
                if (hit.transform != null) {
                    _currentlyAttacking = true;
                    _currentAttackingTower = hit.transform.GetComponent<HealthAttribute>();
                    _attackTimer = 0;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Update() {
            
            if (_currentlyAttacking) {
                if (_currentAttackingTower == null || _currentAttackingTower.Health <= 0) {
                    _currentlyAttacking = false;
                    return;
                }
                if (_attackTimer >= attackDuration) {
                    _currentAttackingTower.Health -= damage;
                    _attackTimer = 0;
                }
                
                _attackTimer += Time.deltaTime;
                return;
            }
            
            transform.Translate(speed * _track.Direction * Time.deltaTime);
            
        }
    }
}
