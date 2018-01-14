using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Media;
using Android.Hardware;
using System.Text;
using Android.Content;

namespace JokerMemoryApp
{
    [Activity(Label = "JokerMemoryApp", MainLauncher = true)]
    public class MainActivity : Activity, ISensorEventListener
    {
        private ViewCreator viewCreator;
        private JokerSoundPlayer jokerSoundPlayer;
        private MySensor mySensor;

        private SensorManager _sensorManager;
        private Sensor _Accelerometer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            viewCreator = new ViewCreator();
            jokerSoundPlayer = new JokerSoundPlayer();
            mySensor = new MySensor(this);

            _sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _Accelerometer = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);


            SetContentView(viewCreator.Layout);

#if true
            mySensor.ShowSenserList(viewCreator.Text);
#endif

            viewCreator.Button.Click += (sender, e) =>
            {
                Toast.MakeText(this, "メッセージ", ToastLength.Short).Show();
                jokerSoundPlayer.PlayClickedSound();
            };

        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            throw new System.NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {
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

