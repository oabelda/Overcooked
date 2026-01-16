using System.ComponentModel;
using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{
    static GameManagerBehaviour instance;

    [SerializeField] MenuSO menu;
    Order order;
    [SerializeField] int score;

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
        order = menu.GetRandomOrder();
        Debug.Log(order.GetNameString());
    }

    public static bool Deliver(PickableItemBehaviour delivered)
    {
        // They have same name, and there is no container or if there is
        // its "same" container
        if(delivered.GetName() == instance.order.GetOrderName()
            &&
                (!instance.order.IsCombinable() ||
                delivered.GetComponent<ContainerCombinableBehaviour>()
                    .CheckToppings(instance.order)
                )
            )
        { 
            ++instance.score;
            return true;
        }
        else
        {
            return false;
        }
    }
}
