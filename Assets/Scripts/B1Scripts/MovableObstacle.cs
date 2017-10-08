using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacle : MonoBehaviour {

	private Rigidbody rb;
	private bool selected;
	private float speed;

	// Use this for initialization
	void Start () {
		speed = 10.0f;
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

	void applyForce (Vector3 movement)
	{
		rb.AddForce (movement * speed);
	}
}
