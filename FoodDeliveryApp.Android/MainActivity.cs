using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FoodDeliveryApp.Constants;
using Xamarin.Essentials;
using AndroidX.AppCompat.App;
using Xamarin.Forms;

namespace FoodDeliveryApp.Droid
{
    [Activity(Label = "FoodDeliveryApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Intent serviceIntent;
        private const int RequestCode = 5469;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            serviceIntent = new Intent(this, typeof(AndroidLocationService));
            SetServiceMethods();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M && !Android.Provider.Settings.CanDrawOverlays(this))
            {
                var intent = new Intent(Android.Provider.Settings.ActionManageOverlayPermission);
                intent.SetFlags(ActivityFlags.NewTask);
                this.StartActivity(intent);
            }
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnBackPressed()
        {
            var app = (FoodDeliveryApp.App)App.Current;
            if (app.PromptToConfirmExit)
            {
                ConfirmWithDialog();
                return;
            }
            base.OnBackPressed();
        }

        void SetServiceMethods()
        {
            MessagingCenter.Subscribe<StartServiceMessage>(this, "ServiceStarted", message =>
            {
                if (!IsServiceRunning(typeof(AndroidLocationService)))
                {
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                    {
                        StartForegroundService(serviceIntent);
                    }
                    else
                    {
                        StartService(serviceIntent);
                    }
                }
            });

            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message =>
            {
                if (IsServiceRunning(typeof(AndroidLocationService)))
                    StopService(serviceIntent);
            });
        }

        private bool IsServiceRunning(System.Type cls)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName.Equals(Java.Lang.Class.FromType(cls).CanonicalName))
                {
                    return true;
                }
            }
            return false;
        }

        private void ConfirmWithDialog()
        {
            using (var alert = new Android.App.AlertDialog.Builder(this))
            {
                alert.SetTitle("Confirma inchiderea aplicatiei");
                alert.SetMessage("Esti sigur ca vrei sa inchizi aplicatia?");
                alert.SetPositiveButton("Da", (sender, args) =>
                {
                    FinishAffinity();
                    if (IsServiceRunning(typeof(AndroidLocationService)))
                        StopService(serviceIntent);
                    Preferences.Set("LocationServiceRunning", false);
                });
                alert.SetNegativeButton("Nu", (sender, args) => { }); // do nothing

                var dialog = alert.Create();
                dialog.Show();
            }
            return;
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            if (requestCode == RequestCode)
            {
                if (Android.Provider.Settings.CanDrawOverlays(this))
                {

                }
            }
            base.OnActivityResult(requestCode, resultCode, intent);
        }
    }
}