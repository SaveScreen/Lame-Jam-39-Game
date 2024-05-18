using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum AudioManagerChannels
{
    MusicChannel = 0,
    SFXChannel
}
public class AudioManager : MonoBehaviour
{
    [Header("Variables")]
    public static AudioManager instance;
    public static float musicChannelVolume = 1f;
    public static float sfxChannelVolume = 1f;
    public AudioSource musicChannel;
    public AudioSource sfxChannel;

    [Header("Music List")]
    [SerializeField] private AudioClip titleTheme;
    [SerializeField] private AudioClip levelTheme;
    [SerializeField] private AudioClip gameOverTheme;

    [Header("SFX")]
    public List<AudioSource> soundEffectSources = new List<AudioSource>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        musicChannel = GetComponents<AudioSource>()[0];
        sfxChannel = GetComponents<AudioSource>()[1];
    }

    //plays music based on the scenename
    //stops all previous music and sfx on scene change
    public void PlayMusicOnSceneSchange(string sceneName)
    {
        StopSound(AudioManagerChannels.MusicChannel);
        StopSound(AudioManagerChannels.SFXChannel);

        if (0 < soundEffectSources.Count)
        {
            foreach(AudioSource soundEffectSource in soundEffectSources.ToList())
            {
                if (soundEffectSource == null)
                {
                    soundEffectSources.Remove(soundEffectSource);
                }
                else
                {
                    soundEffectSource.Stop();
                    KillAudioSource(soundEffectSource);
                }
            }
        }

        switch (sceneName)
        {
            case "Menu":
            {
            PlaySound(AudioManagerChannels.MusicChannel, titleTheme);
            break;
            }
            case "": //idk what the scene name will be
            {
            PlaySound(AudioManagerChannels.MusicChannel, levelTheme);
            break;
            }
        }
    }
    public static void SetChannelVolume(int target, float value)
    {
        switch (target)
        {
            case 0:
                musicChannelVolume = value;
                instance.musicChannel.volume = musicChannelVolume;
                break;
            case 1:
                sfxChannelVolume = value;
                instance.sfxChannel.volume = sfxChannelVolume;
                break;
            default:
                break;
        }
    }

    public void SetMusicLoop()
    {
        musicChannel.loop = !musicChannel.loop;
    }

    //plays audio on a specific channel
    public void PlaySound(AudioManagerChannels target, AudioClip clip)
    {
        switch (target)
        {
            case AudioManagerChannels.MusicChannel:
                {}musicChannel.Stop();
                musicChannel.clip = clip;
                musicChannel.Play();
                break;
            case AudioManagerChannels.SFXChannel:
                sfxChannel.clip = clip;
                sfxChannel.Play();
                break;
        }
    }

    public void PlaySound(AudioManagerChannels target, AudioClip clip, float pitch)
    {
        switch (target)
        {
            case AudioManagerChannels.MusicChannel:
                musicChannel.Stop();
                musicChannel.clip = clip;
                musicChannel.pitch = pitch;
                musicChannel.Play();
                break;
            case AudioManagerChannels.SFXChannel:
                sfxChannel.pitch = pitch;
                sfxChannel.clip = clip;
                sfxChannel.Play();
                break;
        }
    }

    //stops sound on specific channel
    public void StopSound(AudioManagerChannels target)
    {
        switch(target)
        {
            case AudioManagerChannels.MusicChannel:
                musicChannel.Stop();
                break;
            case AudioManagerChannels.SFXChannel:
                sfxChannel.Stop();
                break;
            
        }
    }

    public AudioSource AddSFX(AudioClip newSFX, bool loop, AudioSource lastInstanceToKill)
    {
        if (lastInstanceToKill)
        {
            KillAudioSource(lastInstanceToKill);
        }
        if (newSFX != null)
        {
            AudioSource sfx = gameObject.AddComponent(typeof (AudioSource)) as AudioSource;
            soundEffectSources.Add(sfx);
            sfx.clip = newSFX;
            sfx.Play();
            sfx.outputAudioMixerGroup = sfxChannel.outputAudioMixerGroup;
            if (loop)
            {
                sfx.loop = true;
            }
            return sfx;
        }
        else
        {
            return null;
        }
    }

    public AudioSource KillAudioSource(AudioSource audioSource)
    {
        if(audioSource)
        {
            soundEffectSources.Remove(audioSource);
            Destroy(audioSource);
        }
        return null;
    }
}
