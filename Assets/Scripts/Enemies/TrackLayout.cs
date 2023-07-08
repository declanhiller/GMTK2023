using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies {
    public class TrackLayout : MonoBehaviour {
        
        public static TrackLayout Instance { get; private set; }
        
        [SerializeField] private List<Track> tracks;


        private void Start() {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }


        public Track GetRandomTrack() {
            return tracks[Random.Range(0, tracks.Count)];
        }
    }
}
