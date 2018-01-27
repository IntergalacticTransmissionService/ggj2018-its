using MonoGame_Engine.Phy;
using System;

namespace MonoGame_Engine
{
    public interface ICollectable :IHasPhysics, IDisposable
    {
        CollectableType Grafic { get; set; }
        bool IsActive { get; }
        
    }
}