using System;
using System.Collections;
using MapScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Enemies;


// Simple player controller just for testing... will probably need to be refactored... maybe
namespace Player {
    public class PlayerController : MonoBehaviour {

        public Keybinds Keybinds { get; private set; }
        public Vector2 _mousePosition { get; private set; }

        private Camera _mainCamera;

        [SerializeField] private Map map;

        [SerializeField] private float sensitivity;

        [SerializeField] private GameObject towerPrefab;

        [SerializeField] private GameObject enemyPrefab;

        [SerializeField] private BuildingDragger dragger;


        private float spawnCoolDown;

        private Coroutine _dragCoroutine;

        private ResourceManager _resourceManager;

        private Vector3 _min;
        private Vector3 _max;
        
        private void Awake() {
            Keybinds = new Keybinds();
            Keybinds.Enable();
        }

        // Start is called before the first frame update
        void Start() {

            Keybinds.Player.Click.performed += OnClick;

            Keybinds.Player.RightClick.started += StartDrag;
            Keybinds.Player.RightClick.canceled += EndDrag;
            
            _mainCamera = Camera.main;

            Tuple<Vector2,Vector2> boundingPoints = map.FillOutBoundingPoints();
            _min = boundingPoints.Item1;
            _max = boundingPoints.Item2;

            _resourceManager = ResourceManager.instance;
        }

        public void OnClick(InputAction.CallbackContext context) {
            _mousePosition = _mainCamera.ScreenToWorldPoint(
                Keybinds.Player.MousePosition.ReadValue<Vector2>());
            map.ClickCell(_mousePosition);
        }

        // Update is called once per frame
        void Update() {
            Hover();
            spawnCoolDown -= Time.deltaTime;
        }

        private void Hover() {
            _mousePosition = _mainCamera.ScreenToWorldPoint(
                Keybinds.Player.MousePosition.ReadValue<Vector2>());
            if (dragger.isDragging) return;
            map.LightUpCell(_mousePosition, false);
        }

        public void PlaceUnit() {
            if (!map.HasCell(_mousePosition, out Cell cell)) return;
            if (!cell.isExcavated) return;
            if (cell.isOccupiedByBuilding) return;

            if(spawnCoolDown < 0)
            {
                switch (ResourceManager.instance.currentState)
                {
                    case ResourceManager.State.human:
                        map.PlaceUnitInCell(cell, towerPrefab);
                        spawnCoolDown = 0;
                        break;
                    case ResourceManager.State.nature:
                        map.PlaceUnitInCell(cell, enemyPrefab);
                        spawnCoolDown = enemyPrefab.GetComponent<BasicEnemy>().placeCoolDown;
                        break;
                }
            }
        }

        private void StartDrag(InputAction.CallbackContext context) {
            _dragCoroutine = StartCoroutine(Drag());
        }

        private void EndDrag(InputAction.CallbackContext context) {
            StopCoroutine(_dragCoroutine);
        }

        IEnumerator Drag() {
            Vector2 startMousePosition = _mousePosition;
            Vector2 previousFrameMousePosition = _mousePosition;
            while (true) {
            
                Vector2 deltaMousePosition = previousFrameMousePosition - startMousePosition;

                Vector3 proposedPosition = _mainCamera.transform.position - (Vector3) deltaMousePosition * sensitivity;

                proposedPosition = new Vector3(Mathf.Clamp(proposedPosition.x, _min.x, _max.x), 
                    Mathf.Clamp(proposedPosition.y, _min.y,_max.y), proposedPosition.z);

                _mainCamera.transform.position = proposedPosition;
            
                previousFrameMousePosition = _mousePosition;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
