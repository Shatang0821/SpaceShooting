using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;

    [SerializeField] float minPitch = 0.9f;

    [SerializeField] float maxPitch = 1.1f;

    /// <summary>
    /// Used for UI SFX
    /// </summary>
    /// <param name="audioClip">—¬‚·‰¹</param>
    /// <param name="volume">‰¹—Ê</param>
    public void PlaySFX(AudioData audioData)
    {
        sFXPlayer.PlayOneShot(audioData.audioClip,audioData.volueme);
    }

    // Used for repeat-player SFX
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(minPitch, maxPitch);
        PlaySFX(audioData);
    }

    public void PlayRandomSFX(AudioData[] audioData)
    {
        PlayRandomSFX(audioData[Random.Range(0, audioData.Length)]);
    }
}

[System.Serializable] public class AudioData
{
    public AudioClip audioClip;

    public float volueme;
}
