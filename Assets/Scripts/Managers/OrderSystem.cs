using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    public List<Order> availableOrders;
    KitchenChaosDirectorSystem kitchenChaosDirector;

    void OnEnable()
    {
        DifficultySystem.OnDifficultyChanged += ApplyDifficulty;
        GameEvents.OnLevelEnded += GameEvents_OnLevelEnded;
    }

    private void GameEvents_OnLevelEnded()
    {
        CancelInvoke(nameof(GenerateOrder));
    }

    public void GenerateOrder()
    {
        // decide tipo de receta según estado suavizado
        KitchenState state = kitchenChaosDirector.GetCurrentState();

        Order order;

        if (state.reliefLevel > 0.6f)
            order = PickEasyOrder();
        else if (state.chaosLevel > 0.7f)
            order = PickConflictOrder();
        else
            order = PickNormalOrder();

        // gameManager.CreateOrder(order);
    }

    Order PickEasyOrder() { return availableOrders[0]; }
    Order PickNormalOrder() { return availableOrders[Random.Range(0, availableOrders.Count)]; }
    Order PickConflictOrder() { return availableOrders[Random.Range(0, availableOrders.Count)]; }

    void ApplyDifficulty(DifficultyState state)
    {
        // @TODO
        //spawnInterval = 1f / state.orderSpawnRate;
        //customerPatience = state.customerPatience;
    }



    //Order GenerateRecipe()
    //{
    //    switch (strategy.currentStrategy)
    //    {
    //        case TeamStrategy.SpecializedRoles:
    //            return RecipePool.ParallelFriendly();

    //        case TeamStrategy.Stockpiling:
    //            return RecipePool.ReusableIngredients();

    //        case TeamStrategy.ReactiveCooking:
    //            return RecipePool.FastRecipes();

    //        default:
    //            return RecipePool.SequentialRecipes();
    //    }
    //}

    private Order NormalRecipe()
    {
        throw new System.NotImplementedException();
    }

    private Order HighConflictRecipe()
    {
        throw new System.NotImplementedException();
    }

    private Order EasyComboRecipe()
    {
        throw new System.NotImplementedException();
    }

    internal void IncreaseParallelRecipes()
    {
        throw new System.NotImplementedException();
    }

    internal void ReduceCriticalDependencies()
    {
        throw new System.NotImplementedException();
    }

    internal void SpawnAssistTasksNear(int playerId)
    {
        throw new System.NotImplementedException();
    }
}
