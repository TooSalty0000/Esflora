using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] 
    private Sprite[] soundButtons;
    [SerializeField]
    private Image musicButton;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Audio[] soundEffects;
    [SerializeField] private AudioSource backgroundMusic;
    private bool useMusic = true;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        useMusic = true;
        musicButton.sprite = soundButtons[0];
        StartCoroutine(playClip(Random.Range(0, clips.Length)));
        foreach (var audio in soundEffects)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.playOnAwake = false;
            audio.source.clip = audio.clip;
            audio.source.volume = audio.volume;
            audio.source.loop = false;
        }
    }

    private IEnumerator playClip(int index) {
        backgroundMusic.clip = clips[index];
        backgroundMusic.Play();
        yield return new WaitForSeconds(backgroundMusic.clip.length);
        StartCoroutine(playClip((index + 1) % clips.Length));
    }

    public void playSound(string name, bool backgroundOff = false) {
        if (!useMusic) return;
        if (backgroundOff) {
            StopAllCoroutines();
            backgroundMusic.Stop();
        }
        foreach (var audio in soundEffects)
        {
            if (audio.name == name) {
                audio.source.Play();
            }
        }
    }

    public void toggleMusic() {
        if (useMusic) {
            useMusic = false;
            musicButton.sprite = soundButtons[1];
            backgroundMusic.volume = 0;
        } else {
            useMusic = true;
            musicButton.sprite = soundButtons[0];
            backgroundMusic.volume = 0.7f;
        }
    }

}
