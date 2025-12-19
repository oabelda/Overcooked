using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRespawnBehaviour : MonoBehaviour
{
    Vector3 initPosition;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = initPosition;
        transform.eulerAngles = new Vector3(0, 0, 0);
        rb.linearVelocity = new Vector3(0, 0, 0);
        rb.angularVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision c)
    {
        // Choco con "Deadzone"
        if (c.gameObject.name == "Deadzone")
        {
            Respawn();
        }
    }
}
