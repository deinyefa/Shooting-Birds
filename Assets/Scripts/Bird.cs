using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {

	public BirdState birdState { set; get;}

	private TrailRenderer trailRenderer;
	private Rigidbody2D rigidBody;
	private CircleCollider2D circleCollider;
	private AudioSource audioSource;

	void Awake () {
		
		InitializeVariables ();
	}
	
	void FixedUpdate () {
		if ((birdState == BirdState.Thrown) && (rigidBody.velocity.sqrMagnitude <= GameVariables.MinVelocity)) {
		
			StartCoroutine (DestroyAfterDelay (2f));
		}
	}

	void InitializeVariables () {
		
		trailRenderer = GetComponent<TrailRenderer> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();
		audioSource = GetComponent<AudioSource> ();

		trailRenderer.enabled = false;
		trailRenderer.sortingLayerName = "Foreground";

		rigidBody.isKinematic = true;
		circleCollider.radius = GameVariables.BirdColliderRadiusBig;

		birdState = BirdState.BeforeThrown;
	}

	public void OnThrow () {

		audioSource.Play ();
		trailRenderer.enabled = true;
		rigidBody.isKinematic = false;
		circleCollider.radius = GameVariables.BirdColliderRadiusNormal;
		birdState = BirdState.Thrown;
	}

	IEnumerator DestroyAfterDelay(float delay) {

		yield return new WaitForSeconds(delay);
		Destroy (gameObject);
	}
}
