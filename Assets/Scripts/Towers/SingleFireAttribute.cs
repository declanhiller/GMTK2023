using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Towers {
    public class SingleFireAttribute : MonoBehaviour {
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float fireRate;
        [SerializeField] private float range;
        [SerializeField] private float damage;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private BoxCollider2D collider;

        private List<BasicEnemy> _enemiesInRange;
        private bool _isFiring;

        [SerializeField] private AudioSource fireSound;

        private void Start() {
            _enemiesInRange = new List<BasicEnemy>();
            float width = range * 1.5f;
            float height = range;
            collider.size = new Vector2(width, height);
            Debug.Log(collider.size);
            RefreshNewEnemies();

            BasicEnemy.OnEnemyDeath += RemoveEnemyFromList;

        }

        void RefreshNewEnemies() {
            Collider2D[] overlapBoxAll = Physics2D.OverlapBoxAll((Vector2) transform.position + collider.offset, collider.size, 0,
                _enemyLayerMask);
            foreach (Collider2D collider in overlapBoxAll) {
                _enemiesInRange.Add(collider.GetComponent<BasicEnemy>());
            }
        }

        private void RemoveEnemyFromList(BasicEnemy obj) {
            _enemiesInRange.Remove(obj);
        }


        private void Update() {
            if (_enemiesInRange.Count == 0) {
                RefreshNewEnemies();
                if (_enemiesInRange.Count == 0) return;
            };
            if (_isFiring) return;
            StartCoroutine(Fire());
        }

        private void OnTriggerEnter2D(Collider2D other) {
            _enemiesInRange.Add(other.GetComponent<BasicEnemy>());
        }

        private void OnTriggerExit2D(Collider2D other) {
            _enemiesInRange.Remove(other.GetComponent<BasicEnemy>());
        }

        private IEnumerator Fire() {
             fireSound.Play();
            _isFiring = true;
            while (_enemiesInRange.Count != 0) {
                GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                if(_enemiesInRange[0] == null) {
                    _enemiesInRange.RemoveAt(0);
                }
                StartCoroutine(BulletTravel(bullet, _enemiesInRange[0].transform));
                yield return new WaitForSeconds(fireRate);
            }

            _isFiring = false;
        }
        

        private IEnumerator BulletTravel(GameObject bullet, Transform target) {
            Vector3 startPosition = bullet.transform.position;
            float actualSpeed = bulletSpeed / Vector3.Distance(bullet.transform.position,
                target.transform.position);
            float timer = 0;
            while (timer <= 1) {
                if (target == null) {
                    Destroy(bullet);
                    yield break;
                }
                Vector3 bulletPosition = Vector3.Lerp(startPosition, target.transform.position, timer);
                bullet.transform.position = bulletPosition;
                timer += Time.deltaTime * actualSpeed;
                yield return new WaitForEndOfFrame();
            }

            if (target != null) {
                BasicEnemy basicEnemy = target.GetComponent<BasicEnemy>();
                basicEnemy.Health -= damage;
            }
            Destroy(bullet);
        }


    }
}
