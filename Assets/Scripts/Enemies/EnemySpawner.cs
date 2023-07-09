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

        private bool _isActive = true;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
        private void Start() {
            SpawnPattern(ResourceManager.instance.CurrentState);
            ResourceManager.OnColonialLose += () => _isActive = false;
        }

        private IEnumerator RandomTick() {
            if (!_isActive) yield break;
            while (_isAiControlled) {
                float seconds = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(seconds);
                AISpawn(TrackLayout.Instance.GetRandomTrack());
            }
        }

        private void AISpawn(Track track) {
            if (!_isActive) return;
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
