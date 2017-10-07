using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nazgul : MonoBehaviour {

//	private bool moving;
	private NavMeshAgent navMeshAgent;
	private GameObject stench;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		navMeshAgent.isStopped = true;
		stench = GameObject.Find ("NazztyStench");
		offset = new Vector3 (0.0f, 0.6f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {

		if (!navMeshAgent.isStopped)
			stench.transform.position = transform.position + offset;
		
//		if (moving)
//		{
//			Debug.Log ("yeah");
//			navMeshObstacle.enabled = false; // temporarily disable navMeshObstacle
//			navMeshAgent.enabled = true;
//			transform.position = navMeshAgent.nextPosition;
//
//			if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
//			{
//				navMeshAgent.isStopped = true;
//				moving = false;
//			}
//
////			navMeshAgent.enabled = false;
//			navMeshObstacle.enabled = true;
//		}
	}

	public void moveTo(Vector3 dest)
	{
		Debug.Log ("HEL");
		navMeshAgent.destination = dest;
		navMeshAgent.isStopped = false;

//		navMeshObstacle.enabled = false;
//		navMeshAgent.enabled = true;
//		navMeshAgent.updatePosition = false;
//		navMeshAgent.destination = dest;
//		navMeshAgent.enabled = false;
//		navMeshObstacle.enabled = true;
//		moving = true;
	}
}
