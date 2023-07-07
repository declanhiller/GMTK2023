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

    // Start is called before the first frame update
    void Start() {
        _keybinds = new Keybinds();
        _keybinds.Enable();
        _keybinds.Player.Click.performed += OnClick;
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context) {
        Vector2 readValue = _keybinds.Player.MousePosition.ReadValue<Vector2>();
        Debug.Log(_mousePosition);
    }

    // Update is called once per frame
    void Update() {
        Hover();
    }

    private void Hover() {
        _mousePosition = _keybinds.Player.MousePosition.ReadValue<Vector2>();
        _mousePosition = _mainCamera.ScreenToWorldPoint(_mousePosition);
        map.LightUpCell(_mousePosition);
    }
}
