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
using Android.Gms.Maps.Model;

namespace AppBasic
{
    class Monster
    {
        private int monsterId;
        private int hp;
        private int atk;
        private String name;
        private String nickname;
        private Marker marker;

        public int Hp { get => hp; set => hp = value; }
        public int Atk { get => atk; set => atk = value; }
        public string Name { get => name; set => name = value; }
        public int MonsterId { get => monsterId; set => monsterId = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public Marker Marker { get => marker; set => marker = value; }

        public static Monster getTestMonster()
        {
            Monster m = new Monster();
            m.MonsterId = 1;
            m.Hp = 100;
            m.atk = 10;
            m.Name = "test";
            m.Nickname = "testNic";

            return m;
        }
    }
}