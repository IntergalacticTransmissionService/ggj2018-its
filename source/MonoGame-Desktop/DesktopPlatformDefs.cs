using MonoGame_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame_Desktop
{
    class DesktopPlatformDefs : PlatformDefs
    {
#if DEBUG
        public int Width => 800;

        public int Height => 480;

        public bool FullScreen => false;
#else
        public int Width => 1920;

        public int Height => 1080;

        public bool FullScreen => true;

#endif
    }
}
