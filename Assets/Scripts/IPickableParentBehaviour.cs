using UnityEngine;

public interface IPickableParentBehaviour
{
    void SetItem(PickableItemBehaviour item);
    bool HasItem();
    Transform GetPlaceholder();
    PickableItemBehaviour GetItem();
}
