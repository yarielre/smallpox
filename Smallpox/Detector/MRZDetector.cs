using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Vision;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Smallpox
{
    public class MRZDetector : Detector
    {
        public override SparseArray Detect(Frame p0)
        {
            Console.WriteLine();
            return new SparseArray();
        }
    }
}