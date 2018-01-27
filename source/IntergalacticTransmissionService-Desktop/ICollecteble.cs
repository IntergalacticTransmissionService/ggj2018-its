using MonoGame_Engine.Phy;
using System;

namespace MonoGame_Engine
{
    public interface ICollecteble :IHasPhysics, IDisposable
    {
        CollectebleGrafic Grafic { get; set; }
        bool IsActive { get; }
        
    }
}