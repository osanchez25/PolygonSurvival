using System.Collections;
using UnityEngine;


namespace PolygonWar
{
    public class EnemyHealth : MonoBehaviour
    {
        public float Health = 10;
        public AudioClip[] HitSfxClips;
        public float HitSoundDelay = 0.5f;
        public AudioClip HeadShotSfxClip;

        private SpawnManager _spawnManager;
        private Animator _animator;
        private AudioSource _audioSource;
        private AudioSource _audioSource2;
        private float _hitTime;
        public float destroyAfter = 5f;

        void Start()
        {
            _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
            _animator = GetComponent<Animator>();
            _hitTime = 0f;
            SetupSound();
        }

        void Update()
        {
            _hitTime += Time.deltaTime;
        }

        public void TakeDamage(float damage)
        {
            if (Health <= 0) { 
                return; 
            }

            Health -= damage;
            
            if (_hitTime > HitSoundDelay)
            {
                PlayRandomHit();
                _hitTime = 0;
            }

            if (Health <= 0)
            {
                Death();
            }
        }

        public void HeadShot()
        {
           // print("HEadShot");

            if (Health <= 0)
            {
                return;
            }
            Health = 0;
            Death();
            _audioSource2.clip = HeadShotSfxClip;
            //_audioSource.volume = 5f;
            _audioSource2.Play();
            

        }

        private void SetupSound()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.volume = 0.6f;
            _audioSource2 = gameObject.AddComponent<AudioSource>();
            _audioSource2.volume = 1f;
        }

        private void PlayRandomHit()
        {
            //Debug.Log("PLAY HURT SOUND");
            int index = Random.Range(0, HitSfxClips.Length);
            _audioSource.clip = HitSfxClips[index];
            _audioSource.Play();
        }

        private void Death()
        {
            _animator.SetTrigger("Death");
            _spawnManager.EnemyDefeated();
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }
            //Desactivar objecto del minimapa
            transform.Find("MiniMap Enemy").gameObject.SetActive(false);
            
            //Start destroy timer
            StartCoroutine(DestroyAfter());
        }

        private IEnumerator DestroyAfter()
        {
            //Wait for set amount of time
            yield return new WaitForSeconds(destroyAfter);
            //Destroy bullet object
            Destroy(gameObject);
        }

    }
}