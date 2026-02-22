using UnityEngine;

public class EmergentStrategySystem : MonoBehaviour
{
    public TeamStrategy currentStrategy;

    // public TeamAnalytics analytics;

    void Update()
    {
        DetectStrategy();
    }

    void DetectStrategy()
    {
        //float specialization =
        //    analytics.ActionVarianceBetweenPlayers();

        //float stockpile =
        //    analytics.PreparedIngredientAverage();

        //float reactive =
        //    analytics.LateOrderStartRatio();

        //if (specialization > 0.6f)
        //    currentStrategy = TeamStrategy.SpecializedRoles;

        //else if (stockpile > 0.7f)
        //    currentStrategy = TeamStrategy.Stockpiling;

        //else if (reactive > 0.6f)
        //    currentStrategy = TeamStrategy.ReactiveCooking;

        //else
        //    currentStrategy = TeamStrategy.AssemblyLine;
    }
}

public enum TeamStrategy
{
    Undefined,
    SpecializedRoles,
    AssemblyLine,
    ReactiveCooking,
    Stockpiling
}
