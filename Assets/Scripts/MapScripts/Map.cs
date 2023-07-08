using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace MapScripts {
    public class Map : MonoBehaviour {
        

        [SerializeField] private TileBase hoverTile;

        public List<Cell> _cells;

        [SerializeField] private GameObject _cell;

        private Vector3Int _currentHoveringOverCellPosition;

        private Grid _grid;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _hoverTilemap;
        [SerializeField] private Tilemap _forestTilemap;

        private void Start() {
            _grid = GetComponent<Grid>();
            
            _cells = new List<Cell>();

            for (int i = _tilemap.cellBounds.xMin; i < _tilemap.cellBounds.xMax; i++) {
                for (int j = _tilemap.cellBounds.yMin; j < _tilemap.cellBounds.yMax; j++) {
                    Vector3Int localPos = new Vector3Int(i, j, (int) _tilemap.transform.position.y);
                    if (_tilemap.HasTile(localPos)) {
                        Cell cell = Instantiate(_cell).GetComponent<Cell>();
                        cell.cellPosition = localPos;

                        cell.isExcavated = !_forestTilemap.HasTile(localPos);

                        cell.woodAmount = Random.Range(25, 100);
                        
                        cell.map = this;
                        _cells.Add(cell);
                    }
                }
            }
            
        }

        public void LightUpCell(Vector3 position) {
            Vector3Int gridPosition = _grid.WorldToCell(position);
            if (_currentHoveringOverCellPosition == gridPosition) return;

            _hoverTilemap.SetTile(_currentHoveringOverCellPosition, null);

            
            _currentHoveringOverCellPosition = gridPosition;

            
            if (!_tilemap.HasTile(gridPosition)) {
                return;
            }
            _hoverTilemap.SetTile(gridPosition, hoverTile);

        }

        public void ClickCell(Vector3 position)
        {
            if (HasCell(position, out Cell currentCell))
            {
                currentCell?.onClick();
            }
        }

        public bool HasCell(Vector3 position, out Cell cell) {
            Vector3Int gridPosition = _grid.WorldToCell(position);
            if (!_tilemap.HasTile(gridPosition)) {
                cell = null;
                return false;
            }

            Cell[] cells = _cells.Where((c) => c.cellPosition.Equals(gridPosition)).ToArray();
            if (cells.Length != 1) {
                throw new Exception("Lmao wtf, there is " + cells.Length + " at this position" +
                                    "when there should only be one");
            }

            cell = cells.First();
            return true;
        }

        public Tuple<Vector2, Vector2> FillOutBoundingPoints() {
            Bounds tilemapLocalBounds = _tilemap.localBounds;
            return new Tuple<Vector2, Vector2>(tilemapLocalBounds.min + _tilemap.transform.position,
                tilemapLocalBounds.max + _tilemap.transform.position);
        }

        public void DestroyForest(Vector3Int cellPosition) {
            _forestTilemap.SetTile(cellPosition, null);
        }
    }
}
