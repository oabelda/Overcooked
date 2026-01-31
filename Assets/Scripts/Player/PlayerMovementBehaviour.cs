using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerMovementBehaviour : MonoBehaviour
{
    // Speed atributes
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;

    // Control Movement Keyboard Keys
    [SerializeField] Key fowardKey;
    [SerializeField] Key backwardKey;
    [SerializeField] Key leftKey;
    [SerializeField] Key rightKey;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveDirection;

        // Calculate the direction based on the inputs
        moveDirection = CalculateMoveDirection();

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

    Vector3 CalculateMoveDirection()
    {
        Vector3 moveVector;
        Vector3 moveNormalized;

        moveVector = new Vector3(0, 0, 0);

        // Ckeck inputs

        // Keyboard.current[Key.W].isPressed
        // Keyboard.current.wKey.isPressed
        if (Keyboard.current[fowardKey].isPressed)
        {
            moveVector.z = moveVector.z + 1;
        }
        if (Keyboard.current[backwardKey].isPressed)
        {
            moveVector.z = moveVector.z - 1;
        }
        if (Keyboard.current[leftKey].isPressed)
        {
            moveVector.x = moveVector.x - 1;
        }
        if (Keyboard.current[rightKey].isPressed)
        {
            moveVector.x = moveVector.x + 1;
        }

        // Normalize value (magnitude became one)
        moveNormalized = moveVector.normalized;

        return moveNormalized;
    }
}
