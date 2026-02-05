using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;



public class GameManagerBehaviour : MonoBehaviour
{
    static GameManagerBehaviour instance;

    [SerializeField] ScoreManagerBehaviour scoreManager;

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
    float avgDelivery;
    int comboCount;

    [Header("Score")]
    [SerializeField] int score;

    event Order.EventOrder OnOrderAdded;

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
        AddRandomOrder();
        avgDelivery = 0;
    }

    private void Update()
    {
        for (int i = 0; i < actualOrdersCount; ++i)
        {
            if (Time.time >= orders[i].GetFailTime())
            {
                scoreManager.RegisterFail();

                orders[i].Fail();

                // Pressure Update On Order Failed
                comboCount = 0;
                Pressure -= 0.15f;


            }
        }

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

    #region Pressure Events
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

    public static bool TryDeliver(PickableItemBehaviour delivered)
    {
        for (int order = 0; order < instance.actualOrdersCount; ++order)
        {
            // They have same name, and there is no container or if there is
            // its "same" container
            if (instance.orders[order].CheckOrderInstance(delivered))
            {
                instance.scoreManager.OnDeliver(
                    instance.orders[order].GetToppingsCount(), // Ingredients
                    instance.orders[order].GetRelativeSpeed(), // relativeTime
                    order == 0); // rightOrder
                instance.Deliver(order);
                return true;
            }
        }
        return false;
    }

    private void Deliver(int index)
    {
        // Raise score and combo count
        ++score;

        // Update pressure
        float expectedTime = orders[index].GetExpectedTime();

        float speedScore = Mathf.InverseLerp(expectedTime * 1.5f, expectedTime * 0.5f, orders[index].GetDeliveryTime());

        avgDelivery = Mathf.Lerp(avgDelivery, (1-speedScore)*2f, 0.2f);

        // Raise pressure (fixed) + (combo) + (speed) + (avgSpeed)
        Pressure += 0.1f + (comboCount++ * 0.02f) + (speedScore * 0.08f) + (avgDelivery * 0.04f);

        // Show actual orders
        orders[index].Deliver();

        // Take out one order
        --actualOrdersCount;

        // Destroy the order: "Move down" next orders
        for (int i = index; i < actualOrdersCount; ++i)
        {
            orders[i] = orders[i + 1];
        }

        // Remove "last" order
        orders[actualOrdersCount] = null;

        // If there is no orders
        if (actualOrdersCount == 0)
        {
            // Add one
            AddRandomOrder();

            // Get next delay
            orderTimer = Mathf.Lerp(maxDelay, minDelay, GetEffectivePressure()) * eventDelayMultiplier;
        }
        else
        {
            // Delay the next order
            orderTimer += Random.Range(0.5f, 1.5f);
        }
    }

    private static void AddRandomOrder()
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

    public static void RegisterOnOrderAdded(Order.EventOrder f)
    {
        instance.OnOrderAdded += f;
    }
}
