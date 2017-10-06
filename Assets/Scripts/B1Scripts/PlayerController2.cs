using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{

    public enum KEYBOARD_INPUT : int
    {
        YAW_POS = KeyCode.RightArrow,
        YAW_NEG = KeyCode.LeftArrow,
        PITCH_POS = KeyCode.UpArrow,
        PITCH_NEG = KeyCode.DownArrow,
        
    }

    public Transform TargetObject;
    [Range(1f, 1000f)]
    public float ObjectRotationSpeed = 5f;
    public float moveSpeed = 10;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(0,0,0);
        foreach (KEYBOARD_INPUT val in System.Enum.GetValues(typeof(KEYBOARD_INPUT)))
        {
            if (Input.GetKey((KeyCode)val))
            {
                switch (val)
                {
                    case KEYBOARD_INPUT.YAW_POS:
                        TargetObject.Rotate(TargetObject.forward, -ObjectRotationSpeed * Time.deltaTime, Space.World);
                        movement = new Vector3(1.0f, 0.0f, 0.0f);
                        break;
                    case KEYBOARD_INPUT.YAW_NEG:
                        TargetObject.Rotate(TargetObject.forward, (ObjectRotationSpeed * Time.deltaTime), Space.World);
                        movement = new Vector3(-1.0f, 0.0f, 0.0f);
                        break;
                    case KEYBOARD_INPUT.PITCH_POS:
                        TargetObject.Rotate(TargetObject.right, ObjectRotationSpeed * Time.deltaTime, Space.World);
                        movement = new Vector3(0.0f, 0.0f, 1.0f);
                        break;
                    case KEYBOARD_INPUT.PITCH_NEG:
                        TargetObject.Rotate(TargetObject.right, -(ObjectRotationSpeed * Time.deltaTime), Space.World);
                        movement = new Vector3(0.0f, 0.0f, -1.0f);
                        break;
                    
                }
            }
        }
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * moveSpeed);
    }
}