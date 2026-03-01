using System.Collections;
using UnityEngine;

public class PlateDispenserBehaviour : InteractableBehaviour
{
    [SerializeField] float plateRespawnDelay = 10f;
    [SerializeField] float plateOffsetY = .1f;

    System.Collections.Generic.Stack<PlateBehaviour> platesList;


    void Awake()
    {
        platesList = new System.Collections.Generic.Stack<PlateBehaviour>();
    }

    public void PlateRespawn(PlateBehaviour plate)
    {
        StartCoroutine(PlateRespawnCoroutine(plate));
    }

    IEnumerator PlateRespawnCoroutine(PlateBehaviour plate)
    {
        yield return new WaitForSeconds(plateRespawnDelay);
        plate.gameObject.SetActive(true);
        Place(plate);
    }

    public override void Place(PickableItemBehaviour dropped)
    {
        if (dropped is PlateBehaviour)
        {
            dropped.SetParent(this);

            dropped.transform.localPosition = new Vector3(
                dropped.transform.localPosition.x,
                dropped.transform.localPosition.y + plateOffsetY * (platesList.Count-1),
                dropped.transform.localPosition.z);
        }
    }

    public override PickableItemBehaviour Take()
    {
        return platesList.Count > 0 ? platesList.Peek() : null;
    }

    public override void SetItem(PickableItemBehaviour item)
    {
        if (item != null)
        {
            platesList.Push((PlateBehaviour)item);
        }
        else
        {
            platesList.Pop();
        }
    }
}
