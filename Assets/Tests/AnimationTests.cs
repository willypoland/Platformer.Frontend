using System.Linq;
using Game;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;



public class AnimationTests
{
    [Test]
    public void Mapping()
    {
        const int maxInputFrames = 15;
        const int maxOutputFrames = 4;
        var animation = FixedAnimation.CreateTestMock(maxOutputFrames, maxInputFrames, false);
        
        var actual = new int[maxInputFrames];
        
        for (int i = 0; i < maxInputFrames; i++)
        {
            actual[i] = animation.MapIndex(i);
        }

        var expect = new int[] {0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3};
        Assert.IsTrue(expect.SequenceEqual(actual));
    } 
}
