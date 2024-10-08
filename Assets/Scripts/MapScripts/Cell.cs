using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Towers;

namespace MapScripts {
    public class Cell : MonoBehaviour {

        public Vector3Int cellPosition;
        public Map map;

        public int _startWoodAmount;

        public bool isSpecialTree;

        //example variables
        public bool isExcavated;
        public bool isOccupiedByBuilding;

        [SerializeField] private float clickDamage;

        public HealthAttribute unit;
        
        [FormerlySerializedAs("woodAmount")] public int woodInForest;

        public void onClick()
        {
            if (isExcavated) return;
            Excavation();
        }

        public void onclickHurt()
        {
            if(unit != null)
                unit.Health -= clickDamage;
        }

        public void Excavation() {
            Debug.Log("Excavate");
            if (!HasNeighborExcavatedCell()) return;
            woodInForest--;
            map.PlayMouseClickSound();
            float ratioOfWoodRemaining = (float) woodInForest / _startWoodAmount;
            map.ChangeTilesToMatchResourcesRemaining(this, ratioOfWoodRemaining);
            Debug.Log("Wood remaining in forest: " + woodInForest);
            if(woodInForest <= 0)
            {
                isExcavated = true;
                if (isSpecialTree) {
                    map.PlayTreeDeathSound();
                    ResourceManager.instance.CurrentState = ResourceManager.State.nature;
                }
                map.DestroyForest(cellPosition);
            }
            
            ResourceManager.instance.Wood++;
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
