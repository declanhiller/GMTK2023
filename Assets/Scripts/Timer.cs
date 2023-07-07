using System;
using System.Collections;
using UnityEngine;

public class Timer {

    public event Action OnTimerFinish;
        
    private readonly float _startTime;
    public float GetCurrentRemainingTime => Time.time - _startTime;
    public readonly float _totalDuration;

    public Timer(float totalDuration, MonoBehaviour callingScript, Action onTimerFinish) {
        _totalDuration = totalDuration;
        _startTime = Time.time;
        callingScript.StartCoroutine(StartTimer(totalDuration));
        OnTimerFinish += onTimerFinish;
    }

    IEnumerator StartTimer(float totalDuration) {
        yield return new WaitForSeconds(totalDuration);
        OnTimerFinish?.Invoke();
    }

}