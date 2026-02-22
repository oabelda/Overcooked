using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class KitchenChaosDirectorSystem : MonoBehaviour
{
    List<IntentData> intents = new();

    public KitchenState currentState;
    public KitchenState targetState;

    [Range(0.1f, 5f)]
    public float momentumSpeed = 1.0f;


    [SerializeField] AnalyticsSystem analytics;
    [SerializeField] OrderSystem orderGenerator;
    [SerializeField] RolePressureSystem rolePressureSystem; // Registrar HandleOverloadedPlayer y HandleUnderloadedPlayer

    public void RegisterIntent(KitchenIntent intent,float priority)
    {
        intents.Add(new IntentData(intent, priority));
    }

    void LateUpdate()
    {
        ResolveIntentions();
        intents.Clear();
    }

    void ResolveIntentions()
    {
        float chaos = 0;
        float relief = 0;

        foreach (var i in intents)
        {
            switch (i.intent)
            {
                case KitchenIntent.IncreaseChaos:
                    chaos += i.priority;
                    break;

                case KitchenIntent.ProvideRelief:
                    relief += i.priority;
                    break;
            }
        }

        targetState.chaosLevel =
            Mathf.Clamp01(chaos - relief * 0.5f);

        targetState.reliefLevel =
            Mathf.Clamp01(relief);
    }



    void Update()
    {
        currentState.chaosLevel = Mathf.Lerp(
            currentState.chaosLevel,
            targetState.chaosLevel,
            Time.deltaTime * momentumSpeed);

        currentState.reliefLevel = Mathf.Lerp(
            currentState.reliefLevel,
            targetState.reliefLevel,
            Time.deltaTime * momentumSpeed);

        currentState.cooperationBias = Mathf.Lerp(
            currentState.cooperationBias,
            targetState.cooperationBias,
            Time.deltaTime * momentumSpeed);
    }

    public KitchenState GetCurrentState()
    {
        return currentState;
    }

    //void Update()
    //{
    //    float pressure = EvaluatePressure();

    //    // DecideIntervention
    //    {
    //        if (funPressureIndex < 30)
    //            InjectChaos(); // CreateResourceConflict(); MixRecipeDependencies();

    //        else if (funPressureIndex > 70)
    //            CreateReliefMoment();
    //    }
    //}


    //float EvaluatePressure()
    //{
    //    float orderStress =
    //        analytics.activeOrders * 5f +
    //        analytics.ordersNearFail * 10f;

    //    float chaosStress =
    //        analytics.playerCollisions * 2f +
    //        analytics.burnedFood * 8f;

    //    float idlePenalty =
    //        analytics.timeWithoutMistakes < 20f ? -10f : 0f;

    //    return Mathf.Clamp(orderStress + chaosStress + idlePenalty, 0, 100);

    //    funPressureIndex =
    //        analytics.activeOrders * 0.4f +
    //        analytics.ordersNearFail * 1.2f +
    //        analytics.recentMistakes * 0.8f -
    //        analytics.comboChain * 0.5f;

    //    funPressureIndex = Mathf.Clamp(funPressureIndex, 0, 100);
    //}

    void HandleOverloadedPlayer(int playerId)
    {
        orderGenerator.IncreaseParallelRecipes();
        orderGenerator.ReduceCriticalDependencies();
    }

    void HandleUnderloadedPlayer(int playerId)
    {
        orderGenerator.SpawnAssistTasksNear(playerId);
    }
}

public enum KitchenIntent
{
    None,
    IncreaseChaos,
    ReducePressure,
    EncourageCooperation,
    EnableClimax,
    ProvideRelief
}

public struct KitchenState
{
    public float chaosLevel;
    public float reliefLevel;
    public float cooperationBias;
    public float recipeComplexity;
}

public struct IntentData
{
    public KitchenIntent intent;
    public float priority;

    public IntentData(KitchenIntent intent, float priority)
    {
        this.intent = intent;
        this.priority = priority;
    }
}


