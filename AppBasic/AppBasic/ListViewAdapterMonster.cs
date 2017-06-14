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
            TextView txtTyp = row.FindViewById<TextView>(Resource.Id.textViewMonsterListViewRowTyp);
            TextView txtHp = row.FindViewById<TextView>(Resource.Id.textViewMonsterListViewRowHp);
            TextView txtStarkGegen = row.FindViewById<TextView>(Resource.Id.textViewMonsterListViewRowStarkGegen);
            txtName.Text = monster[position].Name;
            txtTyp.Text = monster[position].Typ.Name;
            txtHp.Text = monster[position].Maxhp.ToString();
            txtStarkGegen.Text = monster[position].Typ.Starkgegen.ToString();
            return row;
        }

     }
}