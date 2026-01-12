using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PickableItemBehaviour))]
public class CombineIngredientBehaviour : MonoBehaviour
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

            this.GetComponent<PickableItemBehaviour>().ClearParent();
            other.ClearParent();

            newIngredient.SetParent(parent);

            return true;
        }
        else
        {
            return false;
        }
    }
}
