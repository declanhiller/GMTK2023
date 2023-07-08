using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies {
    [Serializable]
    public class Track {
        [SerializeField] private Transform startTransform;
        [SerializeField] private Transform endTransform;

        public Vector3 StartPosition => startTransform.position;
        public Vector3 EndPosition => endTransform.position;

        public Vector3 Direction => (EndPosition - StartPosition).normalized;


        public float Length => Vector3.Distance(startTransform.position, endTransform.position);
    }
}
