using System;
using System.Collections;
using MapScripts;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI {
    public class MonsterDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        [SerializeField] private Map map;
        [SerializeField] private PlayerController player;
    
        public static MonsterDragger Instance { get; private set; }
        
        public bool isDragging;
        [FormerlySerializedAs("towerDraggingPrefab")] [SerializeField] private GameObject draggingPrefab;
        private GameObject _draggingObject;

        [SerializeField] private Image icon;

        [SerializeField] private float price;

        [SerializeField] private float cooldown;
        [SerializeField] private TextMeshProUGUI displayedTimer;

        [SerializeField] private Color selectedColor;

        private bool _isSelected;
        

        private void Awake() {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void Update() {
            if (!isDragging) return;
            _draggingObject.transform.position = player._mousePosition;
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            _draggingObject = Instantiate(draggingPrefab);
            _draggingObject.transform.position = player._mousePosition;
            isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            Destroy(_draggingObject);
            isDragging = false;
            if (map.HasCell(player._mousePosition, out Cell cell)) {
                player.PlaceUnit();
            }
        
        }

        IEnumerator Cooldown() {

            float timer = 0;
            while (timer <= cooldown) {

                timer += Time.deltaTime;

                int displayedAsTime = (int) timer + 1;
                displayedTimer.text = displayedAsTime.ToString();
                
                yield return new WaitForEndOfFrame();
            }
        }

    }
}
