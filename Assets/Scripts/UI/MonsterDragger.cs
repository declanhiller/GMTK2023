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
using WeatherEvents;

namespace UI {
    public class MonsterDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        [SerializeField] private Map map;
        [SerializeField] private PlayerController player;
    
        public static MonsterDragger Instance { get; private set; }
        
        public bool isDragging;
        [FormerlySerializedAs("towerDraggingPrefab")] [SerializeField] private GameObject draggingPrefab;
        private GameObject _draggingObject;

        [SerializeField] private Image icon;

        [SerializeField] public float price;

        [SerializeField] private float cooldown;
        [SerializeField] private TextMeshProUGUI displayedTimer;

        [SerializeField] private Color selectedColor;

        private bool _isOnCooldown;

        private void Awake() {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void Update() {
            if (!isDragging) return;
            _draggingObject.transform.position = player._mousePosition;
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (_isOnCooldown) return;
            if (RageBar.Instance.RagePoints < price) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            _draggingObject = Instantiate(draggingPrefab);
            _draggingObject.transform.position = player._mousePosition;
            isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            if(_isOnCooldown) return;
            if (RageBar.Instance.RagePoints < price) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            Destroy(_draggingObject);
            isDragging = false;
            if (map.HasCell(player._mousePosition, out Cell cell)) {
                if (cell.isExcavated && !cell.isOccupiedByBuilding) {
                    player.PlaceUnit();
                    RageBar.Instance.RagePoints -= price;
                    StartCoroutine(Cooldown());
                    _isOnCooldown = true;
                }
            }
        
        }

        IEnumerator Cooldown() {

            float timer = cooldown;
            while (timer > 0) {


                int displayedAsTime = (int) timer + 1;
                displayedTimer.text = displayedAsTime.ToString();
                timer -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            displayedTimer.text = "";
            _isOnCooldown = false;
        }

    }
}
