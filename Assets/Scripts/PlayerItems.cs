using FPSControllerLPFP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{


    public enum ItemType { Health,Ammo };
    public AudioClip HealthSfxClip;
    public ItemType itemType;
    public int ammoAmount;
    public int healthAmount;
    Animator _animator;
    GameObject _player;
    private AudioSource _audioSource;
    //bool destroy = false;
    


    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
        SetupSound();

    }

    //void Update()
    //{
    //    if (!_audioSource.isPlaying && destroy)
    //    {
    //        //Destroy(gameObject);
    //        gameObject.SetActive(false);
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player  )
        {
            if (itemType == ItemType.Health)
            {
                if (_player.GetComponent<PlayerHealth>()._currentHealth != _player.GetComponent<PlayerHealth>().maxHealth)
                {
                    _player.GetComponent<PlayerHealth>().HealPlayer(healthAmount);
                    GameObject.Find("Items").GetComponent<AudioSource>().Play();
                    gameObject.SetActive(false);
                }
            }

            if (itemType == ItemType.Ammo)
            {
                
                    GameObject.Find("arms_assault_rifle_01").GetComponent<AutomaticGunScriptLPFP>().StoredAmmo += ammoAmount;
                    GameObject.Find("Items").GetComponent<AudioSource>().Play();
                    gameObject.SetActive(false);

            }

            
        }
   
        //print("enter trigger with _player");
    }



    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject == _player)
    //    {
    //        _animator.SetBool("IsNearPlayer", false);
    //    }
    //    print("exit trigger with _player");
    //}

    private void SetupSound()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.9f;
    }
}
