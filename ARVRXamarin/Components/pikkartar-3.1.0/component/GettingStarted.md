# Pikkart AR SDK Xamarin Component
This guide is designed to help you in your very first steps with Pikkart AR SDK  for developing your augmented reality project with image recognition.
In [Pikkart Developer Portal](https://developer.pikkart.com) you will find other [tutorials](https://developer.pikkart.com/servizi/Menu/dinamica.aspx?ID=1570) regarding also augmented reality based on geolocation services.


### Android SDK API Usage
1.	Copy the license file (license.spz) we provided to you (or the trial license) inside your app assets dir (<project-root>/Assets/, create this dir if it doesn't exist).
2.	Add the following permissions in the Android Manifest tab in the project properties :
- CAMERA
- READ_EXTERNAL_STORAGE 
- WRITE_EXTERNAL_STORAGE 
3.	 In the Manifest.xml file add the following permissions in the <manifest> tag
```sh
<uses-feature android:name="android.hardware.camera" android:required="true" /> 
<uses-feature android:glEsVersion="0x00020000" android:required="true" /> 
<uses-sdk android:targetSdkVersion="21" android:minSdkVersion="15"/>
```
4.	If you plan to use Pikkart's Cloud Recognition Service also add the following permissions:
- INTERNET 
- ACCESS_NETWORK_STATE 
- ACCESS_WIFI_STATE 
5.	For Android 6+ the permission mechanism is a little different, use this code to handle the permissions or see the [Android Documentation](https://developer.android.com/training/permissions/requesting.html)

```sh
private int _permissionCode = 1234;

protected override void OnCreate(Bundle bundle) 
{
	base.OnCreate(bundle);
	if (Build.Version.SdkInt < BuildVersionCode.M)
	{
		//you don’t have to do anything, just init your app
		InitApp();
	}
	else 
	{
		CheckPermissions(_permissionCode);
	}			
}

private void CheckPermissions(int code)
{
		string[] permissions_required = new String[] {
			Manifest.Permission.Camera,
			Manifest.Permission.WriteExternalStorage,
			Manifest.Permission.ReadExternalStorage 
		};
	List<string> permissions_not_granted_list = new List<string>();
	foreach (string permission in permissions_required)
 	{             
		if (ActivityCompat.CheckSelfPermission(ApplicationContext, permission) != Permission.Granted)
		{
			permissions_not_granted_list.Add(permission); 
		}
	}
	if (permissions_not_granted_list.Count > 0) 
	{
		String[] permissions = new String[permissions_not_granted_list.Count]; 
		permissions = permissions_not_granted_list.ToArray(); ActivityCompat.RequestPermissions(this, permissions, code); 
	}
	else
	{
		InitApp();
	}
}

public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
{
	if (requestCode == m_permissionCode)
	{
		bool ok = true;
		for (int i = 0; i < grantResults.Length; ++i)
		{
			ok = ok && (grantResults[i] == Permission.Granted);
		}
		if (ok)
		{
			InitApp();
		}
		else	
		{
			Toast.MakeText(this, "Error: required permissions not granted!", ToastLength.Short).Show();
		Finish();
		}
	}
}
```





6.	The activity holding Pikkart's AR fragment ( Com.Pikkart.Ar.Recognition.RecognitionFragment ) must have set ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize in the ActivityAttribute as in the following example:
```sh
[Activity(Label = “XamarinAndroidGettingStarted”, MainLauncher = true, Icon = “@drawable/icon”, Theme = “@style/AppTheme”, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity : AppCompatActivity, IRecognitionListener 
{
	...
} 
```
7.	Add Pikkart's AR Recognition Fragment to your AR activity. You can add it through your app layout XML file as:
```sh
<?xml version="1.0" encoding="utf-8"?>
<FrameLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_height="match_parent" 
    android:layout_width="match_parent"
    android:id="@+id/rootLayout">
<fragment android:layout_width="match_parent" 
    android:layout_height="match_parent"
    android:id="@+id/pikkart_ar_fragment"
    android:name="com.pikkart.ar.recognition.RecognitionFragment" />
</FrameLayout>
```
8.	All is set, now you just have to start the recognition process. You can do it (as an example) directly in you activity's OnCreate function: 
```sh
_cameraFragment = FragmentManager.FindFragmentById<RecognitionFragment>(Resource.Id.pikkart_ar_fragment);
_cameraFragment.StartRecognition(new RecognitionOptions(
                    RecognitionOptions.RecognitionStorage.Local, 
                    RecognitionOptions.RecognitionMode.ContinuousScann, 
                    new CloudRecognitionInfo(new String[]{})
                ), this);
```
9.	In the previous example we are using LOCAL marker recognition only (RecognitionManager.RecognitionStorage.Local), in this case you also need to add marker files to your app Asset folder. Move to that folder (usually <project-root>/Assets/) or create it, create a new directory "markers" and copy into it the local marker you want your app to recognize. You can create and download those files using our Marker Manager Web App or you can use the ones provided with the SDK Sample package ( you can find the marker files in <sample-root>/markers/).
10.	As second parameter to our _cameraFragment.StartRecognition  function we have to pass a reference to a class implementing the interface IRecognitionListener  , in our example we are passing a reference to the AR activity itself witch need to override the following callback functions:
```sh
public void ExecutingCloudSearch() { 
    //TODO: add your code here 
}
```
Called every time Pikkart's AR sdk send a cloud recognition search query to our cloud recognition service.
```sh
public void CloudMarkerNotFound() { 
    //TODO: add your code here 
}
```
Called when a cloud recognition search query fails to find the captured image
```sh
public void InternetConnectionNeeded() { 
    //TODO: add your code here 
} 
```
Inform that internet connection is not available (i.e. the app is set to use our cloud recognition service but the user has disabled internet connection).
```sh
public void MarkerFound(Marker marker) { 
    //TODO: add your code here 
} 
```
Called when a marker search (either local or cloud based) successfully find a marker.
```sh
public void MarkerNotFound() { 
    //TODO: add your code here 
} 
```
Called when a marker search fails to find a marker.
```sh
public void MarkerTrackingLost(String markerId){ 
    //TODO: add your code here 
} 
```
Called when the SDK lose tracking of the given marker.
```sh
public void ARLogoFound(string markerId, int code) { 
    //TODO: add your code here  
}
```
Called after markerFound when the ARLogo on the marker is found.
```sh
public bool IsConnectionAvailable(Context context) { 
    //TODO: add your code here  
}
```
Inquire if a connection is available, to be implemented by the app developer. For a purely LOCAL search app you can simply return false here, otherwise use Android's NetworkInfo class
11.	Now you can use the various static functions and attributes of the RecognitionFragment class to get the state, matrices and marker data of the detected and tracked marker
```sh
public static bool IsTracking { get; }

public static Marker CurrentMarker { get; }

public static float[] GetCurrentProjectionMatrix();

public static float[] GetCurrentModelViewMatrix();

public static void RenderCamera(int viewportWidth, int viewportHeight, int angle);
```
The RenderCamera functions is particularly important because it allows your app to render the current camera image. This function needs to be called inside your OpenGL rendering cycle, see our tutorials for additional details.


### iOS SDK API Usage
1.	Copy the license file (license.spz) we provided to you inside your app main bundle

2.	Declare a subclass of PKTCameraController, our AR View Controller entry point  
```sh           
using Pikkart.ArSdk.Recognition;
public class RecognitionViewController : PKTCameraController,IPKTIRecognitionListener
```
3.	All is set, now you just have to start the recognition process. You can do it
	(as an example) setting PKTCameraController as app	RootViewController:

```sh  
Window.RootViewController = new PKTCameraController_SubClass();
string[] dbNames={""};
PKTCloudRecognitionInfo info = new PKTCloudRecognitionInfo(dbNames);
PKTRecognitionOptions options = new PKTRecognitionOptions(PKTRecognitionStorage.PKTLOCAL, PKTRecognitionMode.PKTRECOGNITION_CONTINUOS_SCAN, info);
StartRecognition(options,this);
```

4.	In the previous example we are using LOCAL marker recognition only(PKTRecognitionStorage.PKTLOCAL), and PKTRecognitionMode.PKTRECOGNITION_CONTINUOS_SCAN as recognition mode. In this case you also need to add marker files.
       
As second parameter to our StartRecognition()  method we have to pass an nstance of a class implementing the interface PKTIRecognitionListener. In our example we are passing  the AR PKT View Controller itself which need to implements the following callback functions:  
```sh  
#region IPKTIRecognitionListener methods
[Export ("executingCloudSearch")]
void ExecutingCloudSearch() {
	Console.WriteLine("ExecutingCloudSearch called!");
}

[Export ("cloudMarkerNotFound")]
void CloudMarkerNotFound() {
	Console.WriteLine("CloudMarkerNotFound called!");
}

[Export ("internetConnectionNeeded")]
void InternetConnectionNeeded() {
	   Console.WriteLine("InternetConnectionNeeded called!");
}
	
[Export ("markerFound:")]
void MarkerFound(PKTMarker marker) {
	Console.WriteLine("MarkerFound called with id = {0}!",marker.Id);
}

[Export ("markerNotFound")]
void markerNotFound() {
	Console.WriteLine("markerNotFound called!");
}

[Export ("markerTrackingLost:")]
void MarkerTrackingLost(string markerId) {
	Console.WriteLine("MarkerTrackingLost called! with Id = {0}", markerId);
}
#endregion
```	
Now you can use the various static functions of the PKTCameraController class to get the state, matrices and marker data of the detected and tracked marker 
```sh  
    public virtual void GetCurrentModelViewMatrix (ref IntPtr matrix);

	public virtual void GetCurrentProjectionMatrix (ref IntPtr matrix);
	
	public virtual bool isActive ();

	public virtual bool isTracking ();

	public virtual void RenderCamera (CGSize viewPortSize, int angle);
```