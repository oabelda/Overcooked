using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MicrowaveBehaviour : InteractableBehaviour, IProcessor
{
    Animator anim;

    bool isCooking;
    [SerializeField] float cookingTime;

    event PickableEvent OnItemPlaced;
    event FloatEvent OnItemProcessed;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        isCooking = false;
    }

    public override void Interact(PlayerInteractBehaviour player, bool isFirst)
    {
        if (isFirst && !isCooking)
        {
            StartCoroutine(Cook());
        }
    }

    public override PickableItemBehaviour Take()
    {
        if ( isCooking)
        {
            return null;
        }

        return base.Take();
    }

    IEnumerator Cook()
    {
        // Begin
        isCooking = true;

        anim.SetBool("cooking", true);

        // If the placedItem can be cook
        if (placedItem != null)
        {
            CookIngredientBehaviour cookableItem;
            cookableItem = placedItem.gameObject.GetComponent<CookIngredientBehaviour>();
            if (cookableItem != null)
            {
                yield return new WaitForSeconds(cookingTime);
                // Cook the ingredient
                cookableItem.Complete(); // Option 1
                // placedIngredient.Cook(placedIngredient.GetRemainTime()); // Option 2
            }
            else
            {
                // There is no CookIngredient
                Debug.Log("Se quema la cocina");
                yield return new WaitForSeconds(cookingTime);
            }
        }
        else
        {
            // There is no placedItem
            Debug.Log("Se quema la cocina");
            yield return new WaitForSeconds(cookingTime);
        }

        // End
        isCooking = false;
        anim.SetBool("cooking", false);
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
