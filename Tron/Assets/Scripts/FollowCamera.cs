using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
	public GameObject target;
	public float damping = 1f;
	Vector3 offset;

	void Start() {
		//offset = target.transform.position - transform.position;
		//transform.LookAt (target.transform);
			
	}

	void LateUpdate() {
		/*float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		transform.position = target.transform.position - (rotation * offset);
	//	if (currentAngle == desiredAngle) {
			transform.position -= target.GetComponent<Rigidbody> ().velocity / 15 + new Vector3 (0, 5, 0);
	//	}
		transform.LookAt(target.transform);
		//transform.position = target.transform.position - target.GetComponent<Rigidbody> ().velocity / 2;*/

	//	transform.position = target.transform.position - target.GetComponent<Rigidbody> ().velocity / 2 + new Vector3 (0 , 1.5f , 0);
		Vector3 tempPos = transform.position;
		tempPos.x = target.transform.position.x -87;
		tempPos.z = 5*target.transform.position.z / 3;
		tempPos.y = 70 + target.transform.position.x / 5;
		transform.position = tempPos;
		transform.LookAt (target.transform);
	}


}