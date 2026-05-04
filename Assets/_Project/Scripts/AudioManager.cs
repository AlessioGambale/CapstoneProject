using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : GenericSingleton<AudioManager>
{
    [Header("Sound Data")]
    [SerializeField] private SoundData[] _sounds;

    private Dictionary<SoundID, SoundData> _soundDictionary;
    private AudioSource _audioSource;

    protected override void Awake()
    {
        base.Awake();

        _audioSource = GetComponent<AudioSource>();        
        MapDictionary();
    }

    private void MapDictionary()
    {
        _soundDictionary = new Dictionary<SoundID, SoundData>();
        foreach (var sound in _sounds)
        {
            _soundDictionary.TryAdd(sound.ID, sound);
        }
    }

    public void Play2D(SoundID id)
    {
        if (!_soundDictionary.TryGetValue(id, out var sound)) return;
        if (sound.Clips.Length == 0) return;

        AudioClip clip = sound.Clips[Random.Range(0, sound.Clips.Length)];
        _audioSource.pitch = Random.Range(0.95f, 1.05f);
        _audioSource.PlayOneShot(clip);
    }

    public void Play3DAttached(SoundID id, Transform emitter, float volume)
    {
        if (!_soundDictionary.TryGetValue(id, out var sound)) return;
        if (sound.Clips.Length == 0) return;

        AudioClip clip = sound.Clips[Random.Range(0, sound.Clips.Length)];

        AudioSource source = emitter.GetComponent<AudioSource>();

        if (source == null)
        {
            source = emitter.gameObject.AddComponent<AudioSource>();
            source.spatialBlend = 1f;
        }

        source.volume = volume;
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(clip);
    }

    public void Play3DPooled(SoundID id, Vector3 position, float volume)
    {
        if (!_soundDictionary.TryGetValue(id, out var sound)) return;
        if (sound.Clips.Length == 0) return;

        AudioClip clip = sound.Clips[Random.Range(0, sound.Clips.Length)];

    }
}