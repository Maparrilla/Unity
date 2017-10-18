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
	private Material selectedMaterial;
	private Material agentMaterial;
	private Material nazgulMaterial;
	private Material obstacleMaterial;

	private Ray shootRay;        // Ray cast from mouse position (at camera)
	private RaycastHit shootHit; // GameObject hit by shootRay

    private Camera cam1;
    private Camera cam2;

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

		selectedMaterial = Resources.Load ("B1Materials/agentselected") as Material;
		Debug.Log (selectedMaterial);
		agentMaterial = Resources.Load ("B1Materials/Agent") as Material;
		nazgulMaterial = Resources.Load ("B1Materials/Nazgul") as Material;
		obstacleMaterial = Resources.Load ("B1Materials/BlueCylinder") as Material;

        cam1 = Camera.main;
        cam2 = GameObject.FindGameObjectWithTag("Camera2").GetComponent<Camera>();

        cam1.enabled = true;
        cam2.enabled = false;
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
					deselectObstacle ();

					GameObject myObject = hit.transform.gameObject;
					myObject.GetComponent<MeshRenderer> ().material = selectedMaterial;
					selectedAgents.Add (hit.transform.gameObject.GetComponent<NavMeshAgent> ());
					Debug.Log ("Agent selected");
				}

				else if (hit.collider.CompareTag ("Obstacle"))
				{
					GameObject myObject = hit.transform.gameObject;
					myObject.GetComponent<MeshRenderer> ().material = selectedMaterial;
					selectedObstacle = hit.transform.gameObject.GetComponent<NavMeshObstacle> ();
					TargetObject = hit.transform;
					Debug.Log ("Obstacle selected");
				}

				else if (hit.collider.CompareTag ("Nazgul"))
				{
					deselectObstacle ();

					GameObject myObject = hit.transform.gameObject;
					myObject.GetComponent<MeshRenderer> ().material = selectedMaterial;
					selectedNazguls.Add (hit.transform.gameObject.GetComponent<NavMeshAgent> ());
					Debug.Log ("Nazgul selected");
				}

                else if (hit.collider.CompareTag("Vampire"))
                {
                    deselectObstacle();

                    GameObject myObject = hit.transform.gameObject;
                    VampireScript script = myObject.GetComponent<VampireScript>();
                    script.isSelected = true;

                    cam1.enabled = false;
                    cam2.enabled = true;
                }
					
				else // environment was selected
				{
					

					foreach (NavMeshAgent agent in selectedAgents)
					{
						agent.SendMessage ("moveTo", hit.point);
						agent.gameObject.GetComponent<MeshRenderer> ().material = agentMaterial;
					}
					selectedAgents.Clear (); // clear selection

					foreach (NavMeshAgent naz in selectedNazguls)
					{
						naz.SendMessage ("moveTo", hit.point);
						naz.gameObject.GetComponent<MeshRenderer> ().material = nazgulMaterial;
					}
					selectedNazguls.Clear ();
				}

			}
		}


		if (selectedObstacle != null)
		{
			float ObjectRotationSpeed = 5f;
			Vector3 movement = new Vector3(0,0,0);

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

	private void deselectObstacle()
	{
		if (selectedObstacle != null)
		{
			selectedObstacle.gameObject.GetComponent<MeshRenderer> ().material = obstacleMaterial;
			selectedObstacle = null;
		}
	}
}
