using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralized controller for game sound effects that the player hears. 
/// Most of these are attached to the Mech's dashboard so they are within audible range
/// Note: Sound sounds are connected to the emitting object so they will fade over distance
/// </summary>
public class MechGameAudioManagerScript : MonoBehaviour
{
    public static MechGameAudioManagerScript Instance;

    //catalog of sounds
    public AudioSource SeekerDeathEffect;
    public AudioSource MechWalkEffect;
    public AudioSource PlayerHitEffect;
    public AudioSource PlayerHealedEffect;
    public AudioSource HangarDoorEffect;
    public AudioSource PlayerMechDestroyed;
    public AudioSource BGMusic;                  //background music

    //special vars to control the playback of the mech walk
    public float mechWalkLoopDelay = 2.5f;      //how long to play the footstep SFX
    private float currentWalkLoop = 0.0f;       //play time of the current 
    bool mechWalking = false;                   //on-off flag for mech walk SFX

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(mechWalking)
        {
            //Keep playing the footstep soundeffect
            //TODO: modulate this based on the throttle setting
            currentWalkLoop += Time.deltaTime;
            if (currentWalkLoop > mechWalkLoopDelay)
            {
                MechWalkEffect.PlayOneShot(MechWalkEffect.clip);
                currentWalkLoop = 0;
            }
        }
    }

    /// <summary>
    /// Start the mech walking loop
    /// </summary>
    public void PlayMechWalking()
    {
        if (!mechWalking)
        {
            mechWalking = true;
        }
    }

    /// <summary>
    /// Calls for individual sounds
    /// </summary>
    public void PlayHangarDoor()
    {
        if (HangarDoorEffect != null)
        {
            HangarDoorEffect.PlayOneShot(HangarDoorEffect.clip);
        }
    }

    public void StopMechWalking()
    {
        mechWalking = false;
        MechWalkEffect.Stop();
    }

    public void PlaySeekerDeathSound()
    {
        if (SeekerDeathEffect != null)
        {
            SeekerDeathEffect.PlayOneShot(SeekerDeathEffect.clip);
        }
    }

    public void PlayMechDestroyed()
    {
        if(PlayerMechDestroyed != null)
        {
            PlayerMechDestroyed.PlayOneShot(PlayerMechDestroyed.clip);
        } 
    }

    public void PlayerHit()
    {
        if(PlayerHitEffect != null)
        {
            this.PlayerHitEffect.PlayOneShot(PlayerHitEffect.clip);
        }        
    }

    public void PlayHealed()
    {
        if(this.PlayerHealedEffect != null)
        {
            PlayerHealedEffect.PlayOneShot(PlayerHealedEffect.clip);
        }
    }

    public void PlayMusic()
    {
        if(this.BGMusic != null)
        {
            DebugManagerScript.Instance.AddMessage("Playing bg music");
            BGMusic.PlayOneShot(BGMusic.clip);
        }
    }

    public void StopMusic()
    {
        if (this.BGMusic != null)
        {
            BGMusic.Stop();
        }
    }
}
