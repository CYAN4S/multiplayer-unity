using Unity.Netcode;
using UnityEngine;

public class HelloWorldManager : MonoBehaviour
{
    private NetworkManager networkManager;
    
    
    private void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!networkManager.IsClient && !networkManager.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
            SubmitNewPosition();
        }

        GUILayout.EndArea();
    }

    private void StartButtons()
    {
        if (GUILayout.Button("Host")) networkManager.StartHost();
        if (GUILayout.Button("Client")) networkManager.StartClient();
        if (GUILayout.Button("Server")) networkManager.StartServer();
    }

    private void StatusLabels()
    {
        var mode = networkManager.IsHost ? "Host" : networkManager.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " + networkManager.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    private void SubmitNewPosition()
    {
        if (GUILayout.Button(networkManager.IsServer ? "Move" : "Request Position Change"))
        {
            if (networkManager.IsServer && !networkManager.IsClient)
            {
                foreach (ulong uid in networkManager.ConnectedClientsIds)
                    networkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
            }
            else
            {
                var playerObject = networkManager.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.Move();
            }
        }
    }
}