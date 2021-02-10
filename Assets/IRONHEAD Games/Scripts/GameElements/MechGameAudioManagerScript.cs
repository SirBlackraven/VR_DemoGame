using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechGameAudioManagerScript : MonoBehaviour
{
    public static MechGameAudioManagerScript Instance;

    // Start is called before the first frame update
    public AudioSource SeekerDeathEffect;
    public AudioSource MechWalkEffect;
    public AudioSource PlayerHitEffect;
    public AudioSource PlayerHealedEffect;
    public AudioSource HangarDoorEffect;
    public AudioSource PlayerMechDestroyed;
   public AudioSource BGMusic;

    public float mechWalkLoopDelay = 2.5f;
    private float currentWalkLoop = 0.0f;
    bool mechWalking = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mechWalking)
        {
            currentWalkLoop += Time.deltaTime;
            if (currentWalkLoop > mechWalkLoopDelay)
            {
                MechWalkEffect.PlayOneShot(MechWalkEffect.clip);
                currentWalkLoop = 0;
            }
        }
    }

    public void PlayHangarDoor()
    {
        HangarDoorEffect.PlayOneShot(HangarDoorEffect.clip);
    }

    public void PlaySeekerDeathSound()
    {
        if (SeekerDeathEffect != null)
        {
            SeekerDeathEffect.PlayOneShot(SeekerDeathEffect.clip);
        }
    }

    public void PlayMechWalking()
    {
        if(!mechWalking)
        {
            mechWalking = true;
            
            //MechWalkEffect.PlayOneShot(MechWalkEffect.clip);
        }
        
    }

    public void StopMechWalking()
    {
        mechWalking = false;
        MechWalkEffect.Stop();
    }

    public void PlayMechDestroyed()
    {
        DebugManagerScript.Instance.AddMessage("SFX played: player killed");
        PlayerMechDestroyed.PlayOneShot(PlayerMechDestroyed.clip);

    }

    public void PlayerHit()
    {
        this.PlayerHitEffect.PlayOneShot(PlayerHitEffect.clip);
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
