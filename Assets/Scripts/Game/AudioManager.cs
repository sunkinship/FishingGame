using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private const string SFX_PATH = "SFX/";
    private const string MUSIC_PATH = "Music/";

    [SerializeField] private Transform audioRoot;

    private List<AudioSource> allSFX = new();
    private List<AudioSource> allMusic = new();

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFX(string sfx, float volume = 1f)
    {
        AudioClip clip = LoadClip(sfx, true);
        CreateSFX(sfx, clip, volume);
    }

    public void PlayMusic(string music, float volume = 1f)
    {
        AudioClip clip = LoadClip(music, false);
        CreateMusic(music, clip, volume);
    }

    private AudioClip LoadClip(string name, bool isSFX)
    {
        AudioClip clip = Resources.Load<AudioClip>((isSFX ? SFX_PATH : MUSIC_PATH) + name);

        if (clip != null)
            return clip;
        else
        {
            Debug.LogError($"Could not load audio clip from path '{(isSFX ? SFX_PATH : MUSIC_PATH) + name}'.");
            return null;
        }
    }

    private void CreateSFX(string name, AudioClip clip, float volume = 1f)
    {
        GameObject obj = new(name);
        obj.transform.SetParent(audioRoot);
        AudioSource audioSource = obj.AddComponent<AudioSource>();

        audioSource.loop = false;
        audioSource.clip = clip;
        audioSource.volume = volume;
       
        allSFX.Add(audioSource);

        audioSource.Play();

        if (!audioSource.loop)
            StartCoroutine(DestroySFX(audioSource));
    } 

    private void CreateMusic(string name, AudioClip music, float volume = 1f)
    {
        GameObject obj = new(name);
        obj.transform.SetParent(audioRoot);
        AudioSource audioSource = obj.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.clip = music;
        audioSource.volume = volume;
        
        allMusic.Add(audioSource);

        audioSource.Play();
    }

    private IEnumerator DestroySFX(AudioSource sfx)
    {
        yield return new WaitForSeconds(sfx.clip.length + 1);
        allSFX.Remove(sfx);
        Destroy(sfx.gameObject);
    }

    public void StopMusic(string name)
    {
        AudioSource source = GetMusic(name);

        if (source == null)
            return;

        allMusic.Remove(source);
        Destroy(source.gameObject);
    }

    private AudioSource GetMusic(string name)
    {
        foreach (var clip in allMusic)
        {
            if (name == clip.name)
                return clip;
        }

        Debug.LogWarning($"Could not find music in scene called '{name}'");
        return null;
    }
}
