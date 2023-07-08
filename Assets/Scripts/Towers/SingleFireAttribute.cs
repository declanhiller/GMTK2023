using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Towers {
    public class SingleFireAttribute : MonoBehaviour {
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float fireRate;
        [SerializeField] private float range;
        [SerializeField] private float damage;
        [SerializeField] private Vector3 firePoint;
        [FormerlySerializedAs("projectile")] [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private LayerMask _enemyLayerMask;

        private void Start() {
            StartCoroutine(CheckForFire());
        }

        private IEnumerator CheckForFire() {
            while (true) {
                Transform target;
                while (true) {
                    Collider2D overlapCircle = Physics2D.OverlapCircle(firePoint, range, _enemyLayerMask);
                    if (overlapCircle != null) {
                        target = overlapCircle.transform;
                        break;
                    }
                    yield return new WaitForSeconds(0.2f);
                }

                Fire(target);
                
                yield return new WaitForSeconds(fireRate);
            }
        }

        private void Fire(Transform target) {
            GameObject bullet = Instantiate(projectilePrefab, firePoint, Quaternion.identity);
            StartCoroutine(BulletTravel(bullet, target));
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
            Destroy(bullet);
        }


    }
}
