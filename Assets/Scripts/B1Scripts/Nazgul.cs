using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nazgul : MonoBehaviour {

	private bool moving;
	private NavMeshAgent navMeshAgent;
	private NavMeshObstacle navMeshObstacle;

	// Use this for initialization
	void Start () {
		navMeshObstacle = GetComponent<NavMeshObstacle> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
		{
			Debug.Log ("yeah");
			navMeshObstacle.enabled = false; // temporarily disable navMeshObstacle
			navMeshAgent.enabled = true;
			transform.position = navMeshAgent.nextPosition;

			if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
			{
				navMeshAgent.isStopped = true;
				moving = false;
			}

//			navMeshAgent.enabled = false;
			navMeshObstacle.enabled = true;
		}
	}

	public void moveTo(Vector3 dest)
	{
		Debug.Log ("HEL");
		
		navMeshObstacle.enabled = false;
		navMeshAgent.enabled = true;
		navMeshAgent.updatePosition = false;
		navMeshAgent.destination = dest;
		navMeshAgent.enabled = false;
		navMeshObstacle.enabled = true;
		moving = true;
	}
}
