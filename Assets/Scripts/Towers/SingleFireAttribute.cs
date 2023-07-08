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
        [FormerlySerializedAs("projectile")] [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private CircleCollider2D collider;

        private List<BasicEnemy> _enemiesInRange;
        private bool _isFiring;

        private void Start() {
            _enemiesInRange = new List<BasicEnemy>();
            collider.radius = range;
        }


        private void Update() {
            if (_enemiesInRange.Count == 0) return;
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
            _isFiring = true;
            while (_enemiesInRange.Count != 0) {
                GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
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
                Vector3 bulletPosition = Vector3.Lerp(startPosition, target.transform.position, timer);
                bullet.transform.position = bulletPosition;
                timer += Time.deltaTime * actualSpeed;
                yield return new WaitForEndOfFrame();
            }

            BasicEnemy basicEnemy = target.GetComponent<BasicEnemy>();
            basicEnemy.Health -= damage;
            Destroy(bullet);
        }


    }
}
