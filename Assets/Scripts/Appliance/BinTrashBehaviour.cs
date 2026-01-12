using UnityEngine;

[RequireComponent(typeof(Animation))]
public class BinTrashBehaviour : InteractableBehaviour
{
    Animation anim;

    protected override void Start()
    {
        base.Start();

        anim = this.gameObject.GetComponent<Animation>();
    }

    public override void Place(PickableItemBehaviour dropped)
    {
        dropped.DestroyItem();

        anim.Play();
    }

    public override void Interact(PlayerInteractBehaviour player, bool isFirst)
    {
        if (isFirst)
        {
            if (player.HasItem())
            {
                Place(player.GetItem());
            }
        }
    }


}
