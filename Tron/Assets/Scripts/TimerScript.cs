using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {
	float timeSeconds;
	float timeMinutes;
	// Use this for initialization
	void Start () {
		timeSeconds = 0;
		timeMinutes = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeSeconds += 1 * Time.deltaTime;
		if (timeSeconds >= 60) {
			timeSeconds = 0;
			timeMinutes++;
		}
		setTime ();
	}

	void setTime()
	{	
		int milliSec =  (int)((timeSeconds - (int) timeSeconds)*100);
		string seconds;
		if (timeSeconds < 10)
			seconds = "0" + (int)timeSeconds;
		else
			seconds = "" + (int)timeSeconds;
		string milliSeconds;
		if (milliSec < 10)
			milliSeconds = "0" + milliSec;
		else
			milliSeconds = "" + milliSec;

		Text field = gameObject.GetComponent<Text> ();
		field.text = timeMinutes + ":" + seconds + ":" + milliSeconds;
	}
}
