using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {
    public void Play(string audioName) {
        AudioManager.Instance.PlaySound(audioName);
    }
    public void Stop(string audioName) {
        AudioManager.Instance.StopSound(audioName);
    }
}
