using UnityEngine;

[RequireComponent(typeof(PickableItemBehaviour))]
public class PlateBehaviour : MonoBehaviour, IPickableParentBehaviour, ICombinable
{
    PickableItemBehaviour food;
    [SerializeField] Transform itemPlacePoint;

    PickableItemBehaviour platePickable;

    void Start()
    {
        platePickable = GetComponent<PickableItemBehaviour>();
        platePickable.SetParent(this.transform.parent?.GetComponentInParent<IPickableParentBehaviour>());
    }

    public bool Combine(PickableItemBehaviour other, IPickableParentBehaviour parent)
    {
        if (!this.HasItem())
        {
            // The plate is empty, try to plate the 'other'
            if (other.GetComponent<PlateableBehaviour>())
            {
                other.SetParent(this);
                platePickable.SetParent(parent);
                return true;
            }
        }
        else
        {
            // El plato ya tiene algo
            if (AuxCombine(this.GetItem(), other, this)
                || AuxCombine(other, this.GetItem(), this))
            {
                platePickable.SetParent(parent);
                return true;
            }
        }
        return false;
    }

    private bool AuxCombine(PickableItemBehaviour a,
    PickableItemBehaviour b, IPickableParentBehaviour parent)
    {
        ICombinable[] combinables = a.gameObject.GetComponents<ICombinable>();

        int index;
        for (index = 0; index < combinables.Length; index = index + 1)
        {
            ICombinable actual;
            actual = combinables[index];

            if (actual.Combine(b, parent))
            {
                return true;
            }
        }

        return false;
    }

    public PickableItemBehaviour GetItem()
    {
        return food;
    }

    public Transform GetPlaceholder()
    {
        return itemPlacePoint;
    }

    public bool HasItem()
    {
        return food != null;
    }

    public void SetItem(PickableItemBehaviour item)
    {
        food = item;
    }
}
