using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace JokerMemoryApp
{
    class ViewCreator
    {
        public RelativeLayout Layout { get; private set; }
        public Button Button { get; private set; }
        public TextView Text { get; private set; }

        public ViewCreator()
        {
            CreateView();

            Button.SetText(Resource.String.app_name2);
            Button.SetX(100);
            Layout.AddView(Button);

            Text.SetText(Resource.String.app_name2);
            Text.SetY(100);
            Layout.AddView(Text);
        }

        public void CreateView()
        {
            Layout = new RelativeLayout(Application.Context);
            Button = new Button(Application.Context);
            Text = new TextView(Application.Context);
        }

        public void DestroyView()
        {
            Layout.RemoveAllViews();
        }

    }
}