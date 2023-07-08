using System;
using System.Collections.Generic;
using MapScripts;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace WeatherEvents {
    public class WeatherController : MonoBehaviour {

        [SerializeField] private WeatherButtonsController weatherButtonsController;

        private Map _map;

        // private List<Tuple<WeatherButtonsController.WeatherEvent, bool>> _weatherEventsActive;
        private WeatherEventDataPasser _currentSelectedWeatherEvent;

        [SerializeField] private PlayerController player;
        
        private void Start() {
            weatherButtonsController.OnWeatherToggleClick += OnWeatherToggleClick;
            player.Keybinds.Player.Click.performed += ActivateSelectedWeatherEvent;
            _map = GameObject.FindObjectOfType<Map>();
        }

        public void ActivateSelectedWeatherEvent(InputAction.CallbackContext context) {
            if (_currentSelectedWeatherEvent == null) return;
            Cell cell;
            if (!ValidInput(out cell)) return;
            Debug.Log("Start Weather Event: " + _currentSelectedWeatherEvent.weatherEvent);
            weatherButtonsController.RegisterStartWeatherEvent(_currentSelectedWeatherEvent);
            _currentSelectedWeatherEvent = null;
            //Start weather event at cell
        }

        private bool ValidInput(out Cell cell) {
            return _map.HasCell(player._mousePosition, out cell);
        }

        private void OnWeatherToggleClick(WeatherEventDataPasser dataPasser, bool activated) {

            if (!activated && _currentSelectedWeatherEvent == dataPasser) {
                _currentSelectedWeatherEvent = null;
                return;
            }

            _currentSelectedWeatherEvent = dataPasser;

        }


    }
}
