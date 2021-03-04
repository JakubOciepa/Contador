using System;
using System.ComponentModel;

using Android.Content;
using Android.Widget;

using Contador.Mobile.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Label), typeof(LabelWithFixedTextSizeRenderer))]

namespace Contador.Mobile.Droid.Renderers
{
	/// <summary>
	/// Renderer for the label text fixed size.
	/// </summary>
	public class LabelWithFixedTextSizeRenderer : LabelRenderer
	{
		/// <summary>
		/// Creates instance of the <see cref="LabelWithFixedTextSizeRenderer"/> class.
		/// </summary>
		/// <param name="context"></param>
		public LabelWithFixedTextSizeRenderer(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
		{
			base.OnElementChanged(e);
			SetFontSizeAgain();
		}


		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			SetFontSizeAgain();
		}

		/// <summary>
		/// Sets the fixed size of the label text.
		/// </summary>
		protected void SetFontSizeAgain()
		{
			var xfControl = Element;

			if (Control is TextView nativeControl && xfControl is object)
			{
				float size = Convert.ToInt64(xfControl.FontSize);
				nativeControl.SetTextSize(Android.Util.ComplexUnitType.Dip, size);
			}
		}
	}
}
