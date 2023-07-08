using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies {
    public class EnemySpawner : MonoBehaviour {

        [SerializeField] private GameObject enemyPrefab;
        private bool _isAiControlled = true;
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;

        private void Start() {
            StartCoroutine(RandomTick());
        }

        private IEnumerator RandomTick() {
            while (_isAiControlled) {
                float seconds = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(seconds);
                Spawn(TrackLayout.Instance.GetRandomTrack());
            }
        }

        private void Spawn(Track track) {
            Debug.Log("Spawn Enemy");
            GameObject enemyObj = Instantiate(enemyPrefab, track.StartPosition, Quaternion.identity);
            BasicEnemy enemy = enemyObj.GetComponent<BasicEnemy>();
            enemy.track = track;
        }

    }
}
