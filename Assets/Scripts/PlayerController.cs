using System;
using System.Collections;
using System.Collections.Generic;
using MapScripts;
using UnityEngine;
using UnityEngine.InputSystem;


// Simple player controller just for testing... will probably need to be refactored... maybe
public class PlayerController : MonoBehaviour {

    public Keybinds Keybinds { get; private set; }
    public Vector2 _mousePosition { get; private set; }

    private Camera _mainCamera;

    [SerializeField] private Map map;

    [SerializeField] private float sensitivity;

    private Coroutine _dragCoroutine;

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
    }

    public void OnClick(InputAction.CallbackContext context) {
        _mousePosition = _mainCamera.ScreenToWorldPoint(
            Keybinds.Player.MousePosition.ReadValue<Vector2>());
        map.ClickCell(_mousePosition);
    }

    // Update is called once per frame
    void Update() {
        Hover();
    }

    private void Hover() {
        _mousePosition = _mainCamera.ScreenToWorldPoint(
            Keybinds.Player.MousePosition.ReadValue<Vector2>());
        map.LightUpCell(_mousePosition);
    }

    private void StartDrag(InputAction.CallbackContext context) {
        Debug.Log("Start Drag");
        _dragCoroutine = StartCoroutine(Drag());
    }

    private void EndDrag(InputAction.CallbackContext context) {
        Debug.Log("End Drag");
        StopCoroutine(_dragCoroutine);
    }

    IEnumerator Drag() {
        Vector2 startMousePosition = _mousePosition;
        Vector3 startCameraPosition = _mainCamera.transform.position;
        while (true) {

            Debug.Log("Dragging");
            
            Vector2 deltaMousePosition = _mousePosition - startMousePosition;
            _mainCamera.transform.position = startCameraPosition - ((Vector3) deltaMousePosition * sensitivity);
            
            yield return new WaitForEndOfFrame();
        }
    }
}
