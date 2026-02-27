using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void SimpleDelegate();

public class GameManagerBehaviour : MonoBehaviour
{
    static GameManagerBehaviour instance;

    [SerializeField] LevelConfig level;

    [SerializeField]int numPlayers;
    [SerializeField] string levelName;

    [Header("Orders")]
    [SerializeField] MenuSO menu;
    [SerializeField] int maxOrders;
    Order[] orders;

    [Header("Timing")]
    [SerializeField] float minDelay;
    [SerializeField] float maxDelay;
    int actualOrdersCount;
    float orderTimer;

    float levelTimer;
    [SerializeField] Pressure pressure;

    bool spawnLocked;

    event Order.EventOrder OnOrderAdded;
    event SimpleDelegate OnLevelEnded;
    public static System.Action<Order, int> OnOrderFailed { get; internal set; }
    public static System.Action<Order, int> OnOrderServed { get; internal set; }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.numPlayers = 0;
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
        orderTimer = maxDelay;
        levelTimer = level.GetLevelDuration();
        AddRandomOrder();
    }

    private void Update()
    {
        levelTimer -= Time.deltaTime;

        if (levelTimer < 0)
        {
            // Level ends
            this.enabled = false;
            StopAllCoroutines();
            if (OnLevelEnded != null) OnLevelEnded();
            return;
        }

        for (int i = 0; i < actualOrdersCount; ++i)
        {
            if (orders[i].CheckFail())
            {
                OnOrderFailed?.Invoke(orders[i],i);
            }
        }

        if (actualOrdersCount >= maxOrders || spawnLocked) return;

        orderTimer -= Time.deltaTime;

        if (orderTimer <= 0) // Try Spawn Order
        {
            if (pressure.PressureTry(.4f,.9f,2,.4f))
            {
                AddRandomOrder();
            }
            else
            {
            }

            // Get next delay
            orderTimer = Mathf.Lerp(maxDelay, minDelay, pressure.GetEffectivePressure());
        }
    }

    public static bool TryDeliver(PickableItemBehaviour delivered)
    {
        for (int order = 0; order < instance.actualOrdersCount; ++order)
        {
            // They have same name, and there is no container or if there is
            // its "same" container
            if (instance.orders[order].CheckOrderInstance(delivered))
            {
                instance.Deliver(order);
                return true;
            }
        }
        return false;
    }

    private void Deliver(int index)
    {
        OnOrderServed?.Invoke(orders[index],index);

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
            orderTimer = Mathf.Lerp(maxDelay, minDelay, pressure.GetEffectivePressure());
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

    public static void RegisterOnLevelEnded(SimpleDelegate f)
    {
        instance.OnLevelEnded += f;
    }

    public static void UnregisterOnLevelEnded(SimpleDelegate f)
    {
        instance.OnLevelEnded -= f;
    }

    public static float GetRemainingLevelTime()
    {
        return instance.levelTimer;
    }

    public static int GetNumPlayers()
    {
        return instance.numPlayers;
    }

    public static LevelConfig GetLevel()
    {
        return instance.level;
    }

    public static string GetLevelName()
    {
        return instance.levelName;
    }

    public static void AddNewPlayer()
    {
        instance.numPlayers += 1;
    }

    public static void RemovePlayer() 
    {
        instance.numPlayers -= 1;
    }
}
