using System;
using UnityEngine;

namespace MapScripts {
    public class Map : MonoBehaviour {

        [SerializeField] private int width;
        [SerializeField] private int height;
        
        private Node[] _node;

        private void Start() {
            GenerateNodes();
        }

        private void GenerateNodes() {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    GameObject node = new GameObject("Node [" + i + ", " + j + "]");
                    node.AddComponent<Node>();
                }
            }
        }

    }
}
