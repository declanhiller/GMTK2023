using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies {
    public class EnemySpawner : MonoBehaviour {

        public EnemySpawner instance { get; private set; }

        

        [SerializeField] private GameObject enemyPrefab;
        private bool _isAiControlled = true;


        [SerializeField] private float startMinTime;
        [SerializeField] private float startMaxTime;

        [SerializeField] private float endMinTime;
        [SerializeField] private float endMaxTime;

        [SerializeField] private float timeToReachEnd;
        
        private float _minTime;
        private float _maxTime;
        
        private bool _isActive = true;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
        private void Start() {
            SpawnPattern(ResourceManager.instance.CurrentState);
            ResourceManager.OnColonialLose += () => _isActive = false;
            ResourceManager.OnStateChange += state => _isActive = false;
        }

        private IEnumerator RandomTick() {
            Debug.Log("Start Coroutine");
            while (_isAiControlled && _isActive) {
                float seconds = Random.Range(_minTime, _maxTime);
                Debug.Log(seconds);
                yield return new WaitForSeconds(seconds);
                AISpawn(TrackLayout.Instance.GetRandomTrack());
            }
        }

        private void AISpawn(Track track) {
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
                    StartCoroutine(IncreaseSpawningRate());
                    break;
                case ResourceManager.State.nature:
                    StopCoroutine(RandomTick());
                    break;
            }
        }

        private IEnumerator IncreaseSpawningRate() {
            float t = 0;
            while (t <= 1) {
                _minTime = Mathf.Lerp(startMinTime, endMinTime, t);
                _maxTime = Mathf.Lerp(startMaxTime, endMaxTime, t);
                t += Time.deltaTime / timeToReachEnd;
                yield return new WaitForEndOfFrame();
            }
        }

    }
}
