using UnityEngine;

public class GhostLightFollow : MonoBehaviour
{
    [SerializeField] int playerIndex = 0;
    Transform player;
    [SerializeField] float followSpeed = 2f;
    [SerializeField] Vector3 offset = new Vector3(0, 2, 0);

    void AssingPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > playerIndex)
        {
            player = players[playerIndex].transform;
        }
    }

    void Update()
    {
        if (player == null)
        {
            AssingPlayer();
        }

        if (player != null)
        {
            Vector3 targetPos = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        }
    }
}
