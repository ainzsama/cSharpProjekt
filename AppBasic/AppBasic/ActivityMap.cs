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

namespace AppBasic
{
    [Activity(Label = "ActivityMap")]
    public class ActivityMap : Activity, IOnMapReadyCallback, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener, Android.Gms.Maps.GoogleMap.IInfoWindowAdapter
    {
        private readonly string[] PermissionsLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        const int RequestLocationId = 0;


        private GoogleApiClient apiClient;
        private LocationRequest locRequest;

        private GoogleMap mMap;


        private Spieler spieler;
        private Button btnUebersicht;

        private bool _isGooglePlayServicesInstalled;

        private List<Monster> activeMonsters;

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

            _isGooglePlayServicesInstalled = IsGooglePlayServicesInstalled();

            if (_isGooglePlayServicesInstalled)
            {
                apiClient = new GoogleApiClient.Builder(this, this, this).AddApi(LocationServices.API).Build();
                locRequest = new LocationRequest();
                apiClient.Connect();
            }
            else
            {
                Log.Error("OnCreate", "Google Play Services nicht installiert");
                Toast.MakeText(this, "Google Play Services nicht installiert", ToastLength.Long).Show();

            }
            //RequestPermissions(PermissionsLocation, RequestLocationId);
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted && ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
            {
                Log.Debug("CheckPermissions", "Permissions ok");
                SetUpMap();
            }
            else
            {
                Log.Debug("CheckPermissions", "Permissions nicht ok");
                RequestPermissions(PermissionsLocation, RequestLocationId);
            }            
        }

        private void OnClickUebersicht(object sender, EventArgs e)
        {
            Intent actUebersicht = new Intent(this, typeof(ActivityUebersicht));

            //Übergabe Spieler
            actUebersicht.PutExtra("spieler", Intent.GetStringExtra("spieler"));
            actUebersicht.PutExtra("monsterarten", Intent.GetStringExtra("monsterarten"));
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
                            
                        }
                    }
                    break;
            }

        }

        private void centerMap(Location loc)
        {
            if (mMap != null)
            {
                if(loc != null)
                {
                    CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                    builder.Target(new LatLng(loc.Latitude, loc.Longitude));
                    builder.Zoom(17);
                    CameraPosition camPos = builder.Build();

                    mMap.MoveCamera(CameraUpdateFactory.NewCameraPosition(camPos));
                }
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug("OnResume", "OnResume called, connecting to client...");

            apiClient.Connect();
            if (apiClient.IsConnected)
            {
                Location location = LocationServices.FusedLocationApi.GetLastLocation(apiClient);
                Log.Debug("Location", "Location" + location.ToString());
                if (location != null)
                {
                    mMap.Clear();
                    centerMap(location);
                    for (int i = 0; i < 5; i++) generateMonster();
                    Log.Debug("LocationClient", "letzte position erhalten");
                }
                else Log.Debug("LocationClient", "Position nicht erhalten");
            }
            else Log.Debug("LocationClient", "warte auf client verbindung");

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
            mMap = googleMap;
            mMap.UiSettings.CompassEnabled = true;
            mMap.SetInfoWindowAdapter(this);
           
            try
            {
                mMap.MyLocationEnabled = true;
                mMap.UiSettings.MyLocationButtonEnabled = true;
                centerMap(LocationServices.FusedLocationApi.GetLastLocation(apiClient));

            }
            catch
            {
                RequestPermissions(PermissionsLocation, RequestLocationId);
            }
    }
        private void generateMonster()
        {
            Monster m = GetRandomMonster();
            MarkerOptions opt = new MarkerOptions();
            //Marker Icon setzen
            //opt.Icon = GetMonsterIcon();
            opt.SetPosition(GetRandomLatLng());

            m.Marker = mMap.AddMarker(opt);
            activeMonsters.Add(m);

        }
        public void OnConnected(Bundle connectionHint)
        {
            // This method is called when we connect to the LocationClient. We can start location updated directly form
            // here if desired, or we can do it in a lifecycle method, as shown above 

            // You must implement this to implement the IGooglePlayServicesClientConnectionCallbacks Interface
            Log.Info("LocationClient", "Now connected to client");
            Location loc = LocationServices.FusedLocationApi.GetLastLocation(apiClient);
           
            if (loc != null)centerMap(loc);
        }

        private Monster GetRandomMonster()
        {
            return Monster.GetTestMonster();
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
        public void OnConnectionSuspended(int cause)
        {
            
        }

        public void OnConnectionFailed(ConnectionResult result)
        {// This method is used to handle connection issues with the Google Play Services Client (LocationClient). 
         // You can check if the connection has a resolution (bundle.HasResolution) and attempt to resolve it

            // You must implement this to implement the IGooglePlayServicesClientOnConnectionFailedListener Interface
            Log.Info("LocationClient", "Connection failed, attempting to reach google play services"); throw new NotImplementedException();
        }

        public void OnLocationChanged(Location location)
        {
            // This method returns changes in the user's location if they've been requested

            // You must implement this to implement the Android.Gms.Locations.ILocationListener Interface

            centerMap(location);
            Log.Debug("LocationClient", "Location updated");

        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.MonsterInfo, null, false);
            
            foreach (Monster m in activeMonsters)
            {
                if(m.Marker.Equals(marker))
                {
                    //Infos einfügen
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
    }
}