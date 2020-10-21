using System;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Android.Provider;
using static Android.Gms.Vision.Detector;



namespace Smallpox
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    [MetaData("com.google.android.gms.vision.DEPENDENCIES", Value = "ocr")]
    public class MainActivity : AppCompatActivity, ISurfaceHolderCallback, IProcessor
    {
        private TextView textView;
        private SurfaceView cameraView;
        private CameraSource cameraSource;
        private const int REQUEST_CAMERA_PERMISSION_ID = 1001;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            textView = FindViewById<TextView>(Resource.Id.txtAux);
            cameraView = FindViewById<SurfaceView>(Resource.Id.surfaceView1);

            TextRecognizer textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();

            cameraSource = new CameraSource.Builder(ApplicationContext, textRecognizer)
                    .SetFacing(CameraFacing.Front)
                    .SetRequestedPreviewSize(1280, 1024)
                    .SetRequestedFps(2.0f)
                    .SetAutoFocusEnabled(true)
                    .Build();

            cameraView.Holder.AddCallback(this);
            textRecognizer.SetProcessor(this);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                var intent = new Intent(this, typeof(EnterpriseData));
                StartActivity(intent);
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            //Intent intent = new Intent(MediaStore.ActionImageCapture);
            //StartActivityForResult(intent, 0);
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[]
                {
                    Manifest.Permission.Camera
                }, REQUEST_CAMERA_PERMISSION_ID);
                return;
            }
            cameraSource.Start(cameraView.Holder);
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            cameraSource.Stop();
        }

        public void ReceiveDetections(Detections detections)
        {
            SparseArray items = detections.DetectedItems;
            if (items.Size() != 0)
            {
                for (int i = 0; i < items.Size(); i++)
                {
                    var line = ((TextBlock)items.ValueAt(i)).Value;
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.Contains("<<<"))
                    {
                        textView.Text = line;
                        //cameraSource.TakePicture()
                    }
                    if (line.Contains(">>>"))
                    {
                        textView.Text = line;
                    }
                }
                //textView.Post(() =>
                //{
                //    StringBuilder strBuilder = new StringBuilder();
                //    for (int i = 0; i < items.Size(); i++)
                //    {
                //        var line = ((TextBlock)items.ValueAt(i)).Value;
                //        if (string.IsNullOrEmpty(line)) continue;
                //    }
                //    textView.Text = strBuilder.ToString();
                //});
            }
        }

        public void Release()
        {

        }
    }
}

