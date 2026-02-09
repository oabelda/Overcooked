using UnityEngine;

public class ScoreManagerBehaviour : MonoBehaviour
{

    /*
     * Pedido = Base + Variable (ingredientes) + Tip(tiempo) * Combo
     * 
     * ¿Estrellas?
     */
    [SerializeField] LevelConfig level;

    int comboCount;
    int comboMultiplier;

    int score;
    int highestCombo;
    int fails;
    int delivers;

    public void OnDeliver(int ingredientsCount, float relativeTime,
        bool rightOrder)
    {
        if (!rightOrder)
        {
            // Reiniciar el combo
            ResetCombo();
        }

        // Puntuar
        int tip = Mathf.RoundToInt(Mathf.Lerp(level.GetMinTip(), level.GetMaxTip(), relativeTime));

        int thisScore = level.GetBaseDeliverScore() 
            + ingredientsCount * level.GetIngredientScore()
            + tip * comboMultiplier;

        score += thisScore;

        delivers += 1;

        if (rightOrder)
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

    public void RegisterFail()
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
}
