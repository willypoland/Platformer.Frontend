using System.Net;
using Api;
using NUnit.Framework;
using UnityEngine;


public class ApiTests
{
    [Test]
    public void GameStateTest()
    {
        IApi api = ApiFactory.CreateApiSync();
        
        var endpoint = api.GetPublicEndpoint(7000);

        Location loc = new Location()
        {
            IsFirstPlayer = true,
            PositionFirstPlayer = new Vector2Int(0, 0),
            PositionSecondPlayer = new Vector2Int(256, 0),
            Platfroms = new Platform[]
            {
                new(0, 0, 864, 32, 0, 864),
                new(1, 0, 192, 32, 256, 608),
            },
        };
        
        api.Init(loc);

        var remoteEndpoint = new Endpoint
        {
            RemoteHost = IPAddress.Loopback,
            RemotePort = 7001,
        };
        
        api.RegisterPeer(remoteEndpoint);
        
        api.StartGame();

        InputMap input = default;
        
        api.Update(input);

        var buffer = new byte[512];
        api.GetState(buffer, out int len, out float dx);
        
        IGameState gs = ApiFactory.CreateGameState();
        gs.Update(buffer, len);
        
        api.StopGame();
    }
}