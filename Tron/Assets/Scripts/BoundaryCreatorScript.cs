using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCreatorScript : MonoBehaviour {
	public Rigidbody generalBoundary;
	private Rigidbody topBoundary;
	private Rigidbody rightBoundary;
	private Rigidbody leftBoundary;
	private Rigidbody bottomBoundary;
	// Use this for initialization
	void Start () {
		float xLength = GetComponent<Collider> ().bounds.size.x;
		float zLength = GetComponent<Collider> ().bounds.size.z;

		generalBoundary.transform.localScale = transform.localScale * 10;

		topBoundary = (Rigidbody)Instantiate (generalBoundary, 
			new Vector3(transform.position.x +  xLength/2 + 5, transform.position.y, transform.position.z), 
			new Quaternion());
		topBoundary.transform.Rotate (Vector3.forward, 90, Space.Self);
		topBoundary.GetComponent<MeshRenderer> ().enabled = false;

		bottomBoundary = (Rigidbody)Instantiate (generalBoundary, 
			new Vector3(transform.position.x -  xLength/2 - 5, transform.position.y, transform.position.z), 
			new Quaternion());
		bottomBoundary.transform.Rotate (Vector3.forward, -90, Space.Self);
		bottomBoundary.GetComponent<MeshRenderer> ().enabled = false;

		rightBoundary = (Rigidbody)Instantiate (generalBoundary, 
			new Vector3(transform.position.x, transform.position.y, transform.position.z - zLength/2 - 5), 
			new Quaternion());
		rightBoundary.transform.Rotate (Vector3.right, 90, Space.Self);
		rightBoundary.GetComponent<MeshRenderer> ().enabled = false;

		leftBoundary = (Rigidbody)Instantiate (generalBoundary, 
			new Vector3(transform.position.x, transform.position.y, transform.position.z + zLength/2 + 5), 
			new Quaternion());
		leftBoundary.transform.Rotate (Vector3.right, -90, Space.Self);
		leftBoundary.GetComponent<MeshRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
