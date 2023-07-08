using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WeatherEvents {
    public class WeatherEventDataPasser : MonoBehaviour {
         public WeatherButtonsController.WeatherEvent weatherEvent;
         public Button button;

         private void Start() {
             button = GetComponent<Button>();
         }

    }
}
