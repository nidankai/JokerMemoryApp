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
        private ViewCreator _viewCreator;
        private JokerSoundPlayer _jokerSoundPlayer;
        private SensorManager _sensorManager;
        private Sensor _accelerometer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewCreator = new ViewCreator();
            SetContentView(_viewCreator.Layout);

            _jokerSoundPlayer = new JokerSoundPlayer();

            _sensorManager = (SensorManager)GetSystemService(SensorService);
            _accelerometer = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);

            _viewCreator.Button.Click += (sender, e) =>
            {
                Toast.MakeText(this, "メッセージ", ToastLength.Short).Show();
                _jokerSoundPlayer.PlayClickedSound();
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this, _accelerometer, SensorDelay.Normal);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            //throw new System.NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {
            //throw new System.NotImplementedException();
            StringBuilder strBuild = new StringBuilder();

            strBuild.Append("X軸");
            strBuild.Append(e.Values[0]);
            strBuild.Append(",\n ");
            strBuild.Append("Y軸");
            strBuild.Append(e.Values[1]);
            strBuild.Append(",\n");
            strBuild.Append("Z軸");
            strBuild.Append(e.Values[2]);
            strBuild.Append(",\n");

            _viewCreator.Text.Text = strBuild.ToString();
        }
    }
}

