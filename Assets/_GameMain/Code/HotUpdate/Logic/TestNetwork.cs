using System;
using System.Net;
using MiniGame.Logger;
using MiniGame.Network;
using UnityEngine;

public class TestNetwork : MonoBehaviour
{
    private TcpClient client;
    private byte[] pingBytes;

    private void Awake()
    {
        pingBytes = System.Text.Encoding.UTF8.GetBytes("ping");
    }

    private async void Start()
    {
        client = NetworkModule.CreateTcpClient();
        LogModule.Info("Start Connect");
        await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 11223);
        LogModule.Info("Connect Done");
    }

    private void OnDestroy()
    {
        client.Dispose();
    }
}