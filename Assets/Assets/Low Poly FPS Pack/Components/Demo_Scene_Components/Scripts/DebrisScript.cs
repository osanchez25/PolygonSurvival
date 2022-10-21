using UnityEngine;
using System.Collections;

public class DebrisScript : MonoBehaviour {

	[Header("Audio")]
	public AudioClip[] debrisSounds;
	public AudioSource audioSource;

	public float destroyAfter = 5f;

    private void Start()
    {
		//Start destroy timer
		StartCoroutine(DestroyAfter());
	}

    //If the debris collides with anything
    private void OnCollisionEnter (Collision collision) {
		//Play the random sound if the collision speed is high enough
		if (collision.relativeVelocity.magnitude > 50) 
		{
			//Get a random debris sound from the array every collision
			audioSource.clip = debrisSounds
				[Random.Range (0, debrisSounds.Length)];
			//Play the random debris sound
			audioSource.Play ();
		}
	}

	private IEnumerator DestroyAfter()
	{
		//Wait for set amount of time
		yield return new WaitForSeconds(destroyAfter);
		//Destroy bullet object
		Destroy(gameObject);
	}
}