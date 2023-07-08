using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WeatherEvents {
    public class WeatherButtonsController : MonoBehaviour {

        //Connect to this event to get when a certain weather event happens and
        //Weather Event is the weather event and the bool is whether it was activated or deactivated
        public event Action<WeatherEventDataPasser, bool> OnWeatherToggleClick;

        private RageBar _rageBar;

        [SerializeField] private Color selectedColor = new Color(255, 0, 0);
        [SerializeField] private Color restingColor = new Color(1,1, 1);

        [SerializeField] private WeatherData weatherData;
        private WeatherEventDataPasser[] weatherEventDataArray;

        private WeatherEventDataPasser _currentSelectedButton;

        private void Start() {
            //This is such a terrible way to do it but it's a game jam
            _rageBar = GameObject.FindGameObjectWithTag("RageBar").GetComponent<RageBar>();
            _rageBar.AddListener(UpdateButtonsBasedOnRagePoints);
            weatherEventDataArray = GameObject.FindObjectsByType<WeatherEventDataPasser>(FindObjectsSortMode.None);
        }

        //Not the prettiest way, basically can activate a specified weather event
        public void WeatherEventToggle(WeatherEventDataPasser weatherDataPasser) {
            if (_currentSelectedButton == weatherDataPasser) {
                OnWeatherToggleClick?.Invoke(weatherDataPasser, false);
                weatherDataPasser.button.GetComponent<Image>().color = restingColor;
                _currentSelectedButton = null;
                return;
            }


            if (_currentSelectedButton != null)
                _currentSelectedButton.button.GetComponent<Image>().color = restingColor;

            weatherDataPasser.button.GetComponent<Image>().color = selectedColor;
            OnWeatherToggleClick?.Invoke(weatherDataPasser, true);
            _currentSelectedButton = weatherDataPasser;

        }

        //Just starts the cooldown for the button and decreases the cooldown
        public void RegisterStartWeatherEvent(WeatherEventDataPasser weatherDataPasser) {
            weatherDataPasser.button.interactable = false;
            _rageBar.DecreaseRageBar(weatherData.GetRagePoints(weatherDataPasser.weatherEvent));
            Timer timer = new Timer(weatherData.GetCooldownTime(weatherDataPasser.weatherEvent), this,
                () => weatherDataPasser.button.interactable = true);
            if (weatherDataPasser.button.TryGetComponent(out ButtonCooldown component)) {
                component.StartCooldown(timer);
            }

            _currentSelectedButton.button.GetComponent<Image>().color = restingColor;
            _currentSelectedButton = null;
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
