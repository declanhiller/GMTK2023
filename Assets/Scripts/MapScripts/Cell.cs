using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MapScripts {
    public class Cell : MonoBehaviour {
        public Vector3Int cellPosition;
        public Map map;

        //example variables
        public bool isExcavated;
        public bool isOccupiedByBuilding;

        private int foodAmount;
        public int woodAmount;

        public void onClick()
        {
            if (isExcavated) return;
            Excavation();
        }

        public void Excavation() {
            Debug.Log("Excavate");
            if (!HasNeighborExcavatedCell()) return;
            woodAmount--;
            Debug.Log("Wood remaining in forest: " + woodAmount);
            if(woodAmount <= 0)
            {
                isExcavated = true;
                map.DestroyForest(cellPosition);
            }
            
            ResourceManager.instance.WoodChange(1);
        }

        public bool HasNeighborExcavatedCell()
        {

            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if(Math.Abs(i + j) != 1) continue;
                    Vector3Int neighborCellPosition = new Vector3Int(i, j, 0) + this.cellPosition;
                    Cell[] neighborCell = map._cells.Where(c => c.cellPosition.Equals(neighborCellPosition)).ToArray();
                    if(neighborCell.Length == 0) continue;
                    if (neighborCell[0].isExcavated)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
