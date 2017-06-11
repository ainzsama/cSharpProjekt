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
    class ListViewAdapterMonster : BaseAdapter<Monsterart>
    {
        private List<Monsterart> monster;
        private Context context;

        public ListViewAdapterMonster(Context c, List<Monsterart> m)
        {
            context = c;
            monster = m;
        }

        public override Monsterart this[int position] { get { return monster[position]; } }

        public override int Count { get { return monster.Count; } }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.ListViewRowMonster, null, false);
            }

            TextView txtName = row.FindViewById<TextView>(Resource.Id.textViewMonsterListViewRowName);
            TextView txtAtk = row.FindViewById<TextView>(Resource.Id.textViewMonsterListViewRowAtk);
            TextView txtXp = row.FindViewById<TextView>(Resource.Id.textViewMonsterListViewRowXp);
            txtName.Text = monster[position].Name;
            txtAtk.Text = monster[position].Typ.Id.ToString();
            txtXp.Text = monster[position].Sterbexp.ToString();
            return row;
        }

     }
}