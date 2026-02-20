using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName="Menu",menuName="Menu")]
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

    float spawnTime; // Never changes
    float expectedTime; // Never changes @TODO que dependa de la presión y del plato (suma de tiempos de pasos (cortar, cocinar, etc) + buffers
    float maxTime; // Never changes @TODO que dependa de la presión y del plato (expected * factorMargen)
    float failTime; // Changes

    public event EventOrder OnOrderFailed;
    public event EventOrder OnOrderDelivered;

    public Order(PickableItemBehaviour item)
    {
        order = item;
        ordContainer = order.GetComponent<ContainerCombinableBehaviour>();

        expectedTime = 10;

        if (ordContainer)
        {
            toppings = new bool[ordContainer.GetToppingsCount()];

            for (int index = 0; index < toppings.Length; ++index)
            {
                toppings[index] = Random.Range(0, 2) == 1;
                expectedTime += toppings[index] ? 5 : 0;
            }
        }

        maxTime = expectedTime * 2.5f;

        spawnTime = Time.time;
        failTime = spawnTime + maxTime;
    }

    public string GetNameString()
    {
        string message = order.gameObject.name;
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

    public Sprite GetSprite()
    {
        return order.GetSprite();
    }

    public Sprite[] GetToppingsSprites()
    {
        if (ordContainer)
        {
            List<Sprite> sprites = new List<Sprite>();

            for (int i = 0; i < toppings.Length; ++i)
            {
                if (toppings[i])
                {
                    sprites.Add(ordContainer.GetToppingSprite(i));
                }
            }
            return sprites.ToArray();
        }

        return null;
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

    public int GetToppingsCount()
    {
        if (toppings == null)
            return 0;

        int count = 0;
        for (int i = 0; i < toppings.Length; ++i)
        {
            if (toppings[i])
            {
                count += 1;
            }
        }
        return count;
    }

    private bool IsCombinable()
    {
        return ordContainer != null;
    }

    public float GetDeliveryTime()
    {
        return Time.time - spawnTime;
    }
    public float GetExpectedTime()
    {
        return expectedTime;
    }

    public float GetFailTime() { return  failTime; }

    public float GetRelativeSpeed()
    {
        float duration = GetDeliveryTime();
        float quickTime = expectedTime * 0.8f ;
        float slowTime = expectedTime * 1.2f;

        return Mathf.InverseLerp(slowTime, quickTime, duration);
    }

    public float GetProgress()
    {
        float remaining = failTime - Time.time;
        return 1 - (remaining / maxTime);
    }

    public void Deliver()
    {
        if (OnOrderDelivered!= null) OnOrderDelivered(this);
    }

    public void Fail()
    {
        failTime = Time.time + maxTime;
        if (OnOrderFailed != null) OnOrderFailed(this);
    }
}
