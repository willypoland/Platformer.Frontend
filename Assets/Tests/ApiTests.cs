using Api;
using NUnit.Framework;


public class ApiTests
{
    [Test]
    public void GameStateTest()
    {
        var api = ApiFactory.CreateApiSync();
        var gs = ApiFactory.CreateGameState();
        var buffer = new byte[512];
        
        api.RegisterPeer(0, false, null, 0);
        api.StartGame();
        var status = api.GetStatus();
        api.Update(new InputMap());
        int count = api.GetState(buffer);
        gs.Update(buffer, count);

        api.StopGame();
    }
}