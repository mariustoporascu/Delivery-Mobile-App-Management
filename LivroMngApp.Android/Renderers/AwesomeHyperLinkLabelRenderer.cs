﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text.Util;
using Android.Util;
using Android.Views;
using Android.Widget;
using LivroMngApp.Controls;
using LivroMngApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AwesomeHyperLinkLabel), typeof(AwesomeHyperLinkLabelRenderer))]
namespace LivroMngApp.Droid
{
    public class AwesomeHyperLinkLabelRenderer : LabelRenderer
    {
        public AwesomeHyperLinkLabelRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (AwesomeHyperLinkLabel)Element;
            if (view == null) return;

            TextView textView = new TextView(Context);
            textView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            textView.SetTextColor(view.TextColor.ToAndroid());

            // Setting the auto link mask to capture all types of link-able data
            textView.AutoLinkMask = MatchOptions.All;
            // Make sure to set text after setting the mask
            textView.Text = view.Text;
            textView.SetTextSize(ComplexUnitType.Dip, (float)view.FontSize);

            textView.Gravity = GravityFlags.Center;
            // overriding Xamarin Forms Label and replace with our native control
            SetNativeControl(textView);
        }
    }
}