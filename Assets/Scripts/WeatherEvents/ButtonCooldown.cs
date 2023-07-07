using System;
using UnityEngine;

namespace WeatherEvents {
    public class ButtonCooldown : MonoBehaviour {

        private Timer _timer;
        
        public void StartCooldown(Timer timer) {
            _timer = timer;
        }

        private void Update() {
            float remainingTime = _timer.GetCurrentRemainingTime;
            float totalTime = _timer._totalDuration;
            //use this to change cooldown
            float percentageOfCoolDownTime = remainingTime / totalTime;
            
        }
    }
}
