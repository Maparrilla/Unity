using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Handles Mouse Input for 
 */
public class Director : MonoBehaviour {

	public float raycastDistance;

	private List<NavMeshAgent> selectedAgents; // Currently selected NavMeshAgents
	private List<NavMeshAgent> movingAgents;   // Currently moving NavMeshAgents
	private List<NavMeshObstacle> selectedObstacles;
	private List<NavMeshAgent> selectedNazguls;
	private Ray shootRay;        // Ray cast from mouse position (at camera)
	private RaycastHit shootHit; // GameObject hit by shootRay


	// Use this for initialization
	void Start () {
		selectedAgents = new List<NavMeshAgent> ();
		movingAgents = new List<NavMeshAgent> ();
		selectedObstacles = new List<NavMeshObstacle> ();
		selectedNazguls = new List<NavMeshAgent> ();
	}

	// Update is called once per frame
	void Update () {

		// Handle I/O Pointer Events
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Input.GetButtonDown ("Fire1"))
		{
			if (Physics.Raycast (ray, out hit, raycastDistance))
			{
				if (hit.collider.CompareTag ("Agent"))
				{
					selectedAgents.Add (hit.transform.gameObject.GetComponent<NavMeshAgent> ());
					Debug.Log ("Agent selected");
				}

				else if (hit.collider.CompareTag ("Obstacle"))
				{
					selectedObstacles.Add (hit.transform.gameObject.GetComponent<NavMeshObstacle> ());
					Debug.Log ("Obstacle selected");
				}

				else // environment was selected
				{
					foreach (NavMeshAgent agent in selectedAgents)
					{
						agent.destination = hit.point;
						agent.Resume ();
						movingAgents.Add (agent);
					}
					selectedAgents.Clear (); // clear selection

					foreach (NavMeshObstacle obstacle in selectedObstacles)
					{
						// move the obstacle
					}
					selectedObstacles.Clear (); // clear selection
				}

			}
		}


	}
}
