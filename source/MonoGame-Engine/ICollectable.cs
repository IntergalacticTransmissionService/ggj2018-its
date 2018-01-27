using MonoGame_Engine.Phy;
using System;

namespace MonoGame_Engine
{
    public interface ICollectable : IHasPhysics, IDisposable
    {
        CollectableType Graphics { get; set; }
        bool IsActive { get; }
    }
}