using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMetrics
{
    public float activeTasks;
    public float recentMistakes;
    public float movementBlockedTime;
    public float idleTime;
    public float successfulActions;
}


public class RolePressureSystem : MonoBehaviour
{
    public List<PlayerMetrics> players;

    public Dictionary<int, float> playerPressure =
        new Dictionary<int, float>();

    public event Action<int> OnPlayerOverloaded;
    public event Action<int> OnPlayerUnderloaded;

    void Update()
    {
        EvaluatePlayers();
    }

    void EvaluatePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            float pressure = CalculatePressure(players[i]);
            playerPressure[i] = pressure;

            if (pressure > 60)
                OnPlayerOverloaded?.Invoke(i);

            else if (pressure < 20)
                OnPlayerUnderloaded?.Invoke(i);
        }
    }

    float CalculatePressure(PlayerMetrics p)
    {
        return
            p.activeTasks * 2f +
            p.recentMistakes * 3f +
            p.movementBlockedTime * 1.5f -
            p.idleTime * 2f;
    }
}
