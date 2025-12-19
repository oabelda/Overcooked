using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientBoxBehaviour : InteractableBehaviour
{
    [SerializeField] PickableItemBehaviour ingredient;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    public override void Interact(PlayerInteractBehaviour player, bool isFirst)
    {
        if (!isFirst || placedItem != null || player.HasItem()) 
            // @TODO combine
            return;

        CreateIngredient(player);
    }

    private void CreateIngredient(PlayerInteractBehaviour player)
    {
        PickableItemBehaviour newIngredient;
        // ingredient
        newIngredient = GameObject.Instantiate(ingredient);
        newIngredient.SetParent(player);
    }
}
