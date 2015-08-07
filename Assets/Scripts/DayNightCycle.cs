using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

	public float speed = 1f;
	public float x;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		x += speed;
		gameObject.transform.rotation = new Quaternion(x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
		//gameObject.transform.rotation.Set (x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
	
	}
}
