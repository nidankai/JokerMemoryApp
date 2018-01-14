using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Content;
using Android.Runtime;
using System;
using System.Reflection;
using System.Text;

namespace JokerMemoryApp
{
    [Activity(Label = "JokerMemoryApp", MainLauncher = true)]
    public class MainActivity : Activity, ISensorEventListener
    {
        private ViewCreator viewCreator;
        private SensorManager sensorManager;
        private Sensor mAcc;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.Main);

            viewCreator = new ViewCreator();
            SetContentView(viewCreator.Layout);

            sensorManager = (SensorManager)GetSystemService(SensorService);
            mAcc = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
        }

        protected override void OnResume()
        {
            base.OnResume();
            sensorManager.RegisterListener(this, mAcc, SensorDelay.Normal);
        }

        protected override void OnPause()
        {
            base.OnPause();
            sensorManager.UnregisterListener(this);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            //throw new System.NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {
            //throw new System.NotImplementedException();
            Console.WriteLine(MethodBase.GetCurrentMethod());
            StringBuilder strBuild = new StringBuilder();

            strBuild.Append("X軸");
            strBuild.Append(e.Values[0]);
            strBuild.Append("\n ");
            strBuild.Append("Y軸");
            strBuild.Append(e.Values[1]);
            strBuild.Append(",\n");
            strBuild.Append("Z軸");
            strBuild.Append(e.Values[2]);
            strBuild.Append("\n");

            viewCreator.Text.Text = strBuild.ToString();
        }
    }
}

