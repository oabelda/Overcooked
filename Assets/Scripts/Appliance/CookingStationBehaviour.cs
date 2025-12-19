using UnityEngine;

public class CookingStationBehaviour : InteractableBehaviour
{
    [SerializeField] float cookingPower;

    CookIngredientBehaviour cookableItem;

    protected override void Start()
    {
        base.Start();
        this.enabled = false;
    }

    private void Update()
    {
        cookableItem.Process(Time.deltaTime
            * cookingPower);
    }

    public override void SetItem(PickableItemBehaviour item)
    {
        base.SetItem(item);
        cookableItem = placedItem?.GetComponent<CookIngredientBehaviour>();
        ActivateFire(); // this.enable = cookableItem != null;
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
}
