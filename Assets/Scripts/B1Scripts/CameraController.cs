using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public enum KEYBOARD_INPUT: int {
        RIGHT = KeyCode.D,
        LEFT = KeyCode.A,
        FORWARD = KeyCode.W,
        BACK = KeyCode.S
    }

    public int moveSpeed;
    public int scrollSpeed;

    public float sensitivity;

    private Camera myCam;
    private Transform g_Camera;

	// Use this for initialization
	void Start () {
        moveSpeed = 2;
        scrollSpeed = 10;

        sensitivity = 0.02f;

        myCam = GetComponent<Camera>();
        g_Camera = myCam.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 vp = myCam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, myCam.nearClipPlane));
        vp.x -= 0.5f;
        vp.y -= 0.5f;
        vp.x *= sensitivity;
        vp.y *= sensitivity;
        vp.x += 0.5f;
        vp.y += 0.5f;
        Vector3 sp = myCam.ViewportToScreenPoint(vp);

        Vector3 v = myCam.ScreenToWorldPoint(sp);
        transform.LookAt(v, Vector3.up);

        foreach (KEYBOARD_INPUT val in System.Enum.GetValues(typeof(KEYBOARD_INPUT)))
        {
            if(Input.GetKey((KeyCode)val))
            {
                switch (val)
                {
                    case KEYBOARD_INPUT.FORWARD:
                        g_Camera.Translate(0, 0, 1 * moveSpeed * Time.deltaTime, Space.Self);
                        break;
                    case KEYBOARD_INPUT.BACK:
                        g_Camera.Translate(0, 0, -1 * moveSpeed * Time.deltaTime, Space.Self);
                        break;
                    case KEYBOARD_INPUT.LEFT:
                        g_Camera.Translate(-1 * moveSpeed * Time.deltaTime, 0, 0, Space.Self);
                        break;
                    case KEYBOARD_INPUT.RIGHT:
                        g_Camera.Translate(1 * moveSpeed * Time.deltaTime, 0, 0, Space.Self);
                        break;
                }
            }
        }
	}
}
