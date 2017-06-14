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
        public ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;
        //private Spieler spieler;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Uebersicht);
            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
 
            mViewPager.Adapter = new MyPagerAdapter(SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;
            
            //Entgegennehmen Spieler
            //spieler = JsonConvert.DeserializeObject<Spieler>(Intent.GetStringExtra("spieler"));
        
            

        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_map, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
           
                return base.OnOptionsItemSelected(item);
               
           
        }

    }


    public class MyPagerAdapter : Android.Support.V4.App.FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;

        public MyPagerAdapter(Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
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
        private EditText etSuche;
        private ListView lvMonsterarten;
        private TextView txtName;
        private TextView txtTyp;
        private TextView txtHp;
        private TextView txtStarkGegen;
        private List<Monsterart> monsterarten;

        private ListViewAdapterMonster adapter;
        private List<Monsterart> selected;
        private bool nameAsc;
        private bool hpAsc;
        private bool typAsc;
        private bool starkGegenAsc;
        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragMonsterLayout, container, false);

            monsterarten = JsonConvert.DeserializeObject<List<Monsterart>>(Activity.Intent.GetStringExtra("monsterarten")); //@To-Do In dieser Activiity einlesen 
            selected = monsterarten;
            txtName = view.FindViewById<TextView>(Resource.Id.textViewNameUebersichtMonster);
            txtTyp = view.FindViewById<TextView>(Resource.Id.textViewTypUebersichtMonster);
            txtHp = view.FindViewById<TextView>(Resource.Id.textViewHpUebersichtMonster);
            txtStarkGegen = view.FindViewById<TextView>(Resource.Id.textViewStarkGegenUebersichtMonster);

            txtName.Click += TxtName_Click;
            txtTyp.Click += TxtTyp_Click;
            txtHp.Click += TxtHp_Click;
            txtStarkGegen.Click += TxtStarkGegen_Click;

            etSuche = view.FindViewById<EditText>(Resource.Id.editTextSucheUebersichtMonster);
            etSuche.TextChanged += TxtSuche_TextChanged;
            lvMonsterarten = view.FindViewById<ListView>(Resource.Id.listViewMonsterarten_Uebersicht);

            adapter = new ListViewAdapterMonster(Activity, monsterarten);
            
            lvMonsterarten.Adapter = adapter;

            lvMonsterarten.ItemClick += LvMonsterarten_ItemClick;
            lvMonsterarten.ItemLongClick += LvMonsterarten_ItemLongClick;
            return view;
        }

        private void TxtStarkGegen_Click(object sender, EventArgs e)
        {
            List<Monsterart> gefiltert;

            if (!starkGegenAsc)
            {
                gefiltert = (from monsterart in selected orderby monsterart.Typ.Starkgegen select monsterart).ToList<Monsterart>();
                adapter = new ListViewAdapterMonster(Activity, gefiltert);
                lvMonsterarten.Adapter = adapter;
            }
            else
            {
                gefiltert = (from monsterart in selected orderby monsterart.Typ.Starkgegen descending select monsterart).ToList<Monsterart>();
                adapter = new ListViewAdapterMonster(Activity, gefiltert);
                lvMonsterarten.Adapter = adapter;
            }
            starkGegenAsc = !starkGegenAsc;
        }

        private void TxtHp_Click(object sender, EventArgs e)
        {
            List<Monsterart> gefiltert;

            if (!hpAsc)
            {
                gefiltert = (from monsterart in selected orderby monsterart.Maxhp select monsterart).ToList<Monsterart>();
                adapter = new ListViewAdapterMonster(Activity, gefiltert);
                lvMonsterarten.Adapter = adapter;
            }
            else
            {
                gefiltert = (from monsterart in selected orderby monsterart.Maxhp descending select monsterart).ToList<Monsterart>();
                adapter = new ListViewAdapterMonster(Activity, gefiltert);
                lvMonsterarten.Adapter = adapter;
            }
            hpAsc = !hpAsc;
        }

        private void TxtTyp_Click(object sender, EventArgs e)
        {
            List<Monsterart> gefiltert;

            if (!typAsc)
            {
                gefiltert = (from monsterart in selected orderby monsterart.Typ.Name select monsterart).ToList<Monsterart>();
                adapter = new ListViewAdapterMonster(Activity, gefiltert);
                lvMonsterarten.Adapter = adapter;
            }
            else
            {
                gefiltert = (from monsterart in selected orderby monsterart.Typ.Name descending select monsterart).ToList<Monsterart>();
                adapter = new ListViewAdapterMonster(Activity, gefiltert);
                lvMonsterarten.Adapter = adapter;
            }
            typAsc = !typAsc;
        }

        private void TxtName_Click(object sender, EventArgs e)
        {
            List<Monsterart> gefiltert;

            if (!nameAsc)
            {
                gefiltert = (from monsterart in selected orderby monsterart.Name select monsterart).ToList<Monsterart>();
                adapter = new ListViewAdapterMonster(Activity, gefiltert);
                lvMonsterarten.Adapter = adapter;
            }
            else
            {
                gefiltert = (from monsterart in selected orderby monsterart.Name descending select monsterart).ToList<Monsterart>();
                adapter = new ListViewAdapterMonster(Activity, gefiltert);
                lvMonsterarten.Adapter = adapter;
            }
            nameAsc = !nameAsc;
        }

        private void TxtSuche_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            selected = (from monsterart in monsterarten where monsterart.Name.ToLower().Contains(etSuche.Text.ToLower()) || monsterart.Typ.Name.ToLower().Contains(etSuche.Text.ToLower()) select monsterart).ToList<Monsterart>();
            adapter = new ListViewAdapterMonster(Activity, selected);
            lvMonsterarten.Adapter = adapter;
        }

        private void LvMonsterarten_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            //Anzeigen ausführliche Infos
        }

        private void LvMonsterarten_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Anzeigen Bild
        }

        public override string ToString()
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
        public override string ToString()
        {
            return "Stats";
        }
    }
}