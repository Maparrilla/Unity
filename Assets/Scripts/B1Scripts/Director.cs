using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/* Handles Mouse Input for 
 */
public class Director : MonoBehaviour {

	public float raycastDistance;

	private List<NavMeshAgent> selectedAgents; // Currently selected NavMeshAgents
	private List<NavMeshAgent> movingAgents;   // Currently moving NavMeshAgents
	private List<NavMeshAgent> selectedNazguls;

	private NavMeshObstacle selectedObstacle;
	private Transform TargetObject;
//	private Material selectedMaterial;
//	private Material agentMaterial;
//	private Material nazgulMaterial;
//	private Material obstacleMaterial;

	private Ray shootRay;        // Ray cast from mouse position (at camera)
	private RaycastHit shootHit; // GameObject hit by shootRay

	public enum KEYBOARD_INPUT : int
	{
		YAW_POS = KeyCode.RightArrow,
		YAW_NEG = KeyCode.LeftArrow,
		PITCH_POS = KeyCode.UpArrow,
		PITCH_NEG = KeyCode.DownArrow,
	}


	// Use this for initialization
	void Start () {
		selectedAgents = new List<NavMeshAgent> ();
		movingAgents = new List<NavMeshAgent> ();
		selectedNazguls = new List<NavMeshAgent> ();

//		selectedMaterial = Resources.Load ("Assets/Resources/B1Materials/PurpleWall") as Material;
//		agentMaterial = Resources.Load ("B1Materials/Agent") as Material;
//		nazgulMaterial = Resources.Load ("B1Materials/Nazgul") as Material;
//		obstacleMaterial = Resources.Load ("B1Materials/BlueCylinder") as Material;
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
//					GameObject myObject = hit.transform.gameObject;
//					myObject.GetComponent<MeshRenderer> ().material = selectedMaterial;
					selectedObstacle = null;
					selectedAgents.Add (hit.transform.gameObject.GetComponent<NavMeshAgent> ());
					Debug.Log ("Agent selected");
				}

				else if (hit.collider.CompareTag ("Obstacle"))
				{
					selectedObstacle = hit.transform.gameObject.GetComponent<NavMeshObstacle> ();
					TargetObject = hit.transform;
					Debug.Log ("Obstacle selected");
				}

				else if (hit.collider.CompareTag ("Nazgul"))
				{
					selectedObstacle = null;
					selectedNazguls.Add (hit.transform.gameObject.GetComponent<NavMeshAgent> ());
					Debug.Log ("Nazgul selected");
				}
					
				else // environment was selected
				{
					selectedObstacle = null;

					foreach (NavMeshAgent agent in selectedAgents)
					{
						agent.SendMessage ("moveTo", hit.point);
//						agent.destination = hit.point;
//						agent.Resume ();
//						movingAgents.Add (agent);
//						agent.gameObject.GetComponent<MeshRenderer> ().material = agentMaterial;
					}
					selectedAgents.Clear (); // clear selection

					foreach (NavMeshAgent naz in selectedNazguls)
					{
						naz.SendMessage ("moveTo", hit.point);
					}
					selectedNazguls.Clear ();
				}

			}
		}


		if (selectedObstacle != null)
		{
			float ObjectRotationSpeed = 5f;
			Vector3 movement = new Vector3(0,0,0);
//			float moveX = Input.GetAxis ("HorizontalArrow");
//			float moveZ = Input.GetAxis ("VerticalArrow");
//			Debug.Log (moveX);
//			movement = movement + new Vector3 (moveX, 0, moveZ);
//
////			Vector3.RotateTowards (movement, Camera.main.transform.forward, 2 * Mathf.PI, 0.0F);
//			movement.y = 0;
//			Debug.Log (movement);

			foreach (KEYBOARD_INPUT val in System.Enum.GetValues(typeof(KEYBOARD_INPUT)))
			{
				if (Input.GetKey ((KeyCode)val)) {
					switch (val) {
					case KEYBOARD_INPUT.YAW_POS:
//						TargetObject.Rotate (TargetObject.forward, -ObjectRotationSpeed * Time.deltaTime, Space.World);
						movement = new Vector3 (1.0f, 0.0f, 0.0f);
						break;
					case KEYBOARD_INPUT.YAW_NEG:
//						TargetObject.Rotate (TargetObject.forward, (ObjectRotationSpeed * Time.deltaTime), Space.World);
						movement = new Vector3 (-1.0f, 0.0f, 0.0f);
						break;
					case KEYBOARD_INPUT.PITCH_POS:
//						TargetObject.Rotate (TargetObject.right, ObjectRotationSpeed * Time.deltaTime, Space.World);
						movement = new Vector3 (0.0f, 0.0f, 1.0f);
						break;
					case KEYBOARD_INPUT.PITCH_NEG:
//						TargetObject.Rotate (TargetObject.right, -(ObjectRotationSpeed * Time.deltaTime), Space.World);
						movement = new Vector3 (0.0f, 0.0f, -1.0f);
						break;

					}
				}
			}
			movement = Camera.main.transform.TransformDirection (movement); // transform movement vector to correspond to camera angle
			selectedObstacle.gameObject.SendMessage ("applyForce", movement);
		}





	}
}
