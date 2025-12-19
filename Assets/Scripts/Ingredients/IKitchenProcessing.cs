using UnityEngine;

public interface IKitchenProcessing
{
    void Complete();
    void Process(float time);
    float GetRemainingProcess();
}
