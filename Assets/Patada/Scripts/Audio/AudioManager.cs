using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
	private static AudioManager instance = null;
	public static AudioManager Instance{
		get {
			if (instance == null) {
				instance = FindObjectOfType<AudioManager> ();
			}

			return instance;
		}
	}

    [SerializeField] private AudioMixer mixer = null;

	private List<AudioSource> aSources;
	// [SerializeField] private List<AudioClip> clipList = null;
	[SerializeField] private List<SCOB_AUD_ClipInfo> clips = null;
	[SerializeField] private Dictionary<string,SCOB_AUD_ClipInfo> aClips = null;
	[SerializeField] private UnityEngine.Audio.AudioMixerGroup effectsMixerGroup = null;
	[SerializeField] private UnityEngine.Audio.AudioMixerGroup masterMixerGroup = null;

	[SerializeField] private Image muteIMG = null;
	[SerializeField] private List<Sprite> muteSprites = null;

	[SerializeField] private AudioSource bgMusic = null;
	[SerializeField] private List<AudioClip> bgMusicClips = null;
	[SerializeField] private List<float> bgMusicVolumes = null;
	
	private float lastSFXTime = 0f;
	private string lastSFXName = "";
	[SerializeField] private float sfxMinTime = .3f;

	[SerializeField] private float sfxDefaultVolume = 0f;
	[SerializeField] private float musicDefaultVolume = 0f;

	void Awake() {
		if(AudioManager.Instance != this) {
			Destroy(this.gameObject);
		} else {
			DontDestroyOnLoad(this.gameObject);
		}
	}

	void Start() {
		var sfxVolume = Save.Instance.GetFloat("sfx_volume", -5f);
		mixer.SetFloat("sfxVolume", sfxVolume);

		var musicVolume = Save.Instance.GetFloat("music_volume", -20f);
		mixer.SetFloat("musicVolume", musicVolume);
	}

	private void PopulateASources() {
		if(aSources == null) {
			aSources = new List<AudioSource>();
			var currentSources = GetComponents<AudioSource> ();
			for (int i = 0; i < currentSources.Length; i++) {
				aSources.Add (currentSources[i]);
			}
		}
	}

	private void InitializeAClips() {
		if(aClips == null) {
			aClips = new Dictionary<string, SCOB_AUD_ClipInfo> ();
			for (int i = 0; i < clips.Count; i++) {
				aClips.Add (clips [i].clip.name.ToLower(), clips [i]);
			}
		}
	}


	// Use this for initialization
	public void Initialize () {
		if(Save.Instance.GetBool("sfx_muted")){
			MuteSFX();
		}

		if(Save.Instance.GetBool("music_muted")){
			MuteMusic();
		}

		
		InitializeAClips();
		PopulateASources();

		// if (Mathf.Approximately (Save.Instance.GetFloat ("volume"), -80f)) {
		// 	if(muteIMG != null) {
		// 		muteIMG.sprite = muteSprites [1];
		// 	}

		// 	Mute ();
		// } else {
		// 	if(bgMusic.clip == null) {
		// 		RandomizeBGMusic();
		// 	}

		// 	bgMusic.Play ();
		// }
	}

	void OnDisable(){

	}

	private void RandomizeBGMusic(){
		bgMusic.clip = bgMusicClips[Random.Range(0, bgMusicClips.Count)];
	}

	public void PlayNewMusic(string clipName = null) {
		if(string.IsNullOrEmpty(clipName)) {
			var pool = new List<AudioClip> ();
			for(int i = 0 ; i < bgMusicClips.Count ; i++){
				if(bgMusic.clip != bgMusicClips[i]) {
					pool.Add(bgMusicClips[i]);
				}
			}

			bgMusic.clip = pool[Random.Range(0,pool.Count)];
		} else {
			bgMusic.clip = bgMusicClips.Find((item) => item.name.Equals(clipName));
			if(bgMusic.clip == null) {
				throw new UnityException("Music clip not found {" + clipName + "}");
			}
		}

		bgMusic.Play();
	}

	// public void PlaySound(string soundName, float volume) {
	// 	if((!soundName.Equals(lastSFXName)) || Time.unscaledTime - lastSFXTime >= sfxMinTime) {
	// 		lastSFXTime = Time.unscaledTime;
	// 		lastSFXName = soundName;
			
	// 		var availableASource = FindAvailableASource ();
	// 		var aClip = GetClipByName (soundName);

	// 		if (aClip != null) {
	// 			availableASource.clip = aClip;
	// 			availableASource.volume = volume;
	// 			availableASource.Play ();
	// 		} else {
	// 			Debug.LogError ("Audio clip (" + soundName + ") not found!");
	// 		}
	// 	}
	// }

	public void PlaySound(string soundName) {
		var availableASource = FindAvailableASource ();
		var aClip = GetClipByName (soundName);

		aClip.Play(availableASource);
	}

	public void StopSound(string soundName) {
		var aPlayingSource = FindASourcePlayingClip(soundName);
		if(aPlayingSource != null) {
			aPlayingSource.Stop();
		}
	}

	public AudioSource FindAvailableASource(){
		AudioSource available = null;

		PopulateASources();

		for (int i = 0; i < aSources.Count && available == null; i++) {
			if (!aSources [i].isPlaying) {
				available = aSources [i];
			}
		}

		if (available == null) {
			available = CreateNewASource ();
		}

		return available;
	}

	public AudioSource FindASourcePlayingClip(string clipName){
		PopulateASources();
		InitializeAClips();

		AudioSource available = null;
		for (int i = 0; i < aSources.Count && available == null; i++) {
			if (aSources [i].isPlaying && aSources [i].clip != null && aSources[i].clip.name.Equals(clipName)) {
				return aSources[i];
			}
		}

		return null;
	}

	private AudioSource CreateNewASource() {
		var newASource = gameObject.AddComponent<AudioSource> ();
		newASource.outputAudioMixerGroup = effectsMixerGroup;
		aSources.Add (newASource);

		return newASource;
	}

	private SCOB_AUD_ClipInfo GetClipByName(string name) {
		InitializeAClips();

		var normalizedName = name.ToLower ();

		if (aClips.ContainsKey (normalizedName)) {
			return aClips [normalizedName];
		} else {
			return null;
		}
	}

	public void Mute(){
		var volume = -1f;
		masterMixerGroup.audioMixer.GetFloat ("masterVolume", out volume);

		if (Mathf.Approximately (0f, volume)) {
			if(muteIMG != null) {
				muteIMG.sprite = muteSprites [1];
			}

			masterMixerGroup.audioMixer.SetFloat ("masterVolume", -80f);

			Save.Instance.SetBool ("sfx_muted", true);
			Save.Instance.SetBool ("music_muted", true);
		} else {
			if(muteIMG != null) {
				muteIMG.sprite = muteSprites [0];
			}

			masterMixerGroup.audioMixer.SetFloat ("masterVolume", 0f);
			
			Save.Instance.SetBool ("sfx_muted", false);
			Save.Instance.SetBool ("music_muted", false);
		}
	}

	private void OnPlayerSkinChanged(int playerID) {
		var currentPlaybackTime = bgMusic.time;

		if (playerID == 3) {
			bgMusic.clip = bgMusicClips [1];
			bgMusic.volume = bgMusicVolumes [1];

			currentPlaybackTime = Mathf.Min (bgMusicClips [1].length, currentPlaybackTime);
		} else if (playerID == 4 || playerID == 5) {
			bgMusic.clip = bgMusicClips [2];
			bgMusic.volume = bgMusicVolumes [2];

			currentPlaybackTime = Mathf.Min (bgMusicClips [2].length, currentPlaybackTime);
		} else {
			bgMusic.clip = bgMusicClips [0];
			bgMusic.volume = bgMusicVolumes [0];

			currentPlaybackTime = Mathf.Min (bgMusicClips [0].length, currentPlaybackTime);
		}

		bgMusic.time = currentPlaybackTime;
		bgMusic.Play ();
	}

	public void MuteSFX () {
        mixer.SetFloat("sfxVolume",-80f);
        // Save.Instance.SetBool("sfx_muted", true);
    }

    public void UnmuteSFX () {
        mixer.SetFloat("sfxVolume",sfxDefaultVolume);
        // Save.Instance.SetBool("sfx_muted", false);
    }

    public void MuteMusic () {
        mixer.SetFloat("musicVolume",-80f);
        // Save.Instance.SetBool("music_muted", true);
    }

    public void UnmuteMusic () {
        mixer.SetFloat("musicVolume",musicDefaultVolume);
        // Save.Instance.SetBool("music_muted", false);
    }
}
