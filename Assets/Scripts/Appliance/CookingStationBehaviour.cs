using UnityEngine;

public class CookingStationBehaviour : InteractableBehaviour, IProcessor
{
    [SerializeField] float cookingPower;

    CookIngredientBehaviour cookableItem;

    event PickableEvent OnItemPlaced;
    event FloatEvent OnItemProcessed;

    ParticleSystem particles;

    protected override void Start()
    {
        base.Start();

        this.particles = GetComponentInChildren<ParticleSystem>();

        ActivateFire();
    }

    private void Update()
    {
        cookableItem.Process(Time.deltaTime
            * cookingPower);

        if (OnItemProcessed != null && cookableItem != null)
            OnItemProcessed(cookableItem.GetProcess());
    }

    public override void SetItem(PickableItemBehaviour item)
    {
        base.SetItem(item);
        cookableItem = placedItem?.GetComponent<CookIngredientBehaviour>();
        ActivateFire(); // this.enable = cookableItem != null;
        if (OnItemPlaced != null)
            OnItemPlaced(cookableItem);
    }

    private void ActivateFire()
    {
        if (cookableItem != null)
        {
            this.enabled = true;
            particles.gameObject.SetActive(true);
        }
        else
        {
            this.enabled = false;
            particles.gameObject.SetActive(false);
        }
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
