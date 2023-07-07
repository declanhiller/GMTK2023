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
        
        public float GetCooldownTime(WeatherController.WeatherEvent weatherEvent) {
            switch (weatherEvent) {
                case WeatherController.WeatherEvent.DustBowl:
                    return dustBowlTimer;
                case WeatherController.WeatherEvent.Flood:
                    return floodTimer;
                case WeatherController.WeatherEvent.Tornado:
                    return tornadoTimer;
                default:
                    return 5;
            }
        }

        public float GetRagePoints(WeatherController.WeatherEvent weatherEvent) {
            switch (weatherEvent) {
                case WeatherController.WeatherEvent.DustBowl:
                    return dustBowlRagePoints;
                case WeatherController.WeatherEvent.Tornado:
                    return tornadoRagePoints;
                case WeatherController.WeatherEvent.Flood:
                    return floodRagePoints;
                default:
                    return 50;
            }
        }

    }
}
