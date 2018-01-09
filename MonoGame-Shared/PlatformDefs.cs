using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Shared
{
    public interface PlatformDefs
    {
        int Width { get; }
        int Height { get; }
        bool FullScreen { get; }
    }
}
