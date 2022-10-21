using UnityEngine;


namespace PolygonWar
{
	public class PlayerShootingController : MonoBehaviour
	{
		public float Range = 100;
		public float ShootingDelay = 0.1f;
		public AudioClip ShotSfxClips;



		private Camera _camera;
		private ParticleSystem _particle;
		private LayerMask _shootableMask;
		private LineRenderer laserLine;
		public Transform gunEnd; // Holds a reference to the gun end object, marking the muzzle location of the gun

		private float _timer;
		private AudioSource _audioSource;

		void Start()
		{
			_camera = Camera.main;
			_particle = GetComponentInChildren<ParticleSystem>();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			_shootableMask = LayerMask.GetMask("Shootable");
			laserLine = GetComponentInChildren<LineRenderer>();
			_timer = 0;
			SetupSound();


		}

		void Update()
		{
			_timer += Time.deltaTime;
			if (Input.GetMouseButton(0) && _timer >= ShootingDelay)
			{
				Shoot();
			}
			else if (!Input.GetMouseButton(0))
			{
				_audioSource.Stop();
			}




		}

		private void Shoot()
		{
			_timer = 0;
			// Create a vector at the center of our camera's viewport
			Vector3 rayOrigin = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
			RaycastHit hit = new RaycastHit();
			_audioSource.Play();

			if (Physics.Raycast(rayOrigin, _camera.transform.forward, out hit, Range, _shootableMask))
			{
				print("hit " + hit.collider.gameObject);
				_particle.Play();

				EnemyHealth health = hit.collider.GetComponent<EnemyHealth>();

				EnemyMovement enemyMovement = hit.collider.GetComponent<EnemyMovement>();
				if (enemyMovement != null)
				{
					enemyMovement.KnockBack();
				}

				if (health != null)
				{
					health.TakeDamage(1);
				}
			}
		}

		private void SetupSound()
		{
			_audioSource = gameObject.AddComponent <AudioSource> ();
			_audioSource.volume = 0.2f;
			_audioSource.clip = ShotSfxClips;
		}


	}
}




//private void Shoot()
//{
//	_timer = 0;
//	Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
//	RaycastHit hit = new RaycastHit();

//	if (Physics.Raycast(ray, out hit, Range, _shootableMask))
//	{
//		print("hit " + hit.collider.gameObject);
//		_particle.Play();

//		EnemyHealth health = hit.collider.GetComponent<EnemyHealth>();
//		if (health != null)
//		{
//			health.TakeDamage(1);
//		}
//	}
//}


/*
			if (Input.GetMouseButton(0))
			{
				//Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
				// Create a vector at the center of our camera's viewport
				Vector3 rayOrigin = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
				RaycastHit hit = new RaycastHit();

				// Set the start position for our visual effect for our laser to the position of gunEnd
				laserLine.SetPosition(0, gunEnd.position);
				
				laserLine.SetPosition(1, hit.point);

				laserLine.SetPosition(1, rayOrigin + (_camera.transform.forward * Range));

				Debug.DrawLine(rayOrigin, _camera.transform.forward * Range, Color.green, 1);


				if (Physics.Raycast(rayOrigin,_camera.transform.forward, out hit, Range, _shootableMask))
				{

					//Debug.DrawLine(rayOrigin, _camera.transform.forward, Color.green, 2);
					print("hit " + hit.collider.gameObject);
					_particle.Play();

					EnemyHealth health = hit.collider.GetComponent<EnemyHealth>();
					if (health != null)
					{
						health.TakeDamage(1);
					}
				}
			}

			*/
