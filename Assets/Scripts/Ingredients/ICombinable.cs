using UnityEngine;

public interface ICombinable
{
    bool Combine(PickableItemBehaviour other, 
        IPickableParentBehaviour parent);
}
