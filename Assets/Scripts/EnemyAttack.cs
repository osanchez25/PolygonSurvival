using UnityEngine;
using System.Collections;

namespace PolygonWar
{
    public class EnemyAttack : MonoBehaviour
    {

        public FistCollider LeftFist;
        public FistCollider RightFist;
        public int enemyAttackTypes;
        public AudioClip[] AttackSfxClips;


        Animator _animator;
        GameObject _player;
        private AudioSource _audioSource;


        void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _animator = GetComponent<Animator>();
            SetupSound();

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _player)
            {
                
                _animator.SetBool("IsNearPlayer", true);
                _animator.SetInteger("AttackType", Random.Range(0, enemyAttackTypes));
                //               //compute vector (length one) from game object's pivot to the player's pivot:
                //               Vector3 hitVector = (other.gameObject.transform.position - transform.position).normalized;

                //               //if you want only horizontal plane movement, disable y-component of hitVector:
                //               hitVector = (other.gameObject.transform.position - transform.position);
                //               hitVector.y = 0;
                //               hitVector = hitVector.normalized;


                //other.GetComponent<Rigidbody>().AddForce(hitVector * 2000);
                
               // other.gameObject.GetComponent<Rigidbody>().AddForce(-other.gameObject.transform.forward * 2000);
                //other.gameObject.GetComponent<Rigidbody>().AddForce(other.gameObject.transform.up * 200);


            }
            //print("enter trigger with _player");

        }

       

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _player)
            {
                _animator.SetBool("IsNearPlayer", false);
            }
            //print("exit trigger with _player");
        }

        public void Attack()
        {
            //if (LeftFist.IsCollidingWithPlayer() || RightFist.IsCollidingWithPlayer())
            if(_animator.GetBool("IsNearPlayer"))
            {
                _player.GetComponent<PlayerHealth>().TakeDamage(10);
                //_player.GetComponent<Rigidbody>().AddForce(-_player.gameObject.transform.forward*5000);
                PlayRandomHit();
            }
        }
        private void SetupSound()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.volume = 0.6f;
        }

        private void PlayRandomHit()
        {
            int index = Random.Range(0, AttackSfxClips.Length);
            _audioSource.clip = AttackSfxClips[index];
            _audioSource.Play();
        }
    }
}