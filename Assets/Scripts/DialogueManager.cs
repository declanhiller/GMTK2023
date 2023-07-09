using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using MapScripts;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
    public class DialogueManager : MonoBehaviour {

        [SerializeField] private List<string> colonialDialogues;
        [SerializeField] private List<string> natureDialogues;
        [SerializeField] private string startSceneDialogue;
        [SerializeField] private Map map;

        [SerializeField] private Image image;
        
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI colonialTextbox;
        [SerializeField] private bool isCutScene;
        private Coroutine _currentUpdatingText;
        private int _counterTillNextEvent;
        [SerializeField] private Sprite rageSprite;


        private void Start() {
            if (isCutScene)
            {
                colonialTextbox = dialogueBox.GetComponent<TextMeshProUGUI>();
                LoadCutScene();
                return;
            }
            BasicEnemy.OnEnemyDeath += UpdateDialogueForEnemyDeath;
            map.OnMapEvent += UpdateDialogueForMapEvent;
            colonialTextbox = colonialTextbox;
            ResourceManager.OnStateChange += (c) => { image.sprite = rageSprite; };
            HealthAttribute.OnZeroHealth += UpdateDialogueForNature;
            dialogueBox.SetActive(false);
            GenerateNewCounter();
        }

        private void UpdateDialogueForNature() {
            UpdateNatureDialogue();
        }

        private void GenerateNewCounter() {
            _counterTillNextEvent = Random.Range(3, 5);
        }

        private void UpdateDialogueForEnemyDeath(BasicEnemy obj) {
            if (ResourceManager.instance._gameHasEnded) return;
            if (ResourceManager.instance.CurrentState == ResourceManager.State.human) {
                UpdateColonialDialogue();
            }
        }

        private void UpdateDialogueForMapEvent(Map.MapEvent mapEvent) {
            if (ResourceManager.instance._gameHasEnded) return;
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
            colonialTextbox.text = dialogueString;
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
            colonialTextbox.text = dialogueString;
            GenerateNewCounter();
            _currentUpdatingText = StartCoroutine(UpdateText());
        }

        void LoadCutScene()
        {
            colonialTextbox.text = startSceneDialogue;
            StartCoroutine(EndCutScene());
        }

        IEnumerator UpdateText() {
            yield return new WaitForSeconds(7f);
            dialogueBox.SetActive(false);
        }

        IEnumerator EndCutScene()
        {
            yield return new WaitForSeconds(3f);
            dialogueBox.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}
