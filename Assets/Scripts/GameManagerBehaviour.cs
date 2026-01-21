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
        orders[0] = menu.GetRandomOrder();
        actualOrdersCount = 1;
        Debug.Log(orders[0].GetNameString());
        StartCoroutine(OrderManager());
    }

    IEnumerator OrderManager()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenOrders);
            if (actualOrdersCount < maxOrders)
            {
                orders[actualOrdersCount] = menu.GetRandomOrder();
                Debug.Log(orders[actualOrdersCount].GetNameString());
                actualOrdersCount += 1;
            }
        }
    }

    public static bool Deliver(PickableItemBehaviour delivered)
    {
        for (int order = 0; order < instance.orders.Length; ++order)
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
                ++instance.score;
                return true;
            }
        }
        return false;
    }
}
