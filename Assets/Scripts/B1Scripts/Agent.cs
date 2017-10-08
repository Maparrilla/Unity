﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour {

	private NavMeshAgent agent;
	private Vector3 offset_y = new Vector3 (0.0f, 1.0f, 0.0f); // check presence of other agents at an offset height of 1

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere (agent.destination + offset_y, 0.1f);

		// Check overlapping colliders
		for (int i = 0; i < hitColliders.Length; i++)
		{
			NavMeshAgent other = hitColliders [i].gameObject.GetComponent<NavMeshAgent> ();

			// If there is a near-stationary agent occupying the destination
			if (other != null && other != agent && other.velocity.magnitude < 0.1f) {
				Debug.Log ("Rerouting " + agent.gameObject.name);

				// Redirect to a destination just short of the old one.
				Vector3 opposite = agent.destination - agent.gameObject.transform.position;
				agent.destination = agent.destination - (0.8f * opposite.normalized);
				agent.Resume ();

			}
		}
	}

	public void moveTo(Vector3 dest)
	{
		agent.destination = dest;
		agent.isStopped = false;
	}
}
