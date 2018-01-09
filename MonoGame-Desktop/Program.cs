using System;

namespace MonoGame_Desktop
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new MonoGame_Shared.MonoGame(new DesktopPlatformDefs()))
                game.Run();
        }
    }
}
