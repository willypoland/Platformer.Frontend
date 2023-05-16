using System.Net;
using Api;
using NUnit.Framework;
using UnityEngine;


public class ApiTests
{
    [Test]
    public void GameStateTest()
    {
        IApi api = ApiFactory.CreateApiAsync();
        
        var endpoint = api.GetPublicEndpoint(7000);

        var platfroms = new Platform[]
        {
            new(0, 0, 864, 32, 0, 864),
            new(1, 0, 192, 32, 256, 608),
        };
        var loc = new Location(true, new(0, 0), new(256, 0), platfroms);
        var ctx = new GameContext(60, new Endpoint(IPAddress.Loopback, 7000));

        api.Init(ctx);
        api.SetLocation(loc);
        var mc = api.GetMicrosecondsInOneTick();
        
        api.StartGame();

        InputMap input = default;
        
        api.Update(input);

        var buffer = new byte[512];
        api.GetState(buffer, out int len);
        
        IGameState gs = ApiFactory.CreateGameState();
        gs.Update(buffer, len);
        
        api.StopGame();
    }
}