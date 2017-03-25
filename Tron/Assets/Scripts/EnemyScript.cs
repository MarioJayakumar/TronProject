using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : BikeScript
{

	public float turnTick;
	void Start ()
	{
		base.Start ();
		direction = new Vector3 (-1, 0, 0);
	}

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

			//can only make a new movement every turnTick ticks
			if (timePassed > turnTick) {
				decideMovement ();
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

	protected void respawn ()
	{
		respawnTimer = 0;
		alive = true;
		gameObject.GetComponent<MeshRenderer> ().enabled = true;

		direction = new Vector3 (-1, 0, 0);
		transform.position = new Vector3 (75, 1, -50);
		lastTurn = transform.position;
		createBeam (transform.position, direction);
	}

	//this method will decide how the enemy moves around 
	private void decideMovement()
	{
		//first thing to be checked is if there is an obstruction in front of the bike
		RaycastHit hit;
		if (Physics.Raycast (transform.position + direction, direction, 5)) {
			//returns true if there is an obstruction
			int randResult = Random.Range (1, 100);
			//40% chance turning right, 40% chance turning left, 20% chance jumping
			if (randResult < 40) {
				//turn right
				direction = Vector3.Cross (direction, new Vector3 (0, 1, 0));
				lastTurn = transform.position;
				createBeam (lastTurn, direction);
			} else if (randResult < 80) {
				//turn left	
				direction = Vector3.Cross (direction, new Vector3 (0, -1, 0));
				lastTurn = transform.position;
				createBeam (lastTurn, direction);
			} else {
				jump ();
			}
		} else {
			//if there is nothing in front of the bike, then we can randomly decide to turn or jump or boost
			// 80% chance of doing nothing, 7% turn left, 7% turh right, 4% jump, 2% boost
			int randResult = Random.Range(1, 5000);
			if (randResult < 4960) {
				//DOING NOTHING
			} else if (randResult < 4980) {
				//turn left
				direction = Vector3.Cross (direction, new Vector3 (0, -1, 0));
				lastTurn = transform.position;
				createBeam (lastTurn, direction);
			} else if (randResult < 4990) {
				//turn right
				direction = Vector3.Cross (direction, new Vector3 (0, 1, 0));
				lastTurn = transform.position;
				createBeam (lastTurn, direction);
			} else if (randResult < 5000) {
				//jump
				jump();
			} else {
				//boost
				if (boostCounter == -1)
					boostCounter = 0;

			}
		}
	}
}
