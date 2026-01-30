using UnityEngine;

public class CookingStationBehaviour : InteractableBehaviour, IProcessor
{
    [SerializeField] float cookingPower;

    CookIngredientBehaviour cookableItem;

    event PickableEvent OnItemPlaced;
    event FloatEvent OnItemProcessed;

    protected override void Start()
    {
        base.Start();
        this.enabled = false;
    }

    private void Update()
    {
        cookableItem.Process(Time.deltaTime
            * cookingPower);

        if (OnItemProcessed != null && cookableItem != null)
            OnItemProcessed(cookableItem.GetProcess());
    }

    public override void SetItem(PickableItemBehaviour item)
    {
        base.SetItem(item);
        cookableItem = placedItem?.GetComponent<CookIngredientBehaviour>();
        ActivateFire(); // this.enable = cookableItem != null;
        if (OnItemPlaced != null)
            OnItemPlaced(cookableItem);
    }

    private void ActivateFire()
    {
        if (cookableItem != null)
        {
            this.enabled = true;
            SetPlayerColor(Color.aquamarine);
        }
        else
        {
            this.enabled = false;
            ResetColor();
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
}
