using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PickableItemBehaviour))]
public class CombineIngredientBehaviour : MonoBehaviour, ICombinable
{
    [SerializeField] PickableItemBehaviour other;
    [SerializeField] PickableItemBehaviour result;

    public bool Combine(PickableItemBehaviour other, IPickableParentBehaviour parent)
    {
        if (this.other.GetName() == other.GetName())
        {
            // Combine
            PickableItemBehaviour newIngredient;
            newIngredient = GameObject.Instantiate(result);

            this.GetComponent<PickableItemBehaviour>().DestroyItem();
            other.DestroyItem();

            newIngredient.SetParent(parent);

            return true;
        }
        else
        {
            return false;
        }
    }
}
