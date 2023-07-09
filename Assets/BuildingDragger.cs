using System;
using System.Collections;
using System.Collections.Generic;
using MapScripts;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private Map map;
    [SerializeField] private PlayerController player;
    
    public static BuildingDragger Instance { get; private set; }
    
    
    public bool isDragging;
    [SerializeField] private GameObject towerDraggingPrefab;
    private GameObject _draggingObject;

    [SerializeField] public int requiredAmountOfWood;

    [SerializeField] private Image icon;

    [SerializeField] private Color notReadyColor = new Color(255, 90, 50);

    private void Start() {
        ResourceManager.OnWoodChange += (wood) => { icon.color = wood < requiredAmountOfWood ? notReadyColor : Color.white; };
    }

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (!isDragging) return;
        Vector2 mousePosition = player._mousePosition;
        
        map.LightUpCell(mousePosition, true);
        Cell cell;
        if (map.HasCell(mousePosition, out cell)) {
            if (_draggingObject.activeInHierarchy) {
                _draggingObject.SetActive(false);
            }
        }
        else {
            if (!_draggingObject.activeInHierarchy) {
                _draggingObject.SetActive(true);
            }
            _draggingObject.transform.position = player._mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (ResourceManager.instance.Wood < requiredAmountOfWood) return;
        _draggingObject = Instantiate(towerDraggingPrefab);
        _draggingObject.transform.position = player._mousePosition;
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (ResourceManager.instance.Wood < requiredAmountOfWood) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        Destroy(_draggingObject);
        isDragging = false;
        map.buildingHoverTilemap.ClearAllTiles();
        Cell cell;
        if (map.isValidSpotToBuild && map.HasCell(player._mousePosition, out cell)) {
            player.PlaceUnit();
        }
        
    }
}
