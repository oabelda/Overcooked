using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManagerBehaviour : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] PlayerSlot[] slots;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            SpawnNewPlayer();
    }

    void SpawnNewPlayer()
    {
        if (GameManagerBehaviour.GetNumPlayers() >= slots.Length) return;

        Vector3 playerPos = new Vector3(0, 0, 0);
        Vector3 playerRotation = Camera.main.transform.position - playerPos;

        GameObject newPlayer = GameObject.Instantiate(playerPrefab, 
            new Vector3(0, 0, 0),
            Quaternion.identity);

        newPlayer.transform.LookAt(Camera.main.transform.position);

        Vector3 newRotation = new Vector3(0, newPlayer.transform.eulerAngles.y, 0);
        newPlayer.transform.eulerAngles = newRotation;
    }

    private int SearchEmptySlot()
    {
        for (int i = 0; i < slots.Length; i += 1)
        {
            if (slots[i].IsEmpty())
            {
                return i;
            }
        }

        return -1;
    }

    public bool GetSlot(PlayerControllerBehaviour player)
    {
        int slot = SearchEmptySlot();
        if (slot == -1)
        {
            return false;
        }
        else
        {
            player.name = "Player " + (slot + 1);
            slots[slot].SetPlayer(player);
            return true;
        }
    }
}

[System.Serializable]
class PlayerSlot
{
    [SerializeField] PlayerInputsSO input;
    PlayerControllerBehaviour player;
    [SerializeField] Color color;

    public void SetPlayer(PlayerControllerBehaviour player)
    {
        this.player = player;
        
        if (player != null) player.SetInputs(input);
        player.GetComponent<PlayerColor>().SetColor(color);
    }

    public bool IsEmpty()
    {
        return (this.player == null);
    }
}
