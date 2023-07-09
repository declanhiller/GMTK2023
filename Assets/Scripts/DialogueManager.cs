using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using MapScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
    public class DialogueManager : MonoBehaviour {

        [SerializeField] private List<string> colonialDialogues;
        [SerializeField] private List<string> natureDialogues;
        [SerializeField] private Map map;

        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI colonialTextbox;
        [SerializeField] private TextMeshProUGUI rageTextbox;
        private TextMeshProUGUI _textbox;
        private Coroutine _currentUpdatingText;
        private int _counterTillNextEvent;


        private void Start() {
            BasicEnemy.OnEnemyDeath += UpdateDialogueForEnemyDeath;
            map.OnMapEvent += UpdateDialogueForMapEvent;
            _textbox = colonialTextbox;
            ResourceManager.OnStateChange += (c) => { _textbox = rageTextbox; };
            dialogueBox.SetActive(false);
            GenerateNewCounter();
        }

        private void GenerateNewCounter() {
            _counterTillNextEvent = Random.Range(3, 5);
        }

        private void UpdateDialogueForEnemyDeath(BasicEnemy obj) {
            if (ResourceManager.instance.CurrentState == ResourceManager.State.human) {
                UpdateColonialDialogue();
            }
        }

        private void UpdateDialogueForMapEvent(Map.MapEvent mapEvent) {
            if (ResourceManager.instance.CurrentState == ResourceManager.State.human) {
                UpdateColonialDialogue();
            }
        }



        void UpdateColonialDialogue() {
            if (colonialDialogues.Count == 0) return;
            _counterTillNextEvent--;
            if (_counterTillNextEvent > 0) return;
            if(_currentUpdatingText != null) StopCoroutine(_currentUpdatingText);
            string dialogueString = colonialDialogues[0];
            colonialDialogues.Remove(dialogueString);
            dialogueBox.SetActive(true);
            _textbox.text = dialogueString;
            GenerateNewCounter();
            _currentUpdatingText = StartCoroutine(UpdateText());
        }
        
        void UpdateNatureDialogue() {
            if (natureDialogues.Count == 0) return;
            _counterTillNextEvent--;
            if (_counterTillNextEvent > 0) return;
            if(_currentUpdatingText != null) StopCoroutine(_currentUpdatingText);
            string dialogueString = natureDialogues[0];
            natureDialogues.Remove(dialogueString);
            dialogueBox.SetActive(true);
            _textbox.text = dialogueString;
            GenerateNewCounter();
            _currentUpdatingText = StartCoroutine(UpdateText());
        }

        IEnumerator UpdateText() {
            yield return new WaitForSeconds(3f);
            dialogueBox.SetActive(false);
        }

    }
}
