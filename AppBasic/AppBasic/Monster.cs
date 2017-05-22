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
    class Monster
    {
        private int monsterId;
        private int hp;
        private int atk;
        private String name;
        private String nickname;

        public int Hp { get => hp; set => hp = value; }
        public int Atk { get => atk; set => atk = value; }
        public string Name { get => name; set => name = value; }
        public int MonsterId { get => monsterId; set => monsterId = value; }
        public string Nickname { get => nickname; set => nickname = value; }
    }
}