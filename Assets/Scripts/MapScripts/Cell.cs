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
        private bool occupied;

        private int foodAmount;
        [SerializeField]private int woodAmount;
        private int peopleAmount;
        private int forestCount;

        private static List<Vector3Int> neighbors = new List<Vector3Int> { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };


        public void onClick()
        {
            Debug.Log(cellPosition);
            if (isExcavated)
            {

            }
            Excavation();
        }

        public void Excavation()
        {
            if (!HasNeighborExcavatedCell() || isExcavated)
            { 
                return;
            }
            if(forestCount <= 0)
            {
                isExcavated = true;
            }
            WoodExploration();
            forestCount--;
            Debug.Log(forestCount);
        }

        public bool Excavated()
        {
            return isExcavated;
        }
        public void WoodExploration()
        {
            ResourceManager.instance.WoodChange(woodAmount);
        }

        public bool HasNeighborExcavatedCell()
        {
            foreach (Vector3Int offset in neighbors)
            {
                Cell neighborCell = map._cells.Where((c) => c.cellPosition.Equals(cellPosition + offset)).First();
                    if (neighborCell.isExcavated)
                    {
                        return true;
                    }
            }
            return false;
        }
    }
}
