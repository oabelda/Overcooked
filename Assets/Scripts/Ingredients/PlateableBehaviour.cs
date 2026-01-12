using System;
using UnityEngine;

[RequireComponent(typeof(PickableItemBehaviour))]
public class PlateableBehaviour : MonoBehaviour, ICombinable
{
    PickableItemBehaviour thisPickable;

    void Start()
    {
        thisPickable = GetComponent<PickableItemBehaviour>();
    }

    public bool Combine(PickableItemBehaviour other, 
        IPickableParentBehaviour parent)
    {
        if (other.GetName() == "plate")
        {
            PlateBehaviour plate = other.GetComponent<PlateBehaviour>();

            return plate.Combine(thisPickable, parent);
        }
        return false;
    }
}
