using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioClip backgrounMusic;

    [SerializeField]
    private AudioClip ghostTheme;

    // Start is called before the first frame update
    void Start() {
        ChangeTrack(backgrounMusic);
        //MusicTimer(backgrounMusic.length, delegate{ChangeTrack(ghostTheme);});
    }

    private IEnumerator MusicTimer(float time, Action action = null) {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    private void ChangeTrack(AudioClip clip) {
        musicSource.clip = clip;
        musicSource.Play();
        StartCoroutine(MusicTimer(clip.length, delegate{ChangeTrack(ghostTheme);}));
    }
}
