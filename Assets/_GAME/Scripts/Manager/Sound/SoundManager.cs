namespace _GAME.Scripts.Sound
{
    using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;
        
        [HideInInspector]
        public AudioSource source;
    }
    
    [SerializeField] private List<Sound> sounds = new List<Sound>();
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private bool muteSound = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        InitializeSounds();
    }
    
    private void InitializeSounds()
    {
        foreach (Sound sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            sound.source = source;
            source.clip = sound.clip;
            source.volume = sound.volume * masterVolume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
        }
    }
    
    public void PlaySound(string name)
    {
        if (muteSound) return;
        
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
    }
    
    public void StopSound(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.source.Stop();
        }
    }
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        
        foreach (Sound sound in sounds)
        {
            if (sound.source != null)
            {
                sound.source.volume = sound.volume * masterVolume;
            }
        }
    }
    
    public void MuteSound(bool mute)
    {
        muteSound = mute;
        
        if (mute)
        {
            foreach (Sound sound in sounds)
            {
                if (sound.source != null && sound.source.isPlaying)
                {
                    sound.source.Stop();
                }
            }
        }
    }
}
}