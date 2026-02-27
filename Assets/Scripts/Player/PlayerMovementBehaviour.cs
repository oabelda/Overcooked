using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerMovementBehaviour : MonoBehaviour
{
    // Speed atributes
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Move(Vector3 moveDirection)
    {
        // Actually move the "body"
        transform.position = transform.position +
            moveDirection * movementSpeed * Time.deltaTime;
        
        // If needed, change the direction to look at
        // == !=
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            animator.SetBool("Walk", true);
            LookAt(moveDirection);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    void LookAt(Vector3 lookDirection)
    {
        Quaternion targetRotation;
        Quaternion newRotation;

        // Find the target rotation given the look direction
        targetRotation = Quaternion.LookRotation(lookDirection);

        // Interpolate the actual rotation and the target direction
        newRotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
            );

        // Rotate
        transform.rotation = newRotation;
    }
}
