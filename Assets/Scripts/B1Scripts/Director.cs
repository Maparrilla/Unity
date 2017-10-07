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
	private List<NavMeshObstacle> selectedNazguls;

	public NavMeshObstacle nazzy;

	private Ray shootRay;        // Ray cast from mouse position (at camera)
	private RaycastHit shootHit; // GameObject hit by shootRay


	// Use this for initialization
	void Start () {
		selectedAgents = new List<NavMeshAgent> ();
		movingAgents = new List<NavMeshAgent> ();
		selectedObstacles = new List<NavMeshObstacle> ();
		selectedNazguls = new List<NavMeshObstacle> ();
	}

	// Update is called once per frame
	void Update () {

		// Handle I/O Pointer Events
		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
			//Camera.main.ScreenPointToRay(Input.mousePosition);
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

				else if (hit.collider.CompareTag ("Nazgul"))
				{
					selectedNazguls.Add (hit.transform.gameObject.GetComponent<NavMeshObstacle> ());
					Debug.Log ("Nazgul selected");
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

					foreach (NavMeshObstacle naz in selectedNazguls)
					{
						naz.SendMessage ("moveTo", hit.point);
					}
					selectedNazguls.Clear ();
				}

			}
		}




		// To avoid bunching, check each agent's destination.
		// If an agent's destination has another near-stationary agent in it,
		// retarget the destination to a slightly closer one.
		// (B1 - Refined Navigation: 1)
		Vector3 offset_y = new Vector3 (0.0f, 1.0f, 0.0f); // check presence of other agents at an offset height of 1
		foreach (NavMeshAgent agent in movingAgents)
		{
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





	}
}
