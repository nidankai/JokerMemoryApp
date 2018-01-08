using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware;

namespace JokerMemoryApp
{
    class MySensor
    {
        private SensorManager _sensorManager;
        private IList<Sensor> _sensorList;
        private Sensor _Accelerometer;

        public MySensor(Context context) // システム情報の取得にはContextが必要のため
        {
            _sensorManager = (SensorManager)context.GetSystemService(Context.SensorService);
            _sensorList = _sensorManager.GetSensorList(SensorType.All);
            _Accelerometer = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);
        }

        public void ShowSenserList(TextView text)
        {
            if (text == null) { return; }

            StringBuilder strBuild = new StringBuilder();

            foreach (Sensor sensor in _sensorList)
            {
                strBuild.Append(sensor.Type);
                strBuild.Append(", ");
                strBuild.Append(sensor.Name);
                strBuild.Append(", ");
                strBuild.Append(sensor.Vendor);
                strBuild.Append("\n");
            }

            text.Text = strBuild.ToString();
        }

    }
}