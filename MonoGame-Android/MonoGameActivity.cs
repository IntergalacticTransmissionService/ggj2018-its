using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace MonoGame_Android
{
    [Activity(Label = "MonoGame-Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.FullUser
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class MonoGameActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new MonoGame_Shared.MonoGame(new AndroidPlatformDefXhdpi());
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

