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

namespace AppBasic
{
    class DialogAnzeigenMonsterbild : Android.Support.V4.App.DialogFragment
    {
        private int pic;

        public int Pic { get => pic; set => pic = value; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.DialogAnzeigenMonsterbildLayout, container, false);

            ImageView ivBild = view.FindViewById<ImageView>(Resource.Id.imageViewDialogAnzeigenMonsterbildImage);
           
            //eigentliches bild einfügen


            return view;
        }

        public DialogAnzeigenMonsterbild() : base()
        { }

        public DialogAnzeigenMonsterbild(int pic) : base()
        {
            Pic = pic;
        }
    }
}