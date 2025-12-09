using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<AudioClip> commonSoundList;
    
    public Dictionary<string, AudioClip> soundDict = new Dictionary<string, AudioClip>();

    private List<AudioSource> sourcePool = new List<AudioSource>();
    private int nextSource = 0;

    public float overallVolume = 1f;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < 10; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            sourcePool.Add(src);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
            
        foreach (var clip in commonSoundList) {
            soundDict[clip.name] = clip;
        }

        //prewarm
        foreach (var clip in soundDict.Values) {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero, 0f);
        }

        overallVolume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOneShot(string clipName, bool pitchVariation = false, float volume = -1f) {
        if (!soundDict.ContainsKey(clipName)) {
            Debug.Log("audio clip not found!");
            return;
        }

        AudioSource source = sourcePool[nextSource];
        source.clip = soundDict[clipName];
        source.volume = volume == -1f ? overallVolume : volume;

        if (pitchVariation) {
            source.pitch = Random.Range(0.9f, 1.1f);
        } else {
            source.pitch = 1f;
        }

        source.Play();

        nextSource = (nextSource + 1) % sourcePool.Count;
    }

    public void SetVolume(float volume) {
        overallVolume = volume;
    }

    void OnDestroy() {
        foreach (var source in sourcePool) {
            source.Stop();
        }
    }
}
