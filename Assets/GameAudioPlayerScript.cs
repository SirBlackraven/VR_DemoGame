using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffectEntry
{
    public string SoundEffectName;
    public AudioClip clip;
}


public class GameAudioPlayerScript : MonoBehaviour
{
    public AudioSource Player;

    public List<SoundEffectEntry> effectsList = new List<SoundEffectEntry>();

    public List<AudioSource> audioSources = new List<AudioSource>();

    public void PlayEnterMech()
    {
        DebugManagerScript.Instance.AddMessage("enter mech audio play");
        //AudioClip clip = effectsList[0].clip;        
        //Player.PlayOneShot(clip);

        AudioSource aSource = audioSources[1];
        aSource.PlayOneShot(aSource.clip);
    }

    public void PlayLaserFire()
    {
        //AudioClip clip = effectsList[1].clip;
        //Player.PlayOneShot(clip);
        DebugManagerScript.Instance.AddMessage("laser fire audio");

        AudioSource aSource = audioSources[1];
        aSource.PlayOneShot(aSource.clip);
    }

    public void PlayButtonClick()
    {
        AudioClip clip = effectsList[2].clip;
        Player.PlayOneShot(clip);
    }

    public void PlayEnemyKilled()
    {
        AudioClip clip = effectsList[3].clip;
        Player.PlayOneShot(clip);
    }

    public void PlayPlayerHit()
    {
        AudioClip clip = effectsList[4].clip;
        Player.PlayOneShot(clip);
    }

    public void PlayPlayerKilled()
    {
        AudioClip clip = effectsList[5].clip;
        Player.PlayOneShot(clip);
    }

    public void PlayHangarDoor()
    {
        AudioClip clip = effectsList[6].clip;
        Player.PlayOneShot(clip);
    }
}
