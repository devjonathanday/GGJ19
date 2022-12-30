using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource channel;
    public AudioClip testClip;

    private void Awake()
    {
        tag = "SoundManager";
        GameObject[] clones = GameObject.FindGameObjectsWithTag("SoundManager");
        if (clones.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PlayOnce(testClip);
    }

    public void PlayOnce(AudioClip clip)
    {
        channel.PlayOneShot(clip);
    }
}