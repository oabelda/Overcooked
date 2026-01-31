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
    public delegate void EventOrder(Order order);

    PickableItemBehaviour order;
    ContainerCombinableBehaviour ordContainer;
    bool[] toppings;
    float spawnTime;
    float failTime;

    public event EventOrder OnOrderFailed;
    public event EventOrder OnOrderDelivered;

    public Order(PickableItemBehaviour item)
    {
        order = item;
        ordContainer = order.GetComponent<ContainerCombinableBehaviour>();

        failTime = 20;

        if (ordContainer)
        {
            toppings = new bool[ordContainer.GetToppingsCount()];

            for (int index = 0; index < toppings.Length; ++index)
            {
                toppings[index] = Random.Range(0, 2) == 1;
                failTime += toppings[index] ? 10 : 0;
            }
        }

        spawnTime = Time.time;
        failTime += spawnTime;
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

    public bool CheckOrderInstance(PickableItemBehaviour checking)
    {
        // They have the same name, adn there is no container or if there is its "same" contaniner
        return checking.IsInstanceOf(order) &&
                (!IsCombinable() || checking.GetComponent<ContainerCombinableBehaviour>().CheckToppings(this));
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

    public float GetProgress()
    {
        Debug.Log("time " + Time.time + " Spw: " + spawnTime + " Fail: " + failTime + " time-spwn " + (Time.time - spawnTime) + " fail-spwn" + (failTime - spawnTime) + " result: " + ((Time.time - spawnTime) / (failTime - spawnTime)));
        return (Time.time - spawnTime) / (failTime - spawnTime);
    }

    internal void Deliver()
    {
        if (OnOrderDelivered!= null) OnOrderDelivered(this);
    }
}
