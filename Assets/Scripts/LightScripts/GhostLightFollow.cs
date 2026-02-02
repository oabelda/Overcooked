using UnityEngine;

public class GhostLightFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 2f;
    public Vector3 offset = new Vector3(0, 2, 0);

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player != null)
        {
            Vector3 targetPos = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        }
    }
}
