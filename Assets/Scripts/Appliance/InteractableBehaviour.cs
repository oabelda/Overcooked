using System;
using System.Collections;
using UnityEngine;

public class InteractableBehaviour : MonoBehaviour, IPickableParentBehaviour
{
    protected PickableItemBehaviour placedItem;
    [SerializeField] Transform itemPlacePoint;

    int focusCount;

    public event Action<bool> OnFocusChange;

    protected virtual void Start()
    {
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
        OnFocusChange?.Invoke(focusCount > 0);
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
        for (int index = 0; index < aC.Length; ++index) 
        {
            // If combine a with b is posible
            if (aC[index].Combine(b, parent)) return;
        }

        ICombinable[] bC = b.GetComponents<ICombinable>();
        for (int index = 0; index < bC.Length; ++index)
        {
            // If combine b with a is posible
            if (bC[index].Combine(a, parent)) return;
        }
    }
}
