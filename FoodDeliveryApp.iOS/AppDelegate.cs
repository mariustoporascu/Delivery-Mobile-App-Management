﻿using CoreLocation;
using Foundation;
using Plugin.FacebookClient;
using UIKit;

namespace FoodDeliveryApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            global::Xamarin.Forms.Forms.Init();

            //RedCorners.Forms.GoogleMapsSystem.Init("AIzaSyA486Be5d_22x57VrAaO3pyz3CjrqO9S-c", platformConfig);
            //Debug.WriteLine("MAP IS =>>>>>>>> " + RedCorners.Forms.GoogleMapsSystem.IsInitialized);
            global::Xamarin.FormsMaps.Init();
            new CLLocationManager().RequestWhenInUseAuthorization();
            LoadApplication(new App());
            FacebookClientManager.Initialize(app, options);
            return base.FinishedLaunching(app, options);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
            FacebookClientManager.OnActivated();
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options) =>
            FacebookClientManager.OpenUrl(app, url, options);

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        => FacebookClientManager.OpenUrl(application, url, sourceApplication, annotation);

    }
}
