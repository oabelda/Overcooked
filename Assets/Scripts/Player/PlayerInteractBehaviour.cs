using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractBehaviour : MonoBehaviour, IPickableParentBehaviour
{
    Color playerColor;
    InteractableBehaviour activeAppliance;
    [SerializeField] Key interactKey;
    [SerializeField] Key pickDropKey;

    PickableItemBehaviour carriedIngredient;

    [SerializeField] Transform handsPosition;
    [SerializeField] Transform interactOrigin;

    [SerializeField] float interactionDistance;

    void Update()
    {
        HandleFocus();
        if (activeAppliance != null)
        {
            if (Keyboard.current[interactKey].isPressed)
            {
                activeAppliance.Interact(this, false);
            }
            if (Keyboard.current[interactKey].wasPressedThisFrame)
            {
                activeAppliance.Interact(this, true);
            }
            if (Keyboard.current[pickDropKey].wasPressedThisFrame)
            {
                PickOrDrop();
            }
        }
    }

    void HandleFocus()
    {
        RaycastHit hitCollider;

        if (Physics.Raycast(interactOrigin.position,
            this.transform.forward, 
            out hitCollider ,
            interactionDistance))
        {
            InteractableBehaviour interactuable;
            interactuable =
                hitCollider.transform.gameObject
                .GetComponent<InteractableBehaviour>();

            if (interactuable != activeAppliance)
            {
                // Something new is been pointed
                if (activeAppliance != null)
                {
                    // if there is a previous active, UnFocus it before
                    activeAppliance.Focus(false);
                }

                activeAppliance = interactuable;

                if (activeAppliance != null)
                {
                    // Focus to the new active appliance
                    activeAppliance.Focus(true);
                }

            }
        }

        else
        {
            if (activeAppliance != null)
            {
                activeAppliance.Focus(false);
                activeAppliance = null;
            }
        }
    }

    void PickOrDrop()
    {
        if (HasItem())
        {
            // Try to place
            activeAppliance.Place(carriedIngredient);
        }
        else
        {
            // Try to take
            activeAppliance.Take()?.SetParent(this);

            //PickableItemBehaviour item;
            //item = activeAppliance.Take();

            //if (item != null)
            //{
            //    item.SetParent(this);
            //}
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(interactOrigin.position,
            interactOrigin.position
                +this.transform.forward*interactionDistance);
    }

    public void SetItem(PickableItemBehaviour item)
    {
        carriedIngredient = item;
    }

    public bool HasItem()
    {
        return carriedIngredient != null;
    }

    public Transform GetPlaceholder()
    {
        return handsPosition;
    }

    public PickableItemBehaviour GetItem()
    {
        return carriedIngredient;
    }
}
