using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeScript : MonoBehaviour
{
	public float speed;
	protected Vector3 direction;
	private float timeKey = 0.1f;
	protected float timePassed;
	protected float jumpPassed;
	protected float fallPassed;
	protected float respawnTimer;
	protected float boostCounter;
	protected float timeSinceLastBoost;
	protected float xLength;
	protected float zLength;
	protected bool jumping;
	protected bool falling;
	protected bool alive;
	public Rigidbody beamBody;
	protected Rigidbody beam;
	protected Vector3 lastTurn;
	List<Rigidbody> beamList;
	public Material generatedBeamMat;
	public ParticleSystem explosionSystem;
	public ParticleSystem boostSystem;
	public Vector3 spawnPos;

	// Use this for initialization
	protected void Start ()
	{
		transform.position = spawnPos;
		alive = true;
		direction = new Vector3 (1, 0, 0);
		//direction = direction * speed;
		//gameObject.GetComponent<Rigidbody> ().velocity = speed * direction;
		timePassed = 0f;
		jumpPassed = 0f;
		fallPassed = 0f;
		respawnTimer = 0f;
		boostCounter = -1;
		timeSinceLastBoost = 100;
		jumping = false;
		falling = false;
		lastTurn = transform.position;
		beamList = new List<Rigidbody> ();
		createBeam (transform.position, direction);
		explosionSystem.Stop ();
		boostSystem.Stop ();

		xLength = GetComponent<Collider> ().bounds.size.x;
		zLength = GetComponent<Collider> ().bounds.size.z;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (alive) {
			gameObject.GetComponent<Rigidbody> ().velocity = speed * direction;
			transform.rotation = Quaternion.LookRotation (direction);

			//editing the light beam
			if (!jumping || !falling) {
				if (direction.Equals (new Vector3 (0, 0, 1))) {
					Vector3 toScale = beam.transform.localScale;
					toScale.z = Vector3.Distance (lastTurn, transform.position);
					beam.transform.localScale = toScale;
					Vector3 newPos = beam.transform.position;
					newPos.z = transform.position.z + -1 * beam.transform.localScale.z / 2;
					newPos.y = transform.position.y;
					beam.transform.position = newPos;
				}
				if (direction.Equals (new Vector3 (0, 0, -1))) {
					Vector3 toScale = beam.transform.localScale;
					toScale.z = -1 * Vector3.Distance (lastTurn, transform.position);
					beam.transform.localScale = toScale;
					Vector3 newPos = beam.transform.position;
					newPos.z = transform.position.z + -1 * beam.transform.localScale.z / 2;
					newPos.y = transform.position.y;
					beam.transform.position = newPos;		

				}
				if (direction.Equals (new Vector3 (1, 0, 0))) {
					Vector3 toScale = beam.transform.localScale;
					toScale.x = Vector3.Distance (lastTurn, transform.position);
					beam.transform.localScale = toScale;
					Vector3 newPos = beam.transform.position;
					newPos.x = transform.position.x + -1 * beam.transform.localScale.x / 2;
					newPos.y = transform.position.y;
					beam.transform.position = newPos;	

				}
				if (direction.Equals (new Vector3 (-1, 0, 0))) {
					Vector3 toScale = beam.transform.localScale;
					toScale.x = -1 * Vector3.Distance (lastTurn, transform.position);
					beam.transform.localScale = toScale;
					Vector3 newPos = beam.transform.position;
					newPos.x = transform.position.x + -1 * beam.transform.localScale.x / 2;
					newPos.y = transform.position.y;
					beam.transform.position = newPos;	

				}

			}

	

			timePassed += Time.deltaTime;
			timeSinceLastBoost += Time.deltaTime;

			if (Input.GetKey ("a") && timePassed > timeKey) {
				//gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				if (!(direction.Equals (new Vector3 (0, 0, -1))) && !(direction.Equals (new Vector3 (0, 0, 1)))) {
					direction = new Vector3 (0, 0, 1);
					timePassed = 0;
					lastTurn = transform.position;
					createBeam (lastTurn, direction);

				}
			}
			if (Input.GetKey ("d") && timePassed > timeKey) {
				//gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				if (!(direction.Equals (new Vector3 (0, 0, 1))) && !(direction.Equals (new Vector3 (0, 0, -1)))) {
					direction = new Vector3 (0, 0, -1);
					timePassed = 0;
					lastTurn = transform.position;
					createBeam (lastTurn, direction);
				}
			}
			if (Input.GetKey ("w") && timePassed > timeKey) {
				if (!(direction.Equals (new Vector3 (-1, 0, 0))) && !(direction.Equals (new Vector3 (1, 0, 0)))) {
					direction = new Vector3 (1, 0, 0);
					timePassed = 0;
					lastTurn = transform.position;
					createBeam (lastTurn, direction);
				}
			}
			if (Input.GetKey ("s") && timePassed > timeKey) {
				if (!(direction.Equals (new Vector3 (1, 0, 0))) && !(direction.Equals (new Vector3 (-1, 0, 0)))) {
					direction = new Vector3 (-1, 0, 0);
					timePassed = 0;
					lastTurn = transform.position;
					createBeam (lastTurn, direction);

				}
			}
			if (Input.GetKey ("space") ) {
				jump ();
			}

			if (Input.GetKey ("e") && boostCounter == -1 && timeSinceLastBoost > 12) {
				boostCounter = 0;
				boostSystem.Play ();
				timeSinceLastBoost = 0;
			}
			if (jumping) {
				jumpPassed++;
				if (jumpPassed < 17) {
					transform.position += new Vector3 (0, 0.75f / jumpPassed, 0);
					if (jumpPassed % 3 == 0) {
						lastTurn = transform.position;
						createBeam (transform.position, direction);
					}
				} else {
					jumping = false;
					falling = true;
				}
			}
			if (falling) {
				jumpPassed++;
				fallPassed++;
				if (jumpPassed < 34) {
					transform.position -= new Vector3 (0, 0.75f / (17 - fallPassed), 0);
					if (fallPassed % 3 == 0) {
						lastTurn = transform.position;
						createBeam (transform.position, direction);
					}
				} else {
					falling = false;
					lastTurn = transform.position;
					createBeam (transform.position, direction);
				}
			}
			//boost counter is -1 if not boosting
			if (boostCounter > -1)
				boost ();
			

			//basically checking for collision
			rayCastHandler ();
		} else {
			//stuff to do while respawning
			respawnTimer += Time.deltaTime;

			if (respawnTimer >= 5) {
				respawn ();
			}
		}

	}

	protected void createBeam (Vector3 spawn, Vector3 dir)
	{
		
		//generating a new beam
		beam = (Rigidbody)Instantiate (beamBody, spawn, new Quaternion ());
		beam.GetComponent<Renderer> ().material = generatedBeamMat;
		//	beam.transform.LookAt (transform);
		Vector3 sc = beam.transform.localScale;
		sc = new Vector3 (1f, 2f, 1f);
		if (jumping || falling)
			sc = new Vector3 (0.75f, 2f, 0.5f);
		beam.transform.localScale = sc;
		beamList.Add (beam);
	}

	protected void rayCastHandler ()
	{
		RaycastHit hit;
		//will check for collision by creating a ray and seeing if it intersects with the beam
		//raycast should be slightly above the player
		Vector3 startingPoint = transform.position;
		startingPoint.y += 1;
		if (Physics.Raycast (startingPoint + direction , direction, out hit, 1)) {
			alive = false;
			gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0,0,0);
			crash ();
		} 
	}

	protected void crash()
	{
		ParticleSystemRenderer pRend = explosionSystem.GetComponent<ParticleSystemRenderer> ();
		pRend.material = beamList [0].gameObject.GetComponent<Renderer> ().material;
		explosionSystem.transform.position = GetComponent<Rigidbody> ().position;
		for (int i = 0; i < beamList.Count; i++)
		{
			Rigidbody destroyed = beamList [i];
			Destroy (destroyed.gameObject);
		}
		beamList.Clear();

		explosionSystem.Clear ();
		explosionSystem.Play ();
		gameObject.GetComponent<MeshRenderer> ().enabled = false;

		boostSystem.Clear ();
		boostSystem.Stop ();

	}

	protected void respawn()
	{
		respawnTimer = 0;
		alive = true;
		gameObject.GetComponent<MeshRenderer> ().enabled = true;

		direction = new Vector3 (1,0,0);
		transform.position = spawnPos;
		lastTurn = transform.position;
		createBeam (transform.position, direction);

		stopBoost ();
	}

	//this method is used for the voice command
	public void voiceBoost()
	{
		if (boostCounter == -1 && timeSinceLastBoost > 12)
		{
			boostCounter = 0;
			boostSystem.Play ();
			timeSinceLastBoost = 0;
		}
	}

	protected void boost()
	{
		
		boostCounter+= Time.deltaTime;
		if (boostCounter > 5) {
			stopBoost ();
		} else {
			speed = 50;
			boostSystem.transform.position = GetComponent <Rigidbody> ().position;
			boostSystem.transform.rotation = Quaternion.LookRotation (-direction);
		}
	}

	protected void stopBoost()
	{
		boostCounter = -1;
		speed = 20;
		boostSystem.Clear ();
		boostSystem.Stop ();
	}
		

	public void jump()
	{
		if (!jumping && !falling) {
			//gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3(0,2000,0));
			jumping = true;
			jumpPassed = 0;
			fallPassed = 0;
			lastTurn = transform.position;
			createBeam (lastTurn, direction);
		}
	}

	public void turnRight()
	{
		if (timePassed > timeKey) {
			direction = Vector3.Cross (direction, new Vector3 (0, 1, 0));
			lastTurn = transform.position;
			createBeam (lastTurn, direction);
		}
	}

	public void turnLeft()
	{
		if (timePassed > timeKey) {
			direction = Vector3.Cross (direction, new Vector3 (0, -1, 0));
			lastTurn = transform.position;
			createBeam (lastTurn, direction);
		}
	}
}
