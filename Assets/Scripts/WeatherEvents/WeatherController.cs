using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WeatherEvents {
    public class WeatherController : MonoBehaviour {

        //Connect to this event to get when a certain weather event happens and
        public event Action<WeatherEvent> OnActivateWeatherEvent;

        private RageBar _rageBar;

        [SerializeField] private WeatherData weatherData;
        private WeatherEventDataPasser[] weatherEventDataArray;

        private void Start() {
            //This is such a terrible way to do it but it's a game jam
            _rageBar = GameObject.FindGameObjectWithTag("RageBar").GetComponent<RageBar>();
            _rageBar.AddListener(UpdateButtonsBasedOnRagePoints);
            weatherEventDataArray = GameObject.FindObjectsByType<WeatherEventDataPasser>(FindObjectsSortMode.None);
        }

        //Not the prettiest way, basically can activate a specified weather event
        public void ActivateWeatherEvent(WeatherEventDataPasser weatherDataPasser) {
            OnActivateWeatherEvent?.Invoke(weatherDataPasser.weatherEvent);
            weatherDataPasser.button.interactable = false;
            _rageBar.DecreaseRageBar(this.weatherData.GetRagePoints(weatherDataPasser.weatherEvent));
            Timer timer = new Timer(this.weatherData.GetCooldownTime(weatherDataPasser.weatherEvent), this,
                () => weatherDataPasser.button.interactable = true);
            if (weatherDataPasser.button.TryGetComponent(out ButtonCooldown component)) {
                component.StartCooldown(timer);
            }
        }

        
        //loop through all the weather events
        private void UpdateButtonsBasedOnRagePoints(float newRageValue) {
            foreach (WeatherEventDataPasser data in weatherEventDataArray) {
                float ragePoints = weatherData.GetRagePoints(data.weatherEvent);
                if (ragePoints > newRageValue) {
                    data.button.interactable = false;
                }
            }
        }

        [Serializable]
        public enum WeatherEvent {
            DustBowl, Tornado, Flood
        }
    
    }
}
