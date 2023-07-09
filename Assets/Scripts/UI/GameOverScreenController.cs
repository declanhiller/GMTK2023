using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class GameOverScreenController : MonoBehaviour {

        public GameObject _gameOverObject;
        
        private void Start() {
            ResourceManager.OnColonialLose += OnLose;
            _gameOverObject.SetActive(false);
        }

        private void OnLose() {
            _gameOverObject.SetActive(true);
        }

        public void GoToStart() {
            SceneManager.LoadScene("StartScreen");
        }
    }
}
