using System;
using UnityEngine;

namespace MapScripts {
    public class Map : MonoBehaviour {

        [SerializeField] private int width;
        [SerializeField] private int height;

        [SerializeField] private Vector2Int startTile;
        
        private Grid grid;

        private void Start() {
            grid = GetComponent<Grid>();
            grid.GetCellCenterWorld((Vector3Int) startTile);
            
            
        }
        
        

    }
}
