using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientBoxCrazyBehaviour : InteractableBehaviour
{
    [SerializeField] PickableItemBehaviour[] ingredients;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    public override void Interact(PlayerInteractBehaviour player, bool isFirst)
    {
        if (!isFirst || placedItem != null || player.HasItem()) 
            return;

        CreateIngredient(player);
    }

    private void CreateIngredient(PlayerInteractBehaviour player)
    {
        PickableItemBehaviour newIngredient;

        int randomIndex;

        randomIndex = Random.Range(0,ingredients.Length);

        PickableItemBehaviour randomIngredient = ingredients[randomIndex];

        newIngredient = GameObject.Instantiate(randomIngredient);

        newIngredient.SetParent(player);
    }
}
