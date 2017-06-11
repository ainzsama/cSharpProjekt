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
using System.Collections;

namespace AppBasic
{
    public class Typ 
    {
        private int id;
        private String name;
        private int starkgegen;
        public Typ(int id,String name,int starkgegen)
        {
            Starkgegen = starkgegen;
            Name = name;
            Id = id;
        }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public int Starkgegen { get => starkgegen; set => starkgegen = value; }

        public static Typ GetTestTyp()
        {
            List<Typ> list = Typen.ErstelleTypen();
            return list.ElementAtOrDefault(5);
        }
    }
}