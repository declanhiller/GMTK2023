using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using MapScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace {
    public class DialogueManager : MonoBehaviour {

        [SerializeField] private List<string> dialogues;
        [SerializeField] private Map map;

        [SerializeField] private GameObject dialogueBox;
        private TextMeshProUGUI _textBox;
        private Coroutine _currentUpdatingText;


        private void Start() {
            BasicEnemy.OnEnemyDeath += UpdateDialogueForEnemyDeath;
            map.OnMapEvent += UpdateDialogueForMapEvent;
            _textBox = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
            dialogueBox.SetActive(false);
        }

        private void UpdateDialogueForEnemyDeath(BasicEnemy obj) {
            UpdateDialogue();
        }

        private void UpdateDialogueForMapEvent(Map.MapEvent mapEvent) {
            UpdateDialogue();
        }



        void UpdateDialogue() {
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

    }
}
