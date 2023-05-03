using Game.Scripts.Logic;
using NUnit.Framework;
using UnityEngine;


public sealed class ViewTests
{
    [Test]
    public void InterpolationTest()
    {
        var igo = new GameObject().AddComponent<InterpolatedGameObjectView>();
        var pos = new Vector2(0f, 0f);
        
        const float tickDelta = 10f;
        const float drawDelta = 2f;
        float time = 0f;
        
        igo.SetPosition(pos, time, tickDelta);

        time += drawDelta; // 2
        AreEqual(new Vector2(0f, 0f), igo.GetInterpolatedPosition(time));
        
        time += drawDelta; // 4
        AreEqual(new Vector2(0f, 0f), igo.GetInterpolatedPosition(time));


        pos = new Vector2(30f, 0f);
        igo.SetPosition(pos, time, tickDelta); // (30; 0), 4, 10
        
        
        time += drawDelta; // 6
        AreEqual(new Vector2(6f, 0f), igo.GetInterpolatedPosition(time));
    }

    private void AreEqual(Vector2 expected, Vector2 actual)
    {
        Assert.AreEqual(expected.x, actual.x, 0.01);
        Assert.AreEqual(expected.y, actual.y, 0.01);
    }
}