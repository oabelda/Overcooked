using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OrdersPanelBehaviour : MonoBehaviour
{
    OrderCardBehaviour[] orders;

    void Start()
    {
        orders = GetComponentsInChildren<OrderCardBehaviour>(true);

        GameManagerBehaviour.RegisterOnOrderAdded(AddOrder);
    }

    public void AddOrder(Order newOrder)
    {
        for (int i = 0; i < orders.Length; ++i)
        {
            if (!orders[i].gameObject.activeSelf)
            {
                orders[i].SetOrder(newOrder);
                return;
            }
        }
    }
}
