using System;
using System.ComponentModel;

using Android.Content;
using Android.Widget;

using Contador.Mobile.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Label), typeof(DpLabelRenderer))]
namespace Contador.Mobile.Droid.Renderers
{
	public class DpLabelRenderer : LabelRenderer
	{
		public DpLabelRenderer(Context context) : base(context)
		{
		}

		protected void setFontSizeAgain()
		{
			var nativeControl = (TextView)Control;
			var xfControl = Element;
			if (nativeControl != null && xfControl != null)
			{
				float size = Convert.ToInt64(xfControl.FontSize);
				nativeControl.SetTextSize(Android.Util.ComplexUnitType.Dip, size);
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
		{
			base.OnElementChanged(e);
			setFontSizeAgain();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			setFontSizeAgain();
		}
	}
}
