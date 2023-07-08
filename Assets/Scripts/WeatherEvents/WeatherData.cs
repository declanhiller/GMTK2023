using UnityEngine;

namespace WeatherEvents {
    [CreateAssetMenu(fileName = "Weather Data", menuName = "GMTK", order = 0)]
    public class WeatherData : ScriptableObject {
        
        //not the right way to do scriptable objects but it's fineeeeeeeeeeee
        
        [SerializeField] public float dustBowlTimer;
        [SerializeField] public float tornadoTimer;
        [SerializeField] public float floodTimer;

        [SerializeField] public float dustBowlRagePoints;
        [SerializeField] public float tornadoRagePoints;
        [SerializeField] public float floodRagePoints;
        
        public float GetCooldownTime(WeatherButtonsController.WeatherEvent weatherEvent) {
            switch (weatherEvent) {
                case WeatherButtonsController.WeatherEvent.DustBowl:
                    return dustBowlTimer;
                case WeatherButtonsController.WeatherEvent.Flood:
                    return floodTimer;
                case WeatherButtonsController.WeatherEvent.Tornado:
                    return tornadoTimer;
                default:
                    return 5;
            }
        }

        public float GetRagePoints(WeatherButtonsController.WeatherEvent weatherEvent) {
            switch (weatherEvent) {
                case WeatherButtonsController.WeatherEvent.DustBowl:
                    return dustBowlRagePoints;
                case WeatherButtonsController.WeatherEvent.Tornado:
                    return tornadoRagePoints;
                case WeatherButtonsController.WeatherEvent.Flood:
                    return floodRagePoints;
                default:
                    return 50;
            }
        }

    }
}
