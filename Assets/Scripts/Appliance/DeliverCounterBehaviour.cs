using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DeliverCounterBehaviour : InteractableBehaviour
{
    public override void Place(PickableItemBehaviour dropped)
    {
        if (Analize(dropped))
        {
            dropped.DestroyItem();
        }
        else
        {
            Debug.Log("No puedes entregar " + dropped.gameObject.name);
        }
    }

    private bool Analize(PickableItemBehaviour delivered)
    {
        PlateBehaviour plate = delivered.GetComponent<PlateBehaviour>();
        if (plate && plate.HasItem() 
            && GameManagerBehaviour.TryDeliver(plate.GetItem()))
        {
            return true;
        }
        return false;
    }
}
