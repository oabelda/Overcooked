using UnityEngine;

[RequireComponent(typeof(PickableItemBehaviour))]
public class ChopIngredientBehaviour : MonoBehaviour, IKitchenProcessing
{
    [SerializeField] PickableItemBehaviour choppedPrefab;

    [SerializeField] float choppingTime;
    float choppedTime;

    void Start()
    {
        // Initialize
        choppedTime = 0;
    }

    public void Complete()
    {
        // Change to cooked ingredient
        PickableItemBehaviour newIngredient;
        newIngredient = GameObject.Instantiate(choppedPrefab);

        newIngredient.SetParent(this.GetComponent<PickableItemBehaviour>()
            .GetParent());

        GameObject.Destroy(this.gameObject);
    }

    public void Process(float time)
    {
        // Is not already cooked
        choppedTime = choppedTime + time;

        // Finish cooking
        if (choppedTime >= choppingTime)
        {
            Complete();
        }
    }

    public float GetRemainingProcess()
    {
        // Is not already cooked
        return choppingTime - choppedTime;
    }

    public float GetProcess()
    {
        return choppedTime / choppingTime;
    }
}
