using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChangeGroupVolume : MonoBehaviour {
    [SerializeField] private AudioMixer mixer = null;
    [SerializeField] private string key = "";
    [SerializeField] private string saveKey = "";

    [SerializeField] private float minValue = -80f;
    [SerializeField] private float maxValue = 0f;
    [SerializeField] private EasingFunction.Ease ease = EasingFunction.Ease.Linear;
    
    public void SetVolume (float value) {
        mixer.SetFloat(key, value);
        if(saveKey != null) {
            Save.Instance.SetFloat(saveKey, value);
        }
    }

    public void SetVolumePercentage (float percentage) {
        var eFunc = EasingFunction.GetEasingFunction(ease);
        var value =  eFunc.Invoke(minValue, maxValue, percentage);
        SetVolume(value);
    }
}