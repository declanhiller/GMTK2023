using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class RageBuildingMenuEnabler : MonoBehaviour {

        [SerializeField] private GameObject[] thingsToEnable;
        
        private void Start() {
            ResourceManager.OnStateChange += state => {
                foreach (GameObject obj in thingsToEnable) {
                    obj.SetActive(true);
                    obj.GetComponent<Image>().raycastTarget = true;
                }
            };
        }
    }
}
