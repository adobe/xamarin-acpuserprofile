# Adobe Experience Platform - User Profile plugin for Xamarin apps
![CI](https://github.com/adobe/xamarin-acpuserprofile/workflows/CI/badge.svg)
[![NuGet](https://buildstats.info/nuget/Adobe.ACPUserProfile.Android)](https://www.nuget.org/packages/Adobe.ACPUserProfile.Android/)
[![NuGet](https://buildstats.info/nuget/Adobe.ACPUserProfile.iOS)](https://www.nuget.org/packages/Adobe.ACPUserProfile.iOS/)
[![GitHub](https://img.shields.io/github/license/adobe/xamarin-acpuserprofile)](https://github.com/adobe/xamarin-acpuserprofile/blob/master/LICENSE)

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Running Tests](#running-tests)
- [Sample App](#sample-app)
- [Contributing](#contributing)
- [Licensing](#licensing)

## Prerequisites

Xamarin development requires the installation of [Microsoft Visual Studio](https://visualstudio.microsoft.com/downloads/). Information regarding installation for Xamarin development is available for [Mac](https://docs.microsoft.com/en-us/visualstudio/mac/installation?view=vsmac-2019) or [Windows](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2019).

 An [Apple developer account](https://developer.apple.com/programs/enroll/) and the latest version of Xcode (available from the App Store) are required if you are [building an iOS app](https://docs.microsoft.com/en-us/visualstudio/mac/installation?view=vsmac-2019).

## Installation

**Package Manager Installation**

The ACPUserProfile Xamarin NuGet package for Android or iOS can be added to your project by right clicking the _"Packages"_ folder within the project you are working on then selecting _"Manage NuGet Packages"_. In the window that opens, ensure that your selected source is `nuget.org` and search for _"Adobe.ACP"_. After selecting the Xamarin AEP SDK packages that are required, click on the _"Add Packages"_ button. After exiting the _"Add Packages"_ menu, right click the main solution or the _"Packages"_ folder and select _"Restore"_ to ensure the added packages are downloaded.

**Manual installation**

Local ACPUserProfile NuGet packages can be created via the included Makefile. If building for the first time, run:

```
make setup
```

followed by:

```
make release
```

The created NuGet packages can be found in the `bin` directory. This directory can be added as a local nuget source and packages within the directory can be added to a Xamarin project following the steps in the "Package Manager Installation" above.
## Usage

The ACPUserProfile binding can be opened by loading the `ACPUserProfile.sln` with Visual Studio. The following targets are available in the solution:

- Adobe.ACPUserProfile.iOS - The ACPUserProfile iOS bindings.
- Adobe.ACPUserProfile.Android - The ACPUserProfile Android binding.
- ACPUserProfileTestApp - The Xamarin.Forms base app used by the iOS and Android test apps.
- ACPUserProfileTestApp.iOS - The Xamarin.Forms based iOS manual test app.
- ACPUserProfileTestApp.Android - The Xamarin.Forms based Android manual test app.
- ACPUserProfileiOSUnitTests - iOS unit test app.
- ACPUserProfileAndroidUnitTests - Android unit test app.

### [User Profile](https://aep-sdks.gitbook.io/docs/using-mobile-extensions/profile)

#### Initialization

**iOS:**
```c#
// Import the SDK
using Com.Adobe.Marketing.Mobile;

public override bool FinishedLaunching(UIApplication app, NSDictionary options)
{
  global::Xamarin.Forms.Forms.Init();
  LoadApplication(new App());

  // set the wrapper type
  ACPCore.SetWrapperType(ACPMobileWrapperType.Xamarin);
  
  // set launch config
  ACPCore.ConfigureWithAppID("yourAppId");

  // register SDK extensions
  ACPUserProfile.RegisterExtension();

  // start core
  ACPCore.Start(null);

  // register dependency service to link interface from App base project
  DependencyService.Register<IExtensionService, ExtensionService>();
  return base.FinishedLaunching(app, options);
}
```

**Android:**

```c#
// Import the SDK
using Com.Adobe.Marketing.Mobile;

protected override void OnCreate(Bundle savedInstanceState)
{
  TabLayoutResource = Resource.Layout.Tabbar;
  ToolbarResource = Resource.Layout.Toolbar;
  
  base.OnCreate(savedInstanceState);

  global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
  LoadApplication(new App());
  
  // set the wrapper type
  ACPCore.SetWrapperType(WrapperType.Xamarin);
  
  // set launch config
  ACPCore.ConfigureWithAppID("yourAppId");

  // register SDK extensions
  ACPCore.Application = this.Application;
  ACPUserProfile.RegisterExtension();

  // start core
  ACPCore.Start(null);

  // register dependency service to link interface from App base project
  DependencyService.Register<IExtensionService, ExtensionService>();
}
```

#### User Profile methods

##### Getting User Profile version:

**iOS and Android**

```c#
Console.WriteLine(ACPUserProfile.ExtensionVersion);
```

##### Update a user attribute:

**iOS and Android**

```c#
ACPUserProfile.UpdateUserAttribute("key", "value");
```

##### Update user attributes:

**iOS**

```c#
var attributes = new NSMutableDictionary<NSString, NSString>
{
  ["key1"] = new NSString("value1"),
  ["key2"] = new NSString("value2")
};
ACPUserProfile.UpdateUserAttributes(attributes);
```

**Android**

```c#
var attributes = new Dictionary<string, Java.Lang.Object>();
attributes.Add("key1", "value1");
attributes.Add("key2", "value2");
ACPUserProfile.UpdateUserAttributes(attributes);
```

##### Remove a user attribute:

**iOS and Android**

```c#
ACPUserProfile.RemoveUserAttribute("key");
```

##### Remove user attributes:

**iOS**

```c#
string[] keysToRemove = new string[] { "key1", "key3" };
ACPUserProfile.RemoveUserAttributes(keysToRemove);
```

**Android**

```c#
var keysToRemove = new List<string>();
keysToRemove.Add("key1");
keysToRemove.Add("key3");
ACPUserProfile.RemoveUserAttributes(keysToRemove);
```

##### Get currently stored user attributes:

**iOS**

```c#
var callback = new Action<NSDictionary, NSError>(handleCallback);
var keysToRetrieve = new string[] { "key1", "key2", "key3", "key4" };
ACPUserProfile.GetUserAttributes(keysToRetrieve, callback);

private void handleCallback(NSDictionary content, NSError error)
{
  if (error != null)
  {
    Console.WriteLine("GetUserAttributes error:" + error.DebugDescription);
  }
  else if (content == null)
  {
    Console.WriteLine("GetUserAttributes callback is null.");
  }
  else
  {
    foreach (KeyValuePair<NSObject, NSObject> pair in content)
    {
      Console.WriteLine("[ " + pair.Key + " : " + pair.Value + " ]");
    }
  }
}
```

**Android**

```c#
var keysToRetrieve = new List<string>();
keysToRetrieve.Add("key1");
keysToRetrieve.Add("key2");
keysToRetrieve.Add("key3");
keysToRetrieve.Add("key4");
ACPUserProfile.GetUserAttributes(keysToRetrieve, new AdobeCallback());

class AdobeCallback : Java.Lang.Object, IAdobeCallbackWithError
{
  public void Fail(AdobeError error)
  {
    Console.WriteLine("GetUserAttributes error:" + error.ToString());
  }

  public void Call(Java.Lang.Object retrievedAttributes)
  {
    if (retrievedAttributes != null)
    {
      var attributesDictionary = new Android.Runtime.JavaDictionary<string, object>(retrievedAttributes.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);
      foreach (KeyValuePair<string, object> pair in attributesDictionary)
      {
        Console.WriteLine("[ " + pair.Key + " : " + pair.Value + " ]");
      }
    }
    else
    {
      Console.WriteLine("GetUserAttributes callback is null.");
    }
  }
}
```

##### Running Tests

iOS and Android unit tests are included within the ACPUserProfile binding solution. They must be built from within Visual Studio then manually triggered from the unit test app that is deployed to an iOS or Android device.

## Sample App

A Xamarin Forms sample app is provided in the Xamarin ACPUserProfile solution file.

## Contributing
Looking to contribute to this project? Please review our [Contributing guidelines](.github/CONTRIBUTING.md) prior to opening a pull request.

We look forward to working with you!

## Licensing
This project is licensed under the Apache V2 License. See [LICENSE](LICENSE) for more information.
