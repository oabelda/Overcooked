using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;



public class GameManagerBehaviour : MonoBehaviour
{
    public delegate void NewOrderDelegate(Order newOrder);
    public delegate void RemoveOrderDelegate(int index);

    static GameManagerBehaviour instance;

    [Header("Orders")]
    [SerializeField] MenuSO menu;
    [SerializeField] int maxOrders;
    Order[] orders;

    [Header("Timing")]
    [SerializeField] float minDelay;
    [SerializeField] float maxDelay;
    int actualOrdersCount;
    float orderTimer;

    [Header("Pressure")]
    [Range(0, 1)]
    [SerializeField] float pressure; float Pressure { get { return pressure; } set { pressure = Mathf.Clamp01(value); } }
    [SerializeField] AnimationCurve pressureCuver;

    float eventModifier;
    float eventDelayMultiplier = 1;

    bool spawnLocked;
    int consecutiveSpawns;
    [SerializeField] float targetDeliveryTime;
    float averageDeliveryTime;
    int comboCount;

    [Header("Score")]
    [SerializeField] int score;

    event NewOrderDelegate OnOrderAdded;
    event RemoveOrderDelegate OnOrderRemoved;

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
        comboCount = 0;
        orderTimer = maxDelay;
        averageDeliveryTime = targetDeliveryTime;
        AddRandomOrder();
    }

    #region Pressure Methods 
    private void Update()
    {
        // panel.UpdatePressureSlider(pressure);

        if (actualOrdersCount >= maxOrders || spawnLocked) return;

        orderTimer -= Time.deltaTime;

        if (orderTimer <= 0) // Try Spawn Order
        {
            // Get effective pressure
            float effectivePressure = GetEffectivePressure();

            float chance = Mathf.Lerp(.4f, .9f, effectivePressure);

            if (consecutiveSpawns >= 2)
                chance *= .4f;

            if (Random.value < chance)
            {
                AddRandomOrder();
                ++consecutiveSpawns;
            }
            else
            {
                consecutiveSpawns = 0;
            }

            // Get next delay
            orderTimer = Mathf.Lerp(maxDelay, minDelay, effectivePressure) * eventDelayMultiplier;
        }
    }

    private float GetEffectivePressure()
    {
        return pressureCuver.Evaluate(Pressure) + eventModifier;
    }

    private void OnOrderDelivered(Order order)
    {
        Pressure += 0.1f;

        float deliveryTime = order.GetDelayTime();

        ++comboCount;

        float speedScore = Mathf.InverseLerp(targetDeliveryTime * 1.5f,
                                             targetDeliveryTime * 0.5f,
                                             deliveryTime);

        // Average
        averageDeliveryTime = Mathf.Lerp(averageDeliveryTime, deliveryTime, 0.2f);

        float avgScore = Mathf.InverseLerp(targetDeliveryTime * 1.5f,
                                           targetDeliveryTime * 0.7f,
                                           averageDeliveryTime);

        pressure += comboCount * 0.02f;
        pressure += speedScore * 0.08f;
        pressure += avgScore * 0.05f;

        pressure = Mathf.Clamp01(pressure);
    }

    private void OnOrderFailed()
    {
        comboCount = 0;
        Pressure -= 0.15f;
    }

    public void TriggerRush(int seconds, float eventModifier)
    {
        StartCoroutine(RushEvent(seconds, eventModifier));
    }

    private IEnumerator RushEvent(int seconds, float eventModifier)
    {
        this.eventModifier += eventModifier;
        yield return new WaitForSeconds(seconds);
        this.eventModifier -= eventModifier;
    }

    public void TriggerCalm(int seconds, float delayMultiplier)
    {
        StartCoroutine(CalmEvent(seconds, delayMultiplier));
    }

    private IEnumerator CalmEvent(int seconds, float delayMultiplier)
    {
        this.eventDelayMultiplier += delayMultiplier;
        yield return new WaitForSeconds(seconds);
        this.eventDelayMultiplier -= delayMultiplier;
    }
    #endregion

    public static bool Deliver(PickableItemBehaviour delivered)
    {
        for (int order = 0; order < instance.actualOrdersCount; ++order)
        {
            // They have same name, and there is no container or if there is
            // its "same" container
            if (instance.orders[order].CheckOrderInstance(delivered)
                &&
                    (!instance.orders[order].IsCombinable() ||
                    delivered.GetComponent<ContainerCombinableBehaviour>()
                        .CheckToppings(instance.orders[order])
                    )
                )
            {
                // Update score
                ++instance.score;

                // Succeed Order Delivered
                instance.OnOrderDelivered(instance.orders[order]);

                // Destroy the order
                instance.RemoveOrder(order);

                // Delay the next order
                instance.orderTimer += Random.Range(0.5f, 1.5f);

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

            if (instance.OnOrderAdded != null)
                instance.OnOrderAdded(newOrder);
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
        if (instance.OnOrderRemoved != null)
            instance.OnOrderRemoved(index);

        // If there is no orders
        if (actualOrdersCount == 0)
        {
            // Add one
            AddRandomOrder();

            // Get next delay
            orderTimer = Mathf.Lerp(maxDelay, minDelay, GetEffectivePressure()) * eventDelayMultiplier;
        }
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

    public static void RegisterOnOrderAdded(NewOrderDelegate f)
    {
        instance.OnOrderAdded += f;
    }

    public static void UnregisterOnOrderAdded(NewOrderDelegate f)
    {
        instance.OnOrderAdded -= f;
    }

    public static void RegisterOnOrderRemoved(RemoveOrderDelegate f)
    {
        instance.OnOrderRemoved += f;
    }

    public static void UnregisterOnOrderRemoved(RemoveOrderDelegate f)
    {
        instance.OnOrderRemoved -= f;
    }
}
