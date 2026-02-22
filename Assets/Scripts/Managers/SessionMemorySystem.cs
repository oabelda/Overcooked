using System;
using System.IO;
using UnityEngine;

public class SessionMemorySystem : MonoBehaviour
{

    TeamStrategy lastKnownStrategy;

    // Rendimiento medio
    float averageStarExpectationIndex;
    float averageComboLength;
    float averageMistakeRate;

    // Perfil de presión
    float averageTeamPressure;
    float overloadFrequency;

    string path = Application.persistentDataPath + "/sessionMemory.json";

    public void Save(SessionMemoryData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public SessionMemoryData Load()
    {
        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SessionMemoryData>(json);
    }

    static void ApplyMemory(SessionMemoryData memory)
    {
        // No sé muy bien donde va esto
        //chaosDirector.initialChaosBias =
        //    memory.avgPressure < 40 ? 0.2f : 0.5f;

        //starBalancer.initialReliefBias =
        //    memory.avgSEI < 0.8f ? 0.4f : 0f;

        //strategySystem.ForceInitialStrategy(
        //    memory.preferredStrategy);
    }


}

[Serializable]
public class SessionMemoryData
{
    public TeamStrategy preferredStrategy;

    public float avgSEI;
    public float avgCombo;
    public float avgMistakes;
    public float avgPressure;
}

