using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

[CreateAssetMenu (fileName = "SCOBI_AUD_ClipInfo_", menuName = "Patada/Audio/ClipInfo", order = 1)]
public class SCOB_AUD_ClipInfo : ScriptableObject {
    public AudioClip clip = null;
    public float minSpamTime = 0f;
    public int maxActiveClips = 1;
    public float defaultVolume = 1f;
    public AudioMixerGroup mixerGroup = null;
    public bool loop = false;
    public float minPitch = 1f;
    public float maxPitch = 1f;

    private float lastPlayedTime = 0f;
    private int currentPlayingClips = 0;
    private List<AudioSource> activeSrcs = new List<AudioSource>();

    public AnimationCurve volumeCurve = new AnimationCurve();

    void OnEnable() {
        Reset();
    }

    void OnDisable() {
        Reset();
    }

    private IEnumerator WaitClipFinish (AudioSource src) {
        yield return new WaitUntil(() => !src.isPlaying);
        currentPlayingClips--;
    }

    public void Play (AudioSource src) {
        var clipCountCheck = currentPlayingClips + 1 <= maxActiveClips;
        var clipTimeCheck = Time.realtimeSinceStartup - lastPlayedTime >= minSpamTime;

        if (clipCountCheck && clipTimeCheck) {
            lastPlayedTime = Time.realtimeSinceStartup;
            currentPlayingClips++;

            src.clip = clip;
            src.pitch = Random.Range (minPitch, maxPitch);
            src.loop = loop;
            src.volume = defaultVolume;
            src.outputAudioMixerGroup = mixerGroup;

            src.Play();

            #if UNITY_EDITOR
            if(!Application.isPlaying) {
                EditorCoroutineUtility.StartCoroutine(WaitClipFinish(src), this);
            } else {
                AudioManager.Instance.StartCoroutine(WaitClipFinish(src));
            }
            #else
                AudioManager.Instance.StartCoroutine(WaitClipFinish(src));
            #endif
        }
    }

    private void Reset(){
        currentPlayingClips = 0;
        lastPlayedTime = 0f;
        activeSrcs = new List<AudioSource>();
    }
}