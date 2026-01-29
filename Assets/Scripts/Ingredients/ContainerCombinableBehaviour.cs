using System;
using UnityEngine;

[RequireComponent(typeof(PickableItemBehaviour))]
public class ContainerCombinableBehaviour : MonoBehaviour, ICombinable
{
    [SerializeField] ToppingClass[] combinables;
    PickableItemBehaviour thisItem;

    void Start()
    {
        thisItem = GetComponent<PickableItemBehaviour>();
    }

    public bool Combine(PickableItemBehaviour other, IPickableParentBehaviour parent)
    {
        int index;

        for (index = 0; index < combinables.Length; index = index + 1)
        {
            ToppingClass combinable;
            combinable = combinables[index];

            if (other.IsInstanceOf(combinable.GetItem())
                && !combinable.IsActive())
            {
                // It can be combine with 'other'
                DoCombine(combinable, other, parent);
                return true;
            }
        }

        return false;
    }

    private void DoCombine(ToppingClass topping, PickableItemBehaviour other, 
        IPickableParentBehaviour parent)
    {
        // Activate the entity
        topping.Activate();

        // Destroy the 'other'
        other.DestroyItem();

        // Set the parent
        thisItem.SetParent(parent);
    }

    public bool CheckToppings(Order order)
    {
        for (int index = 0; index < combinables.Length; ++index)
        {
            if (this.combinables[index].IsActive() 
                != order.GetRequiredTopping(index))
            {
                return false;
            }
        }
        return true;
    }

    public int GetToppingsCount()
    {
        return combinables.Length;
    }

    public string GetToppingName(int index)
    {
        return combinables[index].GetItem().gameObject.name;
    }

}

[Serializable]
public class ToppingClass
{
    [SerializeField] PickableItemBehaviour item;
    [SerializeField] GameObject entity;

    public PickableItemBehaviour GetItem()
    {
        return item;
    }

    public void Activate()
    {
        entity.SetActive(true);
    }

    public bool IsActive()
    {
        return entity.activeSelf;
    }
}
