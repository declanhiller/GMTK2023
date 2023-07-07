using System;
using UnityEngine;
using UnityEngine.UI;

namespace WeatherEvents {
    public class WeatherController : MonoBehaviour {

        //Connect to this event to get when a certain weather event happens and
        public event Action<WeatherEvent> OnActivateWeatherEvent;

        
        [SerializeField] public float dustBowlTimer;
        [SerializeField] public float tornadoTimer;
        [SerializeField] public float floodTimer;

        //Not the prettiest way, basically can activate a specified weather event
        public void ActivateWeatherEvent(WeatherEventDataPasser weatherEvent) {
            OnActivateWeatherEvent?.Invoke(weatherEvent.weatherEvent);
            weatherEvent.button.interactable = false;
            Timer timer = new Timer(GetCooldownTime(weatherEvent.weatherEvent), this,
                () => weatherEvent.button.interactable = true);
            if (weatherEvent.button.TryGetComponent(out ButtonCooldown component)) {
                component.StartCooldown(timer);
            }
        }

        public void TestButton(WeatherEvent evt) {
            
        }

        private float GetCooldownTime(WeatherEvent weatherEvent) {
            switch (weatherEvent) {
                case WeatherEvent.DustBowl:
                    return dustBowlTimer;
                case WeatherEvent.Flood:
                    return floodTimer;
                case WeatherEvent.Tornado:
                    return tornadoTimer;
                default:
                    return 5;
            }
        }

        [Serializable]
        public enum WeatherEvent {
            DustBowl, Tornado, Flood
        }
    
    }
}
