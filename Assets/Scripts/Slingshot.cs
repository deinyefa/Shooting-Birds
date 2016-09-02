using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {

	private Vector3 slingshotMiddleVector;

	[HideInInspector]
	public SlingshotState slingShotState;

	public Transform leftSlingShotOrgin, rightSlingShotOrigin;

	public LineRenderer slingShotLineRenderer1, slingShotLineRenderer2, trajectoryLineRenderer;

	[HideInInspector]
	public GameObject birdToThrow;

	public Transform birdWaitPosition;

	public float throwSpeed;

	[HideInInspector]
	public float timeSinceThrown;

	public delegate void BirdThrown ();
	public event BirdThrown birdThrown;


	void Awake () {
		slingShotLineRenderer1.sortingLayerName = "Foreground";
		slingShotLineRenderer2.sortingLayerName = "Foreground";
		trajectoryLineRenderer.sortingLayerName = "Foreground";

		slingShotState = SlingshotState.Idle;
		slingShotLineRenderer1.SetPosition (0, leftSlingShotOrgin.position);
		slingShotLineRenderer2.SetPosition (0, rightSlingShotOrigin.position);

		slingshotMiddleVector = new Vector3 ((leftSlingShotOrgin.position.x + rightSlingShotOrigin.position.x) / 2, 
											(leftSlingShotOrgin.position.y + rightSlingShotOrigin.position.y) / 2, 0);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void InitiaizeBird () {
		birdToThrow.transform.position = birdWaitPosition.position;
		slingShotState = SlingshotState.Idle;
		SetSlingshotLineRenderersActive (true);
	}

	void SetSlingshotLineRenderersActive (bool active) {
	
		slingShotLineRenderer1.enabled = active;
		slingShotLineRenderer2.enabled = active;

	}

	void DisplaySlingshotLineRenderers () {

		slingShotLineRenderer1.SetPosition (1, birdToThrow.transform.position);
		slingShotLineRenderer2.SetPosition (1, birdToThrow.transform.position);

	}

	void SetTrajectoryLineRendererActive (bool active) {
	
		trajectoryLineRenderer.enabled = active;
	}

	void DisplayTrajectoryLineRenderer (float dist) {

		SetTrajectoryLineRendererActive (true);

		Vector3 v2 = slingshotMiddleVector - birdToThrow.transform.position;
		int segmentCount = 15;

		Vector2[] segments = new Vector2[segmentCount];

		segments [0] = birdToThrow.transform.position;

		Vector2 segVelocity = new Vector2 (v2.x, v2.y) * throwSpeed * dist;

		//- calculate time that trajectory needs to get to its path
		for (int i = 1; i < segmentCount; i++) {
		
			float time = i * Time.fixedDeltaTime * 5f;
			segments [i] = segments [0] + segVelocity * time + 0.5f * Physics2D.gravity * Mathf.Pow (time, 2);
		}

		//- sets traj line rend pos, forst being bird throw pos, 
		//- as long as the bird moves, it keeps on drawing
		trajectoryLineRenderer.SetVertexCount (segmentCount);
		for (int i = 0; i < segmentCount; i++) {
		
			trajectoryLineRenderer.SetPosition (i, segments [i]);
		}
	}

	private void ThrowBird (float distance) {

		Vector3 velocity = slingshotMiddleVector - birdToThrow.transform.position;

		birdToThrow.GetComponent<Bird> ().OnThrow ();

		birdToThrow.GetComponent<Rigidbody2D> ().velocity = new Vector2 (velocity.x, velocity.y) * throwSpeed * distance;

		if (birdThrown != null)
			birdThrown ();
	}
}
