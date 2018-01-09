using MonoGame_Shared;

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
