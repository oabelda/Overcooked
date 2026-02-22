using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManagerBehaviour : MonoBehaviour
{

    /*
     * Pedido = Base + Variable (ingredientes) + Tip(tiempo) * Combo
     * 
     * ¿Estrellas?
     */
    LevelConfig level;

    int comboCount;
    int comboMultiplier;

    [SerializeField] int score;
    int highestCombo;
    int fails;
    int delivers;

    private void OnEnable()
    {
        GameManagerBehaviour.OnOrderServed += OnDeliver;
        GameManagerBehaviour.OnOrderFailed += RegisterFail;
        GameManagerBehaviour.RegisterOnLevelEnded(OnLevelEnded);
        level = GameManagerBehaviour.GetLevel();
    }

    private void OnLevelEnded()
    {
        SaveManagerBehaviour.Save(new SaveData(
            GameManagerBehaviour.GetLevelName(),
            GetScore(),
            GetMaxCombo(),
            GetFails(),
            GetDelivers(),
            GetStars(GameManagerBehaviour.GetNumPlayers())
    ));
    }

    private void OnDisable()
    {
        GameManagerBehaviour.OnOrderServed -= OnDeliver;
        GameManagerBehaviour.OnOrderFailed -= RegisterFail;
        GameManagerBehaviour.UnregisterOnLevelEnded(OnLevelEnded);
    }

    public void OnDeliver(Order order, int index)
    {
        if (index != 0)
        {
            // Reiniciar el combo
            ResetCombo();
        }

        // Puntuar
        int tip = Mathf.RoundToInt(Mathf.Lerp(level.GetMinTip(), level.GetMaxTip(), order.GetRelativeSpeed(1.5f, 0.5f)));

        int thisScore = level.GetBaseDeliverScore() 
            + order.GetToppingsCount() * level.GetIngredientScore()
            + tip * comboMultiplier;

        score += thisScore;

        delivers += 1;

        if (index == 0)
        {
            ++comboCount;

            // Recalcular multiplicador
            comboMultiplier = Mathf.Min(
                (comboCount + level.GetComboStep()) / level.GetComboStep()
                , level.GetMaxComboMultiplier());

            highestCombo = Mathf.Max(highestCombo, comboCount);
        }
    }

    public void ResetCombo()
    {
        comboCount = 0;
        comboMultiplier = 1;
    }

    public void RegisterFail(Order order, int index)
    {
        ResetCombo();
        fails += 1;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetMaxCombo()
    {
        return highestCombo;
    }

    public int GetFails()
    {
        return fails;
    }

    public int GetDelivers()
    {
        return delivers;
    }

    // @TODO esto no debería ir aquí
    public float GetLevelDuration()
    {
        return level.GetLevelDuration();
    }

    public int GetStars(int playersCount)
    {
        return level.GetStars(score, playersCount);
    }
}
