using System.Collections;
using System.Collections.Generic;
using MapScripts;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private Map map;
    [SerializeField] private PlayerController player;
    

    public bool isDragging;
    [SerializeField] private GameObject towerDraggingPrefab;
    private GameObject _draggingObject;
    
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
        _draggingObject = Instantiate(towerDraggingPrefab);
        _draggingObject.transform.position = player._mousePosition;
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        Destroy(_draggingObject);
        isDragging = false;
        map.buildingHoverTilemap.ClearAllTiles();
        if (map.isValidSpotToBuild && map.HasCell(player._mousePosition, out Cell cell)) {
            player.PlaceUnit();
        }
        
    }
}
