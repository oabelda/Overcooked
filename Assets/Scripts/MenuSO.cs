using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName="Menu",menuName="ScriptableObject/Menu")]
public class MenuSO : ScriptableObject
{
    [SerializeField] PickableItemBehaviour[] dishes;

    public Order GetRandomOrder()
    {
        int selected = Random.Range(0, dishes.Length);
        Order newOrder = new Order(dishes[selected]);
        return newOrder;
    }
}

public class Order
{
    PickableItemBehaviour order;
    ContainerCombinableBehaviour ordContainer;
    bool[] toppings;
    float spawnTime;

    public Order(PickableItemBehaviour item)
    {
        order = item;
        ordContainer = order.GetComponent<ContainerCombinableBehaviour>();
        if (ordContainer)
        {
            toppings = new bool[ordContainer.GetToppingsCount()];

            for (int index = 0; index < toppings.Length; ++index)
            {
                toppings[index] = Random.Range(0, 2) == 1;
            }
        }

        spawnTime = Time.time;
    }

    public string GetNameString()
    {
        string message = "Quiero " + order.gameObject.name;
        if (ordContainer)
        {
            message += " con ";
            for (int index = 0; index < toppings.Length; ++index)
            {
                if (toppings[index])
                {
                    message += ordContainer.GetToppingName(index) + ", ";
                }
            }
        }

        return message;
    }

    public bool CheckOrderInstance(PickableItemBehaviour instance)
    {
        return instance.IsInstanceOf(order);
    }

    public bool GetRequiredTopping(int index)
    {
        return toppings[index];
    }

    public bool IsCombinable()
    {
        return ordContainer != null;
    }

    public float GetSpawnTime()
    {
        return spawnTime;
    }

    public float GetDelayTime()
    {
        return Time.time - spawnTime;
    }
}
