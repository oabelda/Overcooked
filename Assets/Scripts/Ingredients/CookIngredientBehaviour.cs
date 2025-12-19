using UnityEngine;

[RequireComponent(typeof(PickableItemBehaviour))]
public class CookIngredientBehaviour : MonoBehaviour, IKitchenProcessing
{
    [SerializeField] PickableItemBehaviour cookedPrefab;

    [SerializeField] float cookingTime;
    float cookedTime;

    [SerializeField] bool willBurn;


    void Start()
    {
        // Initialize
        cookedTime = 0;
    }

    public void Complete()
    {
        // Change to cooked ingredient
        PickableItemBehaviour newIngredient;
        newIngredient = GameObject.Instantiate(cookedPrefab);

        newIngredient.SetParent(
            this.gameObject.GetComponent<PickableItemBehaviour>()
                .GetParent()
            );

        GameObject.Destroy(this.gameObject);
    }

    public void Process(float time)
    {
        // Is not already cooked
        cookedTime = cookedTime + time;

        // Finish cooking
        if (cookedTime >= cookingTime)
        {
            Complete();
        }
    }

    public float GetRemainingProcess()
    {
        // Is not already cooked
        return cookingTime - cookedTime;
    }
}
