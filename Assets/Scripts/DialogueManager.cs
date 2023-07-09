using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using MapScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class DialogueManager : MonoBehaviour {

        [SerializeField] private List<string> dialogues;
        [SerializeField] private Map map;

        [SerializeField] private GameObject dialogueBox;
        private TextMeshProUGUI _textBox;
        private Coroutine _currentUpdatingText;

        [SerializeField] private bool isCutScene;


        private void Start() {
            _textBox = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
            if (isCutScene)
            {
                StartCutScene();
                return;
            }
            BasicEnemy.OnEnemyDeath += UpdateDialogueForEnemyDeath;
            map.OnMapEvent += UpdateDialogueForMapEvent;
            dialogueBox.SetActive(false);
        }

        private void UpdateDialogueForEnemyDeath(BasicEnemy obj) {
            UpdateDialogue();
        }

        private void UpdateDialogueForMapEvent(Map.MapEvent mapEvent) {
            UpdateDialogue();
        }



        void UpdateDialogue() {
            if (dialogues.Count == 0) return;
            if(_currentUpdatingText != null) StopCoroutine(_currentUpdatingText);
            string dialogueString = dialogues[0];
            dialogues.Remove(dialogueString);
            dialogueBox.SetActive(true);
            _textBox.text = dialogueString;
            _currentUpdatingText = StartCoroutine(UpdateText());
        }

        IEnumerator UpdateText() {
            yield return new WaitForSeconds(3f);
            dialogueBox.SetActive(false);
        }

        IEnumerator EndStartScene()
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void StartCutScene()
        {
            dialogueBox.SetActive(true);
            string dialogueString = dialogues[0];
            _textBox.text = dialogueString;
            _currentUpdatingText = StartCoroutine(EndStartScene());

        }

    }
}
