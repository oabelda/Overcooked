using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;

public class LocalMultiplayerManager : MonoBehaviour
{

    [SerializeField] GameObject playerPrefab;
    [SerializeField] int maxPlayers = 4;
    [SerializeField] int initPlayers = 2;

    List<PlayerInput> players = new();
    int keyboardPlayersCreated = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < initPlayers; i++)
        {
            CreatePlayer(Keyboard.current);
        }

        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is not Gamepad pad) return;

        if (change == InputDeviceChange.Added)
        {
            AssignGamepadToFreeSlot(pad);
        }
    }

    void AssignGamepadToFreeSlot(Gamepad pad)
    {
        foreach(var player in players)
        {
            // If it's using keyboard move it to Gamepad
            if (player.devices.Contains(Keyboard.current))
            {
                player.user.UnpairDevices();
                UnityEngine.InputSystem.Users.InputUser.PerformPairingWithDevice(pad, player.user);

                Debug.Log($"Player migrated to gamepad: {pad}");
                return;
            }
        }

        // if everyone have a device, create a new player
        CreatePlayer(pad);
    }

    void CreatePlayer(Keyboard keyboard)
    {
        if (players.Count >= maxPlayers) return;

        var go = Instantiate(playerPrefab, new Vector3(players.Count * 2f, 0, 0),
            Quaternion.identity);

        var pi = go.GetComponent<PlayerInput>();

        string mapName = $"Gameplay_P{keyboardPlayersCreated + 1}";

        pi.SwitchCurrentActionMap(mapName);

        UnityEngine.InputSystem.Users.InputUser.PerformPairingWithDevice(Keyboard.current, pi.user);

        players.Add(pi);
        keyboardPlayersCreated++;

        Debug.Log($"Keyboard player created -> {mapName}");
    }


    void CreatePlayer(Gamepad pad)
    {
        if (players.Count >= maxPlayers) return;

        var go = Instantiate(playerPrefab, new Vector3(players.Count * 2f, 0, 0),
            Quaternion.identity);

        var pi = go.GetComponent<PlayerInput>();

        //string mapName = $"Gameplay_P{keyboardPlayersCreated + 1}";

        pi.SwitchCurrentActionMap("Gameplay_P1"); // mismo mapa sirve @TODO

        UnityEngine.InputSystem.Users.InputUser.PerformPairingWithDevice(pad, pi.user);

        players.Add(pi);

        Debug.Log($"Gamepad player created");
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

}
