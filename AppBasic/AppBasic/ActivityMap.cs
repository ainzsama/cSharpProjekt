using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.App;
using Android.Gms.Maps;
using Android.Locations;
using Android.Util;
using Android.Gms.Maps.Model;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Gms.Common;
using Android;
using Android.Content.PM;
using Android.Gms.Tasks;
using Android.Support.V7.App;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Threading;
using System.IO;
using System.Xml.Serialization;

namespace AppBasic
{
    [Activity(Label = "ActivityMap")]
    public class ActivityMap : Activity, IOnMapReadyCallback, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener, Android.Gms.Maps.GoogleMap.IInfoWindowAdapter, Android.Gms.Maps.GoogleMap.IOnInfoWindowClickListener
    {
        private readonly string[] PermissionsLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        const int RequestLocationId = 0;


        private GoogleApiClient apiClient;
        private LocationRequest locRequest;
        private GoogleMap mMap;
        private Spieler spieler;
        private List<Monster> activeMonsters;
        private bool _isGooglePlayServicesInstalled;
        private List<Monsterart> monsterarten;
        private List<Angriff> angriffe;
        private bool requestingLocationUpdates;
        private Button btnUebersicht;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Map);


            btnUebersicht = FindViewById<Button>(Resource.Id.buttonUebersicht);
            btnUebersicht.Click += OnClickUebersicht;


            // Entgegennehmen Spieler
            spieler = JsonConvert.DeserializeObject<Spieler>(Intent.GetStringExtra("spieler"));
            activeMonsters = new List<Monster>();
            requestingLocationUpdates = false;
            SetUpMap();
            _isGooglePlayServicesInstalled = IsGooglePlayServicesInstalled();

            //RequestPermissions(PermissionsLocation, RequestLocationId);
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted && ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
            {
                Log.Debug("OnCreate", "Permissions ok, ");
            }
            else
            {
                Log.Debug("OnCreate", "Permissions not ok, requesting...");
                RequestPermissions(PermissionsLocation, RequestLocationId);
            }

            if (_isGooglePlayServicesInstalled)
            {
                Log.Debug("OnCreate", "Build ApiClient and connect...");
                apiClient = BuildApiClient();
                apiClient.Connect();
                try
                {
                    CreateLocationRequest();
                }catch
                {

                }
            }
            else
            {
                Log.Error("OnCreate", "Google Play Services nicht installiert");
                Toast.MakeText(this, "Google Play Services nicht installiert", ToastLength.Long).Show();

            }


             monsterarten = EinlesenMonsterarten();
             angriffe = EinlesenAngriffe();

        }

        private GoogleApiClient BuildApiClient()
        {
            GoogleApiClient client = new GoogleApiClient.Builder(this, this, this)
                .AddApi(LocationServices.API)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .Build();

            return client;
        }

        private void CreateLocationRequest()
        {
            locRequest = new LocationRequest();
            locRequest.SetInterval(5000);
            locRequest.SetFastestInterval(4000);
            locRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
        }

        protected async void StartLocationUpdates()
        {
            if (!requestingLocationUpdates)
            {
                if (locRequest == null) CreateLocationRequest();

                if (apiClient.IsConnected && locRequest != null)
                {
                    try
                    {

                        await LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locRequest, this);
                        requestingLocationUpdates = true;
                    } catch
                    {
                        Log.Debug("StartLocationUpdates", "permissions not available, requesting now...");
                        requestingLocationUpdates = false;
                        RequestPermissions(PermissionsLocation, RequestLocationId);
                    }
                }

                else
                {
                    if (apiClient == null) apiClient = BuildApiClient();
                    if (!apiClient.IsConnected && !apiClient.IsConnecting) apiClient.Connect();
                   
                }
            }
            else Log.Debug("StartLocationUpdates", "Allready requesting location updates...");
        }

        protected async void  StopLocationUpdates()
        {
            await LocationServices.FusedLocationApi.RemoveLocationUpdates(apiClient, this);
        }

        private void OnClickUebersicht(object sender, EventArgs e)
        {
            Intent actUebersicht = new Intent(this, typeof(ActivityUebersicht));

            //Übergabe Spieler
            actUebersicht.PutExtra("spieler", Intent.GetStringExtra("spieler"));
           
            StartActivity(actUebersicht);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case (RequestLocationId):
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            SetUpMap();
                            StartLocationUpdates();
                        }
                    }
                    break;
            }

        }

        private void centerMap(Location loc)
        {
            Log.Debug("CenterMap", "CenterMap called...");
            if (mMap != null)
            {
                if (loc != null)
                {
                    CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                    builder.Target(new LatLng(loc.Latitude, loc.Longitude));
                    builder.Zoom(17);
                    CameraPosition camPos = builder.Build();

                    mMap.MoveCamera(CameraUpdateFactory.NewCameraPosition(camPos));

                    new Thread(() => { if (activeMonsters.Count == 0) for (int i = 0; i < 5; i++) generateMonster(); }).Start();
                }
            }
            else Log.Debug("CenterMap", "Map is null...");
        }

        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug("OnResume", "OnResume called...");
            if (mMap == null)
            {
                Log.Debug("OnResume", "Map is null, start SetUpMap...");
            }
            else
            {
                Log.Debug("OnResume", "Map not null...");
                if (apiClient == null)
                {
                    Log.Debug("OnResume", "Client is null, building now...");
                    apiClient = BuildApiClient();
                }
                else Log.Debug("OnResume", "ApiClient not null...");
                if (!apiClient.IsConnected)
                {
                    Log.Debug("OnResume", "Client is not connected, connecting now...");
                    apiClient.Connect();
                }
                else Log.Debug("OnResume", "ApiClient is connected...");
                if (!requestingLocationUpdates)
                {
                    Log.Debug("OnResume", "Begin location updates...");
                    StartLocationUpdates();
                }
                else Log.Debug("OnResume", "Allready requesting locatin updates...");
            }
        }

        private void SetUpMap()
        {
            if (mMap == null)
            { FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this); }
        }


        private void starteKampf(Monster monster)
        {
            Intent actKampf = new Intent(this, typeof(ActivityKampf));
            //Übergabe Spieler
            actKampf.PutExtra("spieler", JsonConvert.SerializeObject(spieler));
            //Übergabe Gegner
            actKampf.PutExtra("gegner", JsonConvert.SerializeObject(Monster.GetTestMonster()));
            StartActivity(actKampf);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            Log.Debug("OnMapReady", "Map returned, initializing...");
            mMap = googleMap;
            mMap.UiSettings.CompassEnabled = true;
            mMap.SetInfoWindowAdapter(this);
            mMap.SetOnInfoWindowClickListener(this);
            try
            {
                mMap.MyLocationEnabled = true;
                mMap.UiSettings.MyLocationButtonEnabled = true;
            }
            catch
            {
                Log.Debug("OnMapReady", "Permissions required, requestoing now...");
                RequestPermissions(PermissionsLocation, RequestLocationId);
            }
            if (apiClient == null)
            {
                Log.Debug("OnResume", "Client is null, building now...");
                apiClient = BuildApiClient();
            }
            else Log.Debug("OnResume", "ApiClient not null...");
            if (!apiClient.IsConnected)
            {
                Log.Debug("OnResume", "Client is not connected, connecting now...");
                apiClient.Connect();
            }
            else Log.Debug("OnResume", "ApiClient is connected...");
            if (!requestingLocationUpdates)
            {
                Log.Debug("OnResume", "Begin location updates...");
                StartLocationUpdates();
            }
            else Log.Debug("OnResume", "Allready requesting locatin updates...");
        }

        private List<Monsterart> EinlesenMonsterarten()
        {
            try
            {
                FileStream fs = new FileStream(Protokoll.GetPathArten(), FileMode.Open);
                XmlSerializer xml = new XmlSerializer(typeof(List<Monsterart>));
                List<Monsterart> lm = (List<Monsterart>)xml.Deserialize(fs);
                fs.Close();
                return lm;
            }catch(Exception e)
            {
                Log.Debug("ActMap Arten", e.StackTrace);
            }

            return null;
        }

        private List<Angriff> EinlesenAngriffe()
        {
            try
            {
                FileStream fs = new FileStream(Protokoll.GetPathAngriffe(), FileMode.Open);
                XmlSerializer xml = new XmlSerializer(typeof(List<Angriff>));
                List<Angriff> la = (List<Angriff>)xml.Deserialize(fs);
                fs.Close();
                return la;
            }catch(Exception e)
            {
                Log.Debug("ActMap", e.StackTrace);
            }
            return null;
        }
        private void generateMonster()
        {
            Monster m = GetRandomMonster();
            MarkerOptions opt = new MarkerOptions();
            //Marker Icon setzen
            //opt.Icon = GetMonsterIcon();
            opt.SetPosition(GetRandomLatLng());

            RunOnUiThread(() => { m.Marker = mMap.AddMarker(opt); });
            activeMonsters.Add(m);

        }
        public void OnConnected(Bundle connectionHint)
        {
            Log.Info("LocationClient", "Now connected to client");
            if (locRequest == null) CreateLocationRequest();
            StartLocationUpdates();
        }

        private Monster GetRandomMonster()
        {
            Random r = new Random();
            Monster m = new Monster();
            m.Art = monsterarten.ElementAt(r.Next(0, monsterarten.Count - 1));
            m.Nickname = m.Art.Name;
            m.Angriff = angriffe.ElementAt(r.Next(0, angriffe.Count - 1));
            m.Maxhp = m.Art.Maxhp;
            m.Lvl = 1;
            


            return m;
           // return Monster.GetTestMonster();
        }

        private LatLng GetRandomLatLng()
        {
            double lat;
            double lng;
            

            
            Location loc = LocationServices.FusedLocationApi.GetLastLocation(apiClient);
            Random r = new Random();
            
            int offsetLat = r.Next(10, 40);
            int offsetLng = r.Next(10, 40);
           
            r = new Random();
            if (r.Next(0, 10) > 5)
                lat = loc.Latitude + (offsetLat / 10000.0);
            else lat = loc.Latitude - (offsetLat / 10000.0);

            if (r.Next(0, 10) > 5) lng = loc.Longitude + (offsetLng / 10000.0);
            else lng = loc.Longitude - (offsetLng / 10000.0);

            return new LatLng(lat, lng);
        }

        private void CheckProximity(object loc)
        {
            Location temp = new Location(LocationManager.GpsProvider);
        
            foreach (Monster m in activeMonsters)
            {
                temp.Latitude = m.Marker.Position.Latitude;
                temp.Longitude = m.Marker.Position.Longitude;
                if (temp.DistanceTo((Location)loc) < 5)
                {
                    starteKampf(m);
                    break;
                }
            }
        }
        public void OnConnectionSuspended(int cause)
        {
            
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (requestingLocationUpdates)
            {
                requestingLocationUpdates = false;
                StopLocationUpdates();
            }
        }
        public void OnConnectionFailed(ConnectionResult result)
        {
            Log.Debug("LocationClient", "Connection failed, attempting to reach google play services");
            apiClient.Connect();
        }

        public void OnLocationChanged(Location location)
        {
            
            centerMap(location);
            Log.Debug("LocationClient", "Location updated, new location is: " + location.Latitude.ToString() + ", checking proximity");
            CheckProximity(location);

        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.MonsterInfo, null, false);
            ImageView bild = view.FindViewById<ImageView>(Resource.Id.imageViewMonsterInfoMap);
            TextView name = view.FindViewById<TextView>(Resource.Id.textViewNameMonsterInfo);
            TextView hp = view.FindViewById<TextView>(Resource.Id.textViewHpMonsterInfo);
            TextView atk = view.FindViewById<TextView>(Resource.Id.textViewAtkMonsterInfo);
            foreach (Monster m in activeMonsters)
            {
                if(m.Marker.Equals(marker))
                {
                    bild.SetImageResource(m.Art.Pic);
                    name.Text = m.Nickname;
                    hp.Text = m.Maxhp.ToString();
                    atk.Text = m.Angriff.Name;
                }
            }
            return view;
        }
        private bool IsGooglePlayServicesInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success) return true;

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error("ActivityMap", "Google Play Services Error: {0} - {1}", queryResult, errorString);
            }
            return false;

        }

        public void OnInfoWindowClick(Marker marker)
        {
            List<Monster> result = (from monster in activeMonsters where monster.Marker == marker select monster).ToList();
            starteKampf(result.FirstOrDefault());
        }
    }
}