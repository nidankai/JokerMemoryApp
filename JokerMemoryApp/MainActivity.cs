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
        private Sensor _magField;

        private readonly float k = 0.1F;
        private float lowPassX;
        private float lowPassY;
        private float lowPassZ;
        private float rawAx;
        private float rawAy;
        private float rawAz;
        private double ax;
        private double ay;
        private double az;
        //test
        private readonly int MATRIX_SIZE = 16;
        private float[] mgValues = new float[3];
        private float[] acValues = new float[3];

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewCreator = new ViewCreator();
            SetContentView(_viewCreator.Layout);
            _jokerSoundPlayer = new JokerSoundPlayer();
            _sensorManager = (SensorManager)GetSystemService(SensorService);
            _accelerometer = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            _magField = _sensorManager.GetDefaultSensor(SensorType.MagneticField);

            _viewCreator.Button.Click += (sender, e) =>
            {
                Toast.MakeText(this, "メッセージ", ToastLength.Short).Show();
                _jokerSoundPlayer.PlayClickedSound();
            };
        }
        //test
        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this, _accelerometer, SensorDelay.Normal);
            _sensorManager.RegisterListener(this, _magField, SensorDelay.Normal);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this, _accelerometer);
            _sensorManager.UnregisterListener(this, _magField);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            //throw new System.NotImplementedException();
        }

        private int Rad2Deg(float rad)
        {
            return (int)Math.Floor(rad * 180 / Math.PI);
        }

        public void OnSensorChanged(SensorEvent e)
        {
            //throw new System.NotImplementedException();
            StringBuilder strBuild = new StringBuilder();

            float[] inR = new float[MATRIX_SIZE];
            float[] outR = new float[MATRIX_SIZE];
            float[] I = new float[MATRIX_SIZE];
            float[] orValues = new float[3];

            double nPitchRad = 0;
            double sinNPitch = 0;
            double cosNPitch = 0;

            double nRollRad = 0;
            double sinNRoll = 0;
            double cosNRoll = 0;

            double bx, by; // 一時退避

            double nAzimuthRad;
            double sinNAzimuth;
            double cosNAzimuth;

            switch (e.Sensor.Type)
            {
                case SensorType.Accelerometer:
                    e.Values.CopyTo(acValues, 0);
                    break;
                case SensorType.MagneticField:
                    e.Values.CopyTo(mgValues, 0);
                    break;
            }

            if (mgValues != null && acValues != null)
            {
                SensorManager.GetRotationMatrix(inR, I, acValues, mgValues);

                SensorManager.RemapCoordinateSystem(inR, Axis.X, Axis.Y, outR);
                SensorManager.GetOrientation(outR, orValues);

                lowPassX = (acValues[0] - lowPassX) * k;
                lowPassY = (acValues[1] - lowPassY) * k;
                lowPassZ = (acValues[2] - lowPassZ) * k;

                rawAx = acValues[0] - lowPassX;
                rawAy = acValues[1] - lowPassY;
                rawAz = acValues[2] - lowPassZ;

                // ピッチ・ロール
                nPitchRad = (orValues[1] * 180 / Math.PI);
                sinNPitch = Math.Sin(nPitchRad);
                cosNPitch = Math.Cos(nPitchRad);

                nRollRad = (orValues[2] * 180 / Math.PI);
                sinNRoll = Math.Sin(nRollRad);
                cosNRoll = Math.Cos(nRollRad);

                bx = rawAx * cosNRoll + rawAz * sinNRoll;
                by = rawAx * sinNPitch * sinNRoll + rawAy * cosNPitch - rawAz * sinNPitch * cosNRoll;
                az = -rawAx * cosNPitch * sinNRoll + rawAy * sinNPitch * cosNRoll + rawAz * cosNPitch * cosNRoll;

                nAzimuthRad = (orValues[0] * 180 / Math.PI);
                sinNAzimuth = Math.Sin(nAzimuthRad);
                cosNAzimuth = Math.Cos(nAzimuthRad);

                ax = bx * cosNAzimuth - by * sinNAzimuth;
                ay = bx * sinNAzimuth + by * cosNAzimuth;
                Console.WriteLine(MethodInfo.GetCurrentMethod());
            }

            strBuild.Append("X軸");
            strBuild.Append(lowPassX);
            strBuild.Append(",\n");
            strBuild.Append("Y軸");
            strBuild.Append(lowPassY);
            strBuild.Append(",\n");
            strBuild.Append("Z軸");
            strBuild.Append(lowPassZ);
            strBuild.Append(",\n");

            strBuild.Append("方位角（ラジマス）");
            strBuild.Append(Rad2Deg(orValues[0]));
            strBuild.Append(",\n");
            strBuild.Append("傾斜角（ピッチ）");
            strBuild.Append(Rad2Deg(orValues[1]));
            strBuild.Append(",\n");
            strBuild.Append("回転角（ロール）");
            strBuild.Append(Rad2Deg(orValues[2]));
            strBuild.Append(",\n");

            strBuild.Append("x座標");
            strBuild.Append(ax);
            strBuild.Append(",\n");
            strBuild.Append("y座標");
            strBuild.Append(ay);
            strBuild.Append(",\n");
            strBuild.Append("z座標");
            strBuild.Append(az);
            strBuild.Append(",\n");

            _viewCreator.Text.Text = strBuild.ToString();
        }

    }
}

