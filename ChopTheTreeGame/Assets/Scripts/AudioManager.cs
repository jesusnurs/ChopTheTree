using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private const string MusicMutedKey = "IsMuted";
    private const string SoundsMutedKey = "IsMuted";

    public static AudioManager Instance { get; private set; }
	
    [SerializeField] private List<Sound> soundDictionary;
    [SerializeField] private AudioSource soundsAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
	
    private bool isSoundsMuted;
    private bool isMusicMuted;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
		
        isSoundsMuted = PlayerPrefs.GetInt(SoundsMutedKey, 0) == 1;
        isMusicMuted = PlayerPrefs.GetInt(MusicMutedKey, 0) == 1;
		
        PlayMusic(AudioClipEnum.Background);
    }

    public void ToggleSoundsMute()
    {
        isSoundsMuted = !isSoundsMuted;
        PlayerPrefs.SetInt(SoundsMutedKey, isSoundsMuted ? 1 : 0);
    }

    public void ToggleMusicMute()
    {
        isMusicMuted = !isMusicMuted;
        PlayerPrefs.SetInt(MusicMutedKey, isMusicMuted ? 1 : 0);
    }

    public void PlaySound(AudioClipEnum soundEnum)
    {
        if(isSoundsMuted)
            return;
		
        Sound sound = soundDictionary.Find(x => x.soundEnum == soundEnum);
		
        soundsAudioSource.PlayOneShot(sound.GetRandomAudioClip());
    }

    public void PlayMusic(AudioClipEnum musicEnum)
    {
        if(isMusicMuted)
            return;
		
        Sound music = soundDictionary.Find(x => x.soundEnum == musicEnum);
        musicAudioSource.clip = music.GetRandomAudioClip();
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }
	
    public bool IsSoundMuted() => isSoundsMuted;
    public bool IsMusicMuted() => isMusicMuted;
}

[Serializable]
public struct Sound
{
    public AudioClipEnum soundEnum;
    public List<AudioClip> audioClips;
    
    public AudioClip GetRandomAudioClip() => audioClips[Random.Range(0, audioClips.Count)];
}

public enum AudioClipEnum
{
    Background,
    ChopLog
}

