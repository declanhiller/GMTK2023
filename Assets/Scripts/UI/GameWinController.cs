using System;
using UnityEngine;

namespace UI {
    public class GameWinController : MonoBehaviour {

        [SerializeField] private GameObject activateObject;
        
        private void Start() {
            ResourceManager.OnNatureWin += () => activateObject.SetActive(true);
        }
    }
}
