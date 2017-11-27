using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android;
using Android.Content.PM;
using System;
using Android.Runtime;
using Com.Pikkart.AR.Recognition.Data;
using Com.Pikkart.AR.Recognition;
using Android.Support.V7.App;
using Android.Content;
using Com.Pikkart.AR.Recognition.Items;
using Android.Support.V4.App;

namespace ARVRXamarin
{
    [Activity(Label = "ARVRXamarin", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IRecognitionListener
    {
        private int _permissionCode = 1234;
        private RecognitionFragment _cameraFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if (Build.VERSION.SdkInt < Build.VERSION_CODES.M)
            {
                //you don’t have to do anything, just init your app
                InitApp();
            }
            else
            {
                CheckPermissions(_permissionCode);
            }

        }

        private void InitApp()
        {
            _cameraFragment = FragmentManager.FindFragmentById<RecognitionFragment>(Resource.Id.pikkart_ar_fragment);
            _cameraFragment.StartRecognition(new RecognitionOptions(RecognitionOptions.RecognitionStorage.Local, RecognitionOptions.RecognitionMode.ContinuousScan, new CloudRecognitionInfo(new String[] { })), this);
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
            if (requestCode == _permissionCode)
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

        public void ARLogoFound(string p0, int p1)
        {
            throw new NotImplementedException();
        }

        public void CloudMarkerNotFound()
        {
            throw new NotImplementedException();
        }

        public void ExecutingCloudSearch()
        {
            throw new NotImplementedException();
        }

        public void InternetConnectionNeeded()
        {
            throw new NotImplementedException();
        }

        public void MarkerFound(Marker p0)
        {
            throw new NotImplementedException();
        }

        public void MarkerNotFound()
        {
            throw new NotImplementedException();
        }

        public void MarkerTrackingLost(string p0)
        {
            throw new NotImplementedException();
        }

        public bool IsConnectionAvailable(Context p0)
        {
            throw new NotImplementedException();
        }
    }
}

