using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Animation))]
public class IngredientBoxBehaviour : InteractableBehaviour
{
    [SerializeField] PickableItemBehaviour ingredient;
    Animation anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animation>();
        GetComponentInChildren<Image>().sprite = ingredient.GetSprite();
    }

    public override void Interact(PlayerInteractBehaviour player, bool isFirst)
    {
        if (!isFirst || placedItem != null || player.HasItem()) 
            // @TODO combine
            return;

        CreateIngredient().SetParent(player);
    }

    public override PickableItemBehaviour Take()
    {
        return base.Take() ?? CreateIngredient();
    }


    private PickableItemBehaviour CreateIngredient()
    {
        anim.Play();
        return GameObject.Instantiate(ingredient);
    }
}
