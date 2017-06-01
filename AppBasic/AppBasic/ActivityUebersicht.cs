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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Newtonsoft.Json;

namespace AppBasic
{
    [Activity(Label = "ActivityUebersicht")]
    public class ActivityUebersicht : FragmentActivity
    {
        private ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;
        private Spieler spieler;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Uebersicht);
            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            mViewPager.Adapter = new SamplePagerAdapter(SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;

            //Entgegennehmen Spieler
            spieler = JsonConvert.DeserializeObject<Spieler>(Intent.GetStringExtra("spieler"));
        


        }
       

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_map, menu);
            return base.OnCreateOptionsMenu(menu);
        }
     
    }


    public class SamplePagerAdapter : Android.Support.V4.App.FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;

        public SamplePagerAdapter(Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
        {
            mFragmentHolder = new List<Android.Support.V4.App.Fragment>();
            mFragmentHolder.Add(new FragmentMonster());
            mFragmentHolder.Add(new FragmentStats());
        }
        public override int Count { get { return mFragmentHolder.Count; } }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return mFragmentHolder[position];
        }
    }
  

    public class FragmentMonster : Android.Support.V4.App.Fragment
    {
        private EditText mTxt;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragMonsterLayout, container, false);

            mTxt = view.FindViewById<EditText>(Resource.Id.editText1);
            mTxt.Text = "Monster";
            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Monster";
        }
    }

    public class FragmentStats : Android.Support.V4.App.Fragment
    {
        private TextView tvName;
        private TextView tvSpielzeit;
        private TextView tvKaempfeGes;
        private TextView tvKaempfeGew;
        private Spieler spieler;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragStatsLayout, container, false);

            spieler = JsonConvert.DeserializeObject<Spieler>(Activity.Intent.GetStringExtra("spieler"));
            tvName = view.FindViewById<TextView>(Resource.Id.textViewName);
            tvSpielzeit = view.FindViewById<TextView>(Resource.Id.textViewSpielzeit);
            tvKaempfeGes = view.FindViewById<TextView>(Resource.Id.textViewKaempfeGes);
            tvKaempfeGew = view.FindViewById<TextView>(Resource.Id.textViewKaempfeGewonnen);

            fuelleFelder();
            return view;
        }
        private void fuelleFelder()
        {
            tvName.Text = spieler.Name;
            tvSpielzeit.Text = spieler.Logdaten.Spielzeit.ToString();
            tvKaempfeGes.Text = spieler.Logdaten.KaempfeGesamt.ToString();
            tvKaempfeGew.Text = spieler.Logdaten.KaempfeGewonnen.ToString();
        }
        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Stats";
        }
    }
}