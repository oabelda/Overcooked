using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputs", menuName = "PlayerInputs")]
public class PlayerInputsSO : ScriptableObject
{
    [SerializeField] Key forwardKey;
    [SerializeField] Key backwardKey;
    [SerializeField] Key leftKey;
    [SerializeField] Key rightKey;
    [SerializeField] Key interactKey;
    [SerializeField] Key interact2Key;

    public bool IsForwardPressed()
    {
        return Keyboard.current[forwardKey].isPressed;
    }

    public bool IsBackwardsPressed()
    {
        return Keyboard.current[backwardKey].isPressed;
    }

    public bool IsLeftPressed()
    {
        return Keyboard.current[leftKey].isPressed;
    }

    public bool IsRightPressed()
    {
        return Keyboard.current[rightKey].isPressed;
    }

    public bool IsInteractPressed()
    {
        return Keyboard.current[interactKey].isPressed;
    }

    public bool WasInteractPressedThisFrame()
    {
        return Keyboard.current[interactKey].wasPressedThisFrame;
    }

    public bool IsInteract2KeyPressed()
    {
        return Keyboard.current[interact2Key].isPressed;
    }

    public bool WasInteract2KeyPressedThisFrame()
    {
        return Keyboard.current[interact2Key].wasPressedThisFrame;
    }
}
