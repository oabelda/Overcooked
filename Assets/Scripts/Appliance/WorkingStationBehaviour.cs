using UnityEngine;

public class WorkingStationBehaviour : InteractableBehaviour, IProcessor
{
    ChopIngredientBehaviour choppableItem;

    event PickableEvent OnItemPlaced;
    event FloatEvent OnItemProcessed;

    public override void Interact(PlayerInteractBehaviour player, bool isFirst)
    {
        if (choppableItem != null)
        {
            choppableItem.Process(Time.deltaTime);

            if (OnItemProcessed != null && choppableItem != null)
                OnItemProcessed(choppableItem.GetProcess());
        }
    }

    public void RegisterOnItemPlaced(PickableEvent a)
    {
        OnItemPlaced += a;
    }

    public void RegisterOnItemProcessed(FloatEvent function)
    {
        OnItemProcessed += function;
    }

    public override void SetItem(PickableItemBehaviour item)
    {
        base.SetItem(item);
        choppableItem = placedItem?.GetComponent<ChopIngredientBehaviour>();
        if (OnItemPlaced != null)
            OnItemPlaced(choppableItem);
    }
}
