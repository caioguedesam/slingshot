using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GetGroupVolume : MonoBehaviour {
    [SerializeField] private AudioMixer mixer = null;
    [SerializeField] private string key = "";

    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private EasingFunction.Ease ease = EasingFunction.Ease.Linear;
    
    public float GetVolume () {
        var value = 0f;
        mixer.GetFloat(key, out value);

        return value;
    }

    public void UpdateVolumePercentage () {
        var value = GetVolume();
        
        var eFunc = EasingFunction.GetEasingFunction(ease);

        var min = 0f;
        var max = 1f;
        while(Mathf.Abs(max - min) > .01f) {
            var half = (max + min) / 2f;
            var halfVolume = eFunc.Invoke(-80f, 0f, half);

            if(halfVolume > value) {
                max = half;
            } else {
                min = half;
            }
        }

        if(volumeSlider != null) {
            volumeSlider.value = min;
        }
    }
}