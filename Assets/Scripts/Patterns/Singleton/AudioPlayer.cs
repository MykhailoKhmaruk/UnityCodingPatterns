using System.Collections;
using UnityEngine;

public class AudioPlayer {

    private static AudioPlayer _instance = null;
    public static AudioPlayer Instance => _instance ??= new AudioPlayer();
    
    private AudioSource _audioSource;
    private AudioPlayerBehaviour _behaviour;

    private float _fadeDuration = 0.5f;

    private AudioPlayer()
    {
        GameObject audioPlayerObject = new GameObject("AudioPlayer");
        Object.DontDestroyOnLoad(audioPlayerObject);

        _audioSource = audioPlayerObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        _behaviour = audioPlayerObject.AddComponent<AudioPlayerBehaviour>();
    }

    public void FadeNewMusic(AudioClip newClip)
    {
        if (newClip == null)
        {
            Debug.LogWarning("New music clip is null.");
            return;
        }
        _behaviour.StartCoroutine(FadeMusicRoutine(newClip));
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Music clip is null.");
            return;
        }
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.volume = 1f;
        _audioSource.Play();
    }
    public void StopMusic()
    {
        _audioSource.Stop();
    }
    
    public void PauseMusic()
    {
        _audioSource.Pause();
    }
    
    public void ResumeMusic()
    {
        _audioSource.UnPause();
    }
    
    public void SetVolume(float volume)
    {
        _audioSource.volume = Mathf.Clamp01(volume);
    }
    
    private IEnumerator FadeMusicRoutine(AudioClip newClip)
    {
  
        float startVolume = _audioSource.volume;
        float timer = 0f;

        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / _fadeDuration);
            yield return null;
        }

        _audioSource.Stop();
        _audioSource.clip = newClip;
        _audioSource.Play();

        timer = 0f;
        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(0f, 1f, timer / _fadeDuration);
            yield return null;
        }
        _audioSource.volume = 1f;
    }
}