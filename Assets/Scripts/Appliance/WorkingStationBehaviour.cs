using UnityEngine;

public class WorkingStationBehaviour : InteractableBehaviour
{
    ChopIngredientBehaviour choppableItem;

    public override void Interact(PlayerInteractBehaviour player, bool isFirst)
    {
        if (choppableItem != null)
        {
            choppableItem.Process(Time.deltaTime);
        }
    }

    public override void SetItem(PickableItemBehaviour item)
    {
        base.SetItem(item);
        choppableItem = placedItem?.GetComponent<ChopIngredientBehaviour>();
    }
}
