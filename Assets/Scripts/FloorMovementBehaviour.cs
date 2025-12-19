using UnityEngine;
using UnityEngine.InputSystem;

public class FloorMovementBehaviour : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x + rotationSpeed * Time.deltaTime,
                transform.eulerAngles.y,
                transform.eulerAngles.z);
        }

        if (Keyboard.current.sKey.isPressed)
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x - rotationSpeed * Time.deltaTime,
                transform.eulerAngles.y,
                transform.eulerAngles.z);
        }

        if (Keyboard.current.aKey.isPressed)
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                transform.eulerAngles.z + rotationSpeed * Time.deltaTime);
        }

        if (Keyboard.current.dKey.isPressed)
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                transform.eulerAngles.z - rotationSpeed * Time.deltaTime);
        }
    }
}
