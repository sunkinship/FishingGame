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

    private void CreateMusic(string name, AudioClip music, float volume = 1f, float fadeSpeed = 3f)
    {
        GameObject obj = new(name);
        obj.transform.SetParent(audioRoot);
        AudioSource audioSource = obj.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.clip = music;
        audioSource.volume = 0;
        
        allMusic.Add(audioSource);

        audioSource.Play();
        StartCoroutine(FadeInMusic(audioSource, volume, fadeSpeed));
    }

    private IEnumerator DestroySFX(AudioSource sfx)
    {
        yield return new WaitForSeconds(sfx.clip.length + 1);
        allSFX.Remove(sfx);
        Destroy(sfx.gameObject);
    }

    public void StopMusic(string name, float fadeSpeed = 3f)
    {
        AudioSource source = GetMusic(name);

        if (source == null)
            return;

        allMusic.Remove(source);
        StartCoroutine(FadeOutMusic(source, fadeSpeed));
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

    private IEnumerator FadeOutMusic(AudioSource source, float fadeSpeed = 3f)
    {
        while (source.volume > 0)
        {
            source.volume = Mathf.MoveTowards(source.volume, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(source.gameObject);
    }

    private IEnumerator FadeInMusic(AudioSource source, float fadeInVolume = 1f, float fadeSpeed = 3f)
    {
        while (source.volume < fadeInVolume)
        {
            source.volume = Mathf.MoveTowards(source.volume, fadeInVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
