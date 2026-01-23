using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{
    static GameManagerBehaviour instance;

    [SerializeField] MenuSO menu;
    Order[] orders;
    [SerializeField] int score;

    [SerializeField] int maxOrders;
    int actualOrdersCount;
    [SerializeField] int timeBetweenOrders;

    [SerializeField] OrdersPanelBehaviour panel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        orders = new Order[maxOrders];
        actualOrdersCount = 0;
        AddRandomOrder();

        StartCoroutine(OrderManager());
    }

    IEnumerator OrderManager()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenOrders);
            AddRandomOrder();
        }
    }

    public static bool Deliver(PickableItemBehaviour delivered)
    {
        for (int order = 0; order < instance.actualOrdersCount; ++order)
        {
            // They have same name, and there is no container or if there is
            // its "same" container
            if (delivered.GetName() == instance.orders[order].GetOrderName()
                &&
                    (!instance.orders[order].IsCombinable() ||
                    delivered.GetComponent<ContainerCombinableBehaviour>()
                        .CheckToppings(instance.orders[order])
                    )
                )
            {
                // Update score
                ++instance.score;

                // Destroy the order
                instance.RemoveOrder(order);

                return true;
            }
        }
        return false;
    }

    public static void AddRandomOrder()
    {
        if (instance.actualOrdersCount < instance.maxOrders)
        {
            Order newOrder = instance.menu.GetRandomOrder();
            instance.orders[instance.actualOrdersCount] = newOrder;
            instance.actualOrdersCount += 1;

            instance.panel.AddOrder(newOrder);
        }
    }

    private void RemoveOrder(int index)
    {
        // Take out one order
        --actualOrdersCount;

        // "Move down" next orders
        for (int i = index; i< actualOrdersCount; ++i)
        {
            orders[i] = orders[i+1];
        }

        // Remove "last" order
        orders[actualOrdersCount] = null;

        // Show actual orders
        ShowActualOrders();
    }

    private void ShowActualOrders()
    {
        string message = "Los pedidos actuales son: ";
        for (int i = 0; i < actualOrdersCount; ++i)
        {
            message += "\n\t" + i + ": " + orders[i].GetNameString();
        }
        Debug.Log(message);
    }
}
