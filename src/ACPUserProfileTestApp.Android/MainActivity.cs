/*
Copyright 2020 Adobe
All Rights Reserved.

NOTICE: Adobe permits you to use, modify, and distribute this file in
accordance with the terms of the Adobe license agreement accompanying
it. If you have received this file from a source other than Adobe,
then your use, modification, or distribution of it requires the prior
written permission of Adobe. (See LICENSE-MIT in the root folder for details)
*/

using System;
using Com.Adobe.Marketing.Mobile;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;

namespace ACPUserProfileTestApp.Droid
{
    class CoreStartCompletionCallback : Java.Lang.Object, IAdobeCallback
    {
        public void Call(Java.Lang.Object callback)
        {
            // set launch config
            ACPCore.ConfigureWithAppID("94f571f308d5/00fc543a60e1/launch-c861fab912f7-development");
        }
    }

    [Activity(Label = "ACPUserProfileTestApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    DataScheme = "acpuserprofiletestapp",
    DataHost = "link",
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            // set the wrapper type
            ACPCore.SetWrapperType(WrapperType.Xamarin);

            // set log level
            ACPCore.LogLevel = LoggingMode.Verbose;

            // set application
            ACPCore.Application = this.Application;
            // register SDK extensions
            ACPUserProfile.RegisterExtension();

            // start core
            ACPCore.Start(new CoreStartCompletionCallback());

            // register dependency service to link interface from ACPGriffonTestApp base project
            DependencyService.Register<IACPUserProfileExtensionService, ACPUserProfileExtensionService>();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ACPCore.LifecycleStart(null);
        }

        protected override void OnPause()
        {
            base.OnPause();
            ACPCore.LifecyclePause();
        }
    }
}