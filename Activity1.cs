using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace Sharp_Blast
{
    [Activity(

        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
        )]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);

            HideSystemUI();

            _game = new Game1();
            _view = _game.Services.GetService(typeof(View)) as View;
            
            
            SetContentView(_view);
            
            //Window.Attributes.LayoutInDisplayCutoutMode = LayoutInDisplayCutoutMode.Never;
            _game.Run();
        }
        protected override void OnResume()
        {
            base.OnResume();
            HideSystemUI(); // Force immersive mode when resuming

        }

        protected override void OnStop()
        {
            Game1.exit();
            base.OnStop();
        }

        private void HideSystemUI()
        {
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                SystemUiFlags.LayoutStable |
                SystemUiFlags.LayoutHideNavigation |
                
                SystemUiFlags.HideNavigation |
                
                SystemUiFlags.ImmersiveSticky);
        }
    }
}
