using TMPro;
using UnityEngine;

namespace UI {
    public class WoodCounter : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI woodText;
        private void Start() {
            ResourceManager.OnWoodChange += (wood) => woodText.text = wood.ToString();
        }
    }
}
