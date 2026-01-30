using UnityEngine;

public delegate void FloatEvent(float a);
public delegate void PickableEvent(IKitchenProcessing a);

public interface IProcessor
{
    void RegisterOnItemPlaced(PickableEvent a);
    void RegisterOnItemProcessed(FloatEvent function);
}
