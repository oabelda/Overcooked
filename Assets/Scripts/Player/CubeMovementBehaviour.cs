using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubeMovementBehaviour : MonoBehaviour
{
    [SerializeField]float speed; // Atributos
    [SerializeField]float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
    } // End Start

    // Update is called once per frame
    void Update()
    {
        // speed = 0.05f; Variable local

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position 
                + (transform.right * speed) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                transform.eulerAngles.y + rotationSpeed * Time.deltaTime,
                transform.eulerAngles.z);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                transform.eulerAngles.y - rotationSpeed * Time.deltaTime,
                transform.eulerAngles.z);
        }

        // speed = 0.2f;

        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position 
                - (transform.right * speed) * Time.deltaTime;

            //transform.position = new Vector3(transform.position.x - speed,
            //   transform.position.y,
            //   transform.position.z);
        }

        if (Input.GetKey(KeyCode.W))
        {
            MoveForward();
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position 
                - (transform.forward * speed) * Time.deltaTime;

            //transform.position = new Vector3(transform.position.x,
            //    transform.position.y,
            //    transform.position.z - speed);
        }
    } // End Update

    // Function that moves the player avatar in the
    // direction of the looking
    public void MoveForward()
    {
        // Change the transform using the forward direction
        transform.position = transform.position
            + (transform.forward * speed) * Time.deltaTime;
    }

}
