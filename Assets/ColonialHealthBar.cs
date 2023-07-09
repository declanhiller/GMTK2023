using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonialHealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    void Start() {
        RectTransform rectTransform = GetComponent<RectTransform>();
        ResourceManager.OnHealthChange += (health) => {
            rectTransform.localScale =
                new Vector3(health / ResourceManager.instance.startingMaxHealth, 1);
        };
    }
}
