using NUnit.Framework;
using CsUtility.Pool;


public class ObjectPoolTest
{
    [Test]
    public void CommonObjectPool()
    {
        var fact = new TestObject.Factory();
        var op = new ObjectPool<TestObject>(fact, 2);

        var o0_1 = op.Get();
        Assert.AreEqual(0, o0_1.Idx.Index);
        Assert.AreEqual(1, o0_1.Idx.Generation);
        Assert.AreEqual(1, o0_1.Item.Id);
        Assert.IsTrue(o0_1.Item.Enabled);
        Assert.IsTrue(op.IsValid(o0_1));

        Assert.IsTrue(op.TryRelease(o0_1));
        Assert.IsFalse(o0_1.Item.Enabled);
        Assert.IsFalse(op.IsValid(o0_1));

        var o0_2 = op.Get();
        Assert.AreEqual(0, o0_2.Idx.Index);
        Assert.AreEqual(2, o0_2.Idx.Generation);
        Assert.AreEqual(1, o0_2.Item.Id);
        Assert.IsTrue(o0_2.Item.Enabled);
        Assert.IsFalse(op.IsValid(o0_1));
        Assert.IsTrue(op.IsValid(o0_2));

        op.Get(); // 1_1
        op.Get(); // 2_1
        op.Get(); // 3_1

        var o4_1 = op.Get();
        Assert.AreEqual(4, o4_1.Idx.Index);
        Assert.AreEqual(1, o4_1.Idx.Generation);
        Assert.AreEqual(5, o4_1.Item.Id);
        Assert.IsTrue(o4_1.Item.Enabled);
        Assert.IsTrue(op.IsValid(o4_1));
        
        op.ReleaseAll();
        
        Assert.IsFalse(op.IsValid(o0_1));
        Assert.IsFalse(op.IsValid(o0_2));
        Assert.IsFalse(op.IsValid(o4_1));
        
        Assert.IsFalse(o0_1.Item.Enabled);
        Assert.IsFalse(o0_2.Item.Enabled);
        Assert.IsFalse(o4_1.Item.Enabled);
        Assert.IsFalse(o4_1.Item.Disposed);
        
        op.Clear();
        
        Assert.IsTrue(o0_1.Item.Disposed);
        Assert.IsTrue(o0_2.Item.Disposed);
        Assert.IsTrue(o4_1.Item.Disposed);
    }

    internal class TestObject
    {
        public readonly int Id;
        public bool Enabled;
        public bool Disposed;

        public TestObject(int id)
        {
            Id = id;
        }

        public override string ToString() => Disposed ? "(DISPOSED)" : $"({Id}:{Enabled})";

        public class Factory : IObjectPoolFactory<TestObject>
        {
            private int GlobalId;

            public TestObject Create() => new TestObject(++GlobalId);

            public void ActionOnGet(TestObject obj) => obj.Enabled = true;

            public void ActionOnRelease(TestObject obj) => obj.Enabled = false;

            public void ActionOnDispose(TestObject obj) => obj.Disposed = true;
        }
    }
}
