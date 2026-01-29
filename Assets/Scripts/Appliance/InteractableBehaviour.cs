using System.Collections;
using UnityEngine;

public class InteractableBehaviour : MonoBehaviour, IPickableParentBehaviour
{
    protected PickableItemBehaviour placedItem;
    [SerializeField] Transform itemPlacePoint;

    Color initColor;
    [SerializeField] Color activeColor;
    [SerializeField] new MeshRenderer renderer;
    int focusCount;

    protected virtual void Start()
    {
        if (renderer == null)
        {
            renderer = GetComponent<MeshRenderer>();
        }
        initColor = renderer.material.color;
        focusCount = 0;
    }

    public virtual void Interact(PlayerInteractBehaviour player, bool isFirst)
    {
        Debug.Log("You are interacting with " + this.gameObject.name);
    }

    public void Focus(bool newFocus)
    {
        if (newFocus)
        {
            focusCount = focusCount + 1;
        }
        else
        {
            focusCount = focusCount - 1;
        }

        if (renderer.material.color == initColor ||
            renderer.material.color == activeColor)
        {
            ResetColor();
        }
    }

    protected void SetPlayerColor(Color givenColor)
    {
        renderer.material.color= givenColor;
    }

    protected void ResetColor()
    {
        if (focusCount > 0) // Is Focus
        {
            renderer.material.color = activeColor;
        }
        else // Is not focus
        {
            renderer.material.color = initColor;
        }
    }

    public virtual PickableItemBehaviour Take()
    {
        if (placedItem == null)
        {
            return null;
        }

        else
        {
            return placedItem;
        }
    }

    public virtual void Place(PickableItemBehaviour dropped)
    {
        if (!HasItem())
        {
            dropped.SetParent(this);
        }
        else
        {
            // Try to combine
            Combine(placedItem,dropped,this);
        }
    }

    public virtual void SetItem(PickableItemBehaviour item)
    {
        placedItem = item;
    }

    public bool HasItem()
    {
        return placedItem != null;
    }

    public Transform GetPlaceholder()
    {
        return itemPlacePoint;
    }

    public PickableItemBehaviour GetItem()
    {
        return placedItem;
    }

    private void Combine(PickableItemBehaviour a, 
        PickableItemBehaviour b,
        IPickableParentBehaviour parent)
    {
        ICombinable[] aC = a.GetComponents<ICombinable>();
        ICombinable[] bC = b.GetComponents<ICombinable>();

        int index;
        index = 0;
        while (index < aC.Length)
        {
            ICombinable actual;
            actual = aC[index];

            if (actual.Combine(b, parent))
            {
                // He podido combinar
                return;
            }
            else
            {
                // No he podido combinar
            }

            index = index + 1;
        }
        index = 0;

        while (index < bC.Length)
        {
            ICombinable actual;
            actual = bC[index];
            if (actual.Combine(a, parent))
            {
                // He podido combinar
                return;
            }
            else
            {
                // No he podido combinar
            }

            index = index + 1;
        }
    }
}
