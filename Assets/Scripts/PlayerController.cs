using System.Collections;
using System.Collections.Generic;
using MapScripts;
using UnityEngine;
using UnityEngine.InputSystem;


// Simple player controller just for testing... will probably need to be refactored... maybe
public class PlayerController : MonoBehaviour {

    private Keybinds _keybinds;
    private Vector2 _mousePosition;

    private Camera _mainCamera;

    [SerializeField] private Map map;

    [SerializeField] private float sensitivity;

    private Coroutine _dragCoroutine;

    // Start is called before the first frame update
    void Start() {
        _keybinds = new Keybinds();
        _keybinds.Enable();
        _keybinds.Player.Click.performed += OnClick;

        _keybinds.Player.RightClick.started += StartDrag;
        _keybinds.Player.RightClick.canceled += EndDrag;
        
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context) {
        Debug.Log(_mousePosition);
    }

    // Update is called once per frame
    void Update() {
        Hover();
    }

    private void Hover() {
        _mousePosition = _mainCamera.ScreenToWorldPoint(
            _keybinds.Player.MousePosition.ReadValue<Vector2>());
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
