using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapScripts {
    public class Map : MonoBehaviour {

        [SerializeField] private int width;
        [SerializeField] private int height;

        [SerializeField] private Vector2Int startTile;

        [SerializeField] private TileBase hoverTile;

        private List<Cell> _cell;

        private Vector3Int _currentHoveringOverCellPosition;

        private Grid _grid;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _hoverTilemap;

        private void Start() {
            _grid = GetComponent<Grid>();
            _grid.GetCellCenterWorld((Vector3Int) startTile);

            // for (int i = _tilemap.cellBounds.xMin; i < _tilemap.cellBounds.xMax; i++) {
            //     for (int j = _tilemap.cellBounds.xMin; j < _tilemap.cellBounds.yMax; j++) {
            //         Vector3Int localPos = new Vector3Int(i, j, (int) _tilemap.transform.position.y);
            //         if (_tilemap.HasTile(localPos)) {
            //             
            //         }
            //     }
            // }
            
        }

        public void LightUpCell(Vector3 position) {
            Vector3Int gridPosition = _grid.WorldToCell(position);
            if (_currentHoveringOverCellPosition == gridPosition) return;

            _hoverTilemap.SetTile(_currentHoveringOverCellPosition, null);

            
            _currentHoveringOverCellPosition = gridPosition;

            
            if (!_tilemap.HasTile(gridPosition)) {
                Debug.Log("Hello");
                return;
            }
            _hoverTilemap.SetTile(gridPosition, hoverTile);
        }

        public void GetCell(Vector3 position) {
            Vector3Int worldToCell = _grid.WorldToCell(position);
        }

    }
}
