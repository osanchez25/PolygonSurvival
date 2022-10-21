using UnityEngine;
using UnityEngine.AI;

namespace PolygonWar
{
	public class EnemyMovement : MonoBehaviour
	{

		public float KnockBackForce = 1.1f;
		public AudioClip[] WalkingClips;
		public float WalkingDelay = 0.4f;

		private NavMeshAgent _nav;
		private Transform _player;
		private EnemyHealth _enemyHealth;
		private AudioSource _walkingAudioSource;
		private Animator _animator;
		private float _time;
		



		void Start()
		{
			_nav = GetComponent<NavMeshAgent>();
			_player = GameObject.FindGameObjectWithTag("Player").transform;
			_enemyHealth = GetComponent<EnemyHealth>();
			SetupSound();
			_time = 0f;
			_animator = GetComponent<Animator>();
		}

		void Update()
		{
			_time += Time.deltaTime;
			if (_enemyHealth.Health > 0)
			{
				if (_nav != null)
				{
					if(!_animator.GetBool("IsNearPlayer"))
					{
						if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
						{
							//print("moviendo enemigo");
							_nav.SetDestination(_player.position);
							if (_time > WalkingDelay && _animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
							{
								PlayRandomFootstep();
								_time = 0f;
							}
						}
					}

				}

			}
			else
			{
				//print("agente desactivado");
				_walkingAudioSource.Stop();
				_nav.enabled = false;
			}
		}
		private void SetupSound()
		{
			_walkingAudioSource = gameObject.AddComponent<AudioSource>();
			_walkingAudioSource.volume = 0.2f;
		}

		private void PlayRandomFootstep()
		{
            int index = Random.Range(0, WalkingClips.Length);
            _walkingAudioSource.clip = WalkingClips[index];
            _walkingAudioSource.Play();
        }

		public void KnockBack()
		{
			//_animator.SetTrigger("knockBack");
			_nav.velocity = -transform.forward * KnockBackForce;
		}

		public void KnockBack(float KnockBackForce)
		{
			_nav.velocity = -transform.forward * KnockBackForce;
		}

		// plays our enemy's default victory state 
		public void PlayVictory()
		{
			_animator.SetTrigger("Idle");
		}

	}
}