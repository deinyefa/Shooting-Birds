using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	private AudioSource audioSource;

	public float health = 70f;

	void Awake () {
		audioSource = GetComponent<AudioSource> ();
	}

	void OnCollisionEnter2D (Collider2D target) {
	
		if (target.gameObject.GetComponent<Rigidbody2D> () != null)
			return;

		float damage = target.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude * 10f;

		if (damage > 10f)
			audioSource.Play ();

		health -= damage;

		if (health <= 0)
			Destroy (gameObject);
	}
}