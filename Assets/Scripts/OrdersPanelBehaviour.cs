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
        Debug.Log("Order Added");
        for (int i = 0; i < orders.Length; ++i)
        {
            Debug.Log(i);
            if (!orders[i].gameObject.activeSelf)
            {
                Debug.Log("Ready to set");
                orders[i].SetOrder(newOrder);
                return;
            }
        }
    }
}
