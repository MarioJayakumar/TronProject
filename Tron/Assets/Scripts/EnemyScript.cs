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
			timeSinceLastBoost += Time.deltaTime;

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
		transform.position = spawnPos;
		lastTurn = transform.position;
		createBeam (transform.position, direction);

		stopBoost ();
	}

	//this method will decide how the enemy moves around 
	private void decideMovement()
	{
		timePassed = 0;
		//first thing to be checked is if there is an obstruction in front of the bike
		RaycastHit hit;
		if (Physics.Raycast (transform.position + direction, direction, 5)) {
			//returns true if there is an obstruction
			int randResult = Random.Range (1, 100);
			//40% chance turning right, 40% chance turning left, 20% chance jumping
			if (randResult < 40) {
				//check if right is clear
				Vector3 r = Vector3.Cross(direction, new Vector3(0,1,0));
				if (Physics.Raycast (transform.position + r, r, 5)) {
					//there is an obstruction right side

					Vector3 l = Vector3.Cross(direction, new Vector3(0,-1,0));
					if (Physics.Raycast (transform.position + l, l, 5)) {
						//there is an obstruction left side, so just jump
						jump();
					} else
						turnLeft ();
				}
				else
					//no obstruction on right side so turn 
					turnRight();
			} else if (randResult < 80) {
				//check if left is clear
				Vector3 l = Vector3.Cross(direction, new Vector3(0,-1,0));
				if (Physics.Raycast (transform.position + l, l, 5)) {
					//there is an obstruction left side
					Vector3 r = Vector3.Cross(direction, new Vector3(0,1,0));
					if (Physics.Raycast (transform.position + r, r, 5)) {
						//obstruction right side so just jump
						jump ();
					} else
						turnRight ();
				}
				else
					turnLeft();
			} else {
				jump ();
			}
		} else {
			//if there is nothing in front of the bike, then we can randomly decide to turn or jump or boost
			int randResult = Random.Range(1, 1000);
			if (randResult < 960) {
				//DOING NOTHING
			} else if (randResult < 970) {
				//turn left
				Vector3 l = Vector3.Cross(direction, new Vector3(0,-1,0));
				if (!(Physics.Raycast (transform.position + l, l, 5))) {
					direction = Vector3.Cross (direction, new Vector3 (0, -1, 0));
					lastTurn = transform.position;
					createBeam (lastTurn, direction);
				}
			} else if (randResult < 980) {
				//turn right
				Vector3 r = Vector3.Cross(direction, new Vector3(0,1,0));
				if (!(Physics.Raycast (transform.position + r, r, 5))) {
					direction = Vector3.Cross (direction, new Vector3 (0, 1, 0));
					lastTurn = transform.position;
					createBeam (lastTurn, direction);
				}
			} else if (randResult < 990) {
				//jump
				jump();
			} else {
				//boost
				if (Physics.Raycast(transform.position + direction, direction, 20)){
					if (boostCounter == -1) {
						boostCounter = 0;
						timeSinceLastBoost = 0;
						boostSystem.Play ();
					}
				}
			}
		}
	}

	private void turnRight()
	{
		direction = Vector3.Cross (direction, new Vector3(0, 1, 0));
		lastTurn = transform.position;
		createBeam (lastTurn, direction);
	}

	private void turnLeft()
	{
		direction = Vector3.Cross (direction, new Vector3 (0, -1, 0));
		lastTurn = transform.position;
		createBeam (lastTurn, direction);
	}
}
