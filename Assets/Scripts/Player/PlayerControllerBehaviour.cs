using UnityEngine;

[RequireComponent(typeof(PlayerMovementBehaviour))]
[RequireComponent(typeof(PlayerInteractBehaviour))]
public class PlayerControllerBehaviour : MonoBehaviour
{
    [SerializeField] PlayerInputsSO inputs;

    PlayerMovementBehaviour movement;
    PlayerInteractBehaviour interact;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PlayerMovementBehaviour>();
        interact = GetComponent<PlayerInteractBehaviour>();

        GameObject playersManagerGO = GameObject.FindGameObjectWithTag("PlayersManager");
        PlayersManagerBehaviour playersManager = playersManagerGO.GetComponent<PlayersManagerBehaviour>();

        GameManagerBehaviour.AddNewPlayer();

        if (!playersManager.GetSlot(this))
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement.Move(CalculateMoveDirection());

        if (inputs.IsInteractPressed())
        {
            interact.Interact(false);
        }
        if (inputs.WasInteractPressedThisFrame())
        {
            interact.Interact(true);
        }
        if (inputs.WasInteract2KeyPressedThisFrame())
        {
            interact.PickOrDrop();
        }
    }

    Vector3 CalculateMoveDirection()
    {
        Vector3 moveVector;
        Vector3 moveNormalized;

        moveVector = new Vector3(0, 0, 0);

        // Ckeck inputs
        // Keyboard.current[Key.W].isPressed
        // Keyboard.current.wKey.isPressed
        if (inputs.IsForwardPressed())
        {
            moveVector.z = moveVector.z + 1;
        }
        if (inputs.IsBackwardsPressed())
        {
            moveVector.z = moveVector.z - 1;
        }
        if (inputs.IsLeftPressed())
        {
            moveVector.x = moveVector.x - 1;
        }
        if (inputs.IsRightPressed())
        {
            moveVector.x = moveVector.x + 1;
        }

        // Normalize value (magnitude became one)
        moveNormalized = moveVector.normalized;

        return moveNormalized;
    }


    public void SetInputs(PlayerInputsSO inputs)
    {
        this.inputs = inputs;
    }

    void OnDestroy()
    {
        GameManagerBehaviour.RemovePlayer();
    }
}
