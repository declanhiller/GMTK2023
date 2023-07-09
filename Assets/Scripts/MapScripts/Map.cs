using System;
using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using Enemies;

namespace MapScripts {
    public class Map : MonoBehaviour {
        

        [SerializeField] private TileBase hoverTile;

        [SerializeField] private TileBase buildingTile;

        [SerializeField] private AnimatedTile animatedSawBuildingTile;

        [SerializeField] private TileBase buildingErrorTile;
        [SerializeField] private TileBase buildingOkTile;

        [SerializeField] private TileBase waypoint;

        [SerializeField] private TileBase treeTile;
        [SerializeField] private TileBase aliveForestTile;
        [SerializeField] private TileBase semiDeadForestTile;
        [SerializeField] private TileBase deadForestTile;
        
        public List<Cell> _cells;

        [SerializeField] private GameObject _cell;

        private Vector3Int _currentHoveringOverCellPosition;
        
        public event Action<MapEvent> OnMapEvent; //probably use for the rage counter

        private Grid _grid;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _hoverTilemap;
        [SerializeField] private Tilemap _forestTilemap;
        [SerializeField] public Tilemap buildingHoverTilemap;

        private ResourceManager _resourceManager;
        [SerializeField] public float woodRequirementForBasicBuilding; //eventually move this so it's in a SO

        [SerializeField] public GameObject basicTowerPrefab;

        [SerializeField] private GameObject mouseClickSound;
        [SerializeField] private GameObject treeDeathSound;

        public void PlayMouseClickSound()
        {
            mouseClickSound.GetComponent<AudioSource>().Play();
        }
        public void PlayTreeDeathSound()
        {
            treeDeathSound.GetComponent<AudioSource>().Play();
        }

        private void Start() {

            _resourceManager = ResourceManager.instance;
            
            _grid = GetComponent<Grid>();
            
            _cells = new List<Cell>();

            for (int i = _tilemap.cellBounds.xMin; i < _tilemap.cellBounds.xMax; i++) {
                for (int j = _tilemap.cellBounds.yMin; j < _tilemap.cellBounds.yMax; j++) {
                    Vector3Int localPos = new Vector3Int(i, j, (int) _tilemap.transform.position.y);
                    if (_tilemap.HasTile(localPos)) {
                        Cell cell = Instantiate(_cell).GetComponent<Cell>();
                        cell.cellPosition = localPos;
                        cell.transform.position = _grid.GetCellCenterWorld(localPos);

                        cell.isExcavated = !_forestTilemap.HasTile(localPos);


                        if (_forestTilemap.GetTile(localPos) == waypoint) {
                            cell.isSpecialTree = true;
                        }

                        int range = Random.Range(50, 100);
                        cell.woodInForest = range;
                        cell._startWoodAmount = range;

                        cell.map = this;
                        _cells.Add(cell);
                    }
                }
            }
            
        }

        public bool isValidSpotToBuild; //this is so spaghettied

        public void LightUpCell(Vector3 position, bool isBuildingLightUp) {
            if (isBuildingLightUp) {
                Vector3Int gridPosition = _grid.WorldToCell(position);
                if (_currentHoveringOverCellPosition == gridPosition) return;

                buildingHoverTilemap.SetTile(_currentHoveringOverCellPosition, null);
                _hoverTilemap.SetTile(_currentHoveringOverCellPosition, null);

            
                _currentHoveringOverCellPosition = gridPosition;

            
                if (!_tilemap.HasTile(gridPosition)) {
                    return;
                }

                Cell cell = _cells.First((c) => c.cellPosition.Equals(gridPosition));

                if (!cell.isExcavated || cell.isOccupiedByBuilding) {
                    buildingHoverTilemap.SetTile(gridPosition, buildingErrorTile);
                    isValidSpotToBuild = false;
                }
                else {
                    _hoverTilemap.SetTile(gridPosition, buildingOkTile);
                    isValidSpotToBuild = true;
                }

            }
            else {
                Vector3Int gridPosition = _grid.WorldToCell(position);
                if (_currentHoveringOverCellPosition == gridPosition) return;

                _hoverTilemap.SetTile(_currentHoveringOverCellPosition, null);

            
                _currentHoveringOverCellPosition = gridPosition;

            
                if (!_tilemap.HasTile(gridPosition)) {
                    return;
                }
                _hoverTilemap.SetTile(gridPosition, hoverTile);
            }
        }

        public void ClickCell(Vector3 position)
        {
            if (HasCell(position, out Cell currentCell))
            {
                switch (ResourceManager.instance.CurrentState)
                {
                    case ResourceManager.State.human:
                        currentCell?.onClick();
                        break;
                    case ResourceManager.State.nature:
                        currentCell?.onclickHurt();
                        break;
                }
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
            OnMapEvent?.Invoke(MapEvent.ForestDestroyed);
            _forestTilemap.SetTile(cellPosition, null);
        }

        public void RegrowForest(Vector3Int cellPosition)
        {
            OnMapEvent?.Invoke(MapEvent.RegrowTree);
            _forestTilemap.SetTile(cellPosition, treeTile);
        }

        public void PlaceUnitInCell(Cell cell, GameObject placingUnit) {
            cell.isOccupiedByBuilding = true;
            // _forestTilemap.SetTile(cell.cellPosition, buildingTile);
            Instantiate(placingUnit, cell.transform.position, Quaternion.identity, cell.transform);
            if(ResourceManager.instance.CurrentState == ResourceManager.State.nature)
            {
                OnMapEvent?.Invoke(MapEvent.PlacingWolf);
            } else OnMapEvent?.Invoke(MapEvent.BuildingPlaced);
        }
        
        public void PlaceBuildingInCell(Cell cell) {
            cell.isOccupiedByBuilding = true;
            _forestTilemap.SetTile(cell.cellPosition, animatedSawBuildingTile);
            GameObject basicTower = Instantiate(basicTowerPrefab, cell.transform.position, Quaternion.identity, cell.transform);
            ResourceManager.instance.Wood -= BuildingDragger.Instance.requiredAmountOfWood;
            HealthAttribute tower = basicTower.GetComponent<HealthAttribute>();
            tower.cell = cell;
            tower.map = this;
            cell.unit = tower;
            OnMapEvent?.Invoke(MapEvent.BuildingPlaced);
            
        }

        public enum MapEvent {
            BuildingPlaced, ForestDestroyed, PlacingWolf, RegrowTree
        }


        public void ChangeTilesToMatchResourcesRemaining(Cell cell, float ratio) {
            Debug.Log(ratio);
            if (ratio > 0.85f) {
                _tilemap.SetTile(cell.cellPosition, aliveForestTile);
            } else if (ratio > 0.6f) {
                _tilemap.SetTile(cell.cellPosition, semiDeadForestTile);
            }
            else {
                _tilemap.SetTile(cell.cellPosition, deadForestTile);
            }
        }

        public void RemoveBuilding(Cell cell) {
            _forestTilemap.SetTile(cell.cellPosition, null);
        }
    }
}
