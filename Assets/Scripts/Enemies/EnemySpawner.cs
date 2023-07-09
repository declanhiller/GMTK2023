using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies {
    public class EnemySpawner : MonoBehaviour {

        public EnemySpawner instance { get; private set; }

        

        [SerializeField] private GameObject enemyPrefab;
        private bool _isAiControlled = true;
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;
        [SerializeField] private float spawnRate;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
        private void Start() {
            SpawnPattern(ResourceManager.instance.CurrentState);
        }

        private IEnumerator RandomTick() {
            while (_isAiControlled) {
                minTime = Mathf.Min(maxTime, minTime *= spawnRate);
                yield return new WaitForSeconds(minTime);
                AISpawn(TrackLayout.Instance.GetRandomTrack());
            }
        }

        private void AISpawn(Track track) {
            Debug.Log("Spawn Enemy");
            GameObject enemyObj = Instantiate(enemyPrefab, track.StartPosition, Quaternion.identity);
            BasicEnemy enemy = enemyObj.GetComponent<BasicEnemy>();
            enemy.track = track;
        }

        private void SpawnPattern(ResourceManager.State state)
        {
            switch (state)
            {
                case ResourceManager.State.human:
                    StartCoroutine(RandomTick());
                    break;
                case ResourceManager.State.nature:
                    StopCoroutine(RandomTick());
                    break;
            }
        }

    }
}
