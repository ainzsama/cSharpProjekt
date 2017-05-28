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
    public class ActivityMap : Activity, IOnMapReadyCallback, GoogleApiClient.IConnectionCallbacks, 
        GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    {
        private readonly string[] PermissionsLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        const int RequestLocationId = 0;


        private GoogleApiClient apiClient;
        private LocationRequest locRequest;

        private GoogleMap mMap;
        private MarkerOptions spielerOptions;

        private Spieler spieler;
        private Button btnUebersicht;

        private bool _isGooglePlayServicesInstalled;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Map);


            btnUebersicht = FindViewById<Button>(Resource.Id.buttonUebersicht);
            btnUebersicht.Click += OnClickUebersicht;


            // Entgegennehmen Spieler
            spieler = JsonConvert.DeserializeObject<Spieler>(Intent.GetStringExtra("spieler"));

            spielerOptions = new MarkerOptions();
            spielerOptions.SetTitle("Spieler");
            SetUpMap();

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

            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch(requestCode)
            {
                case (RequestLocationId):
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            mMap.MyLocationEnabled = true;
                            mMap.AddMarker(new MarkerOptions().SetPosition(new LatLng(mMap.MyLocation.Latitude, mMap.MyLocation.Longitude)));
                        }
                    }
                    break;
            }
            
        }
        private bool IsGooglePlayServicesInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success) return true;

            if(GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error("ActivityMap", "Google Play Services Error: {0} - {1}", queryResult, errorString);
            }
            return false;
     
        }


        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug("OnResume", "OnResume called, connecting to client...");

            apiClient.Connect();
            if (apiClient.IsConnected)
            {
                Location location = LocationServices.FusedLocationApi.GetLastLocation(apiClient);
                if (location != null)
                {
                    mMap.AddMarker(new MarkerOptions().SetPosition(new LatLng(location.Latitude, location.Longitude)));
                    btnUebersicht.Text = location.Latitude.ToString();
                    Log.Debug("LocationClient", "letzte position erhalten");
                }
            }
            else Log.Info("LocationClient", "warte auf client verbindung");

        }
        private void OnClickUebersicht(object sender, EventArgs e)
        {
            Intent actUebersicht = new Intent(this, typeof(ActivityUebersicht));

            //Übergabe Spieler
            actUebersicht.PutExtra("spieler", JsonConvert.SerializeObject(spieler));
            StartActivity(actUebersicht);
        }

        private void SetUpMap()
        {
            if (mMap == null)
            { FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this); }
        }


        private void starteKampf()
        {
            Intent actKampf = new Intent(this, typeof(ActivityKampf));
            //Übergabe Spieler
            actKampf.PutExtra("spieler", JsonConvert.SerializeObject(spieler));
            //Übergabe Gegner
            actKampf.PutExtra("gegner", JsonConvert.SerializeObject(Monster.getTestMonster()));
            StartActivity(actKampf);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
           try
            {
                mMap.MyLocationEnabled = true;
                mMap.AddMarker(new MarkerOptions().SetPosition(new LatLng(mMap.MyLocation.Latitude, mMap.MyLocation.Longitude)));
            }

            catch {
                 RequestPermissions(PermissionsLocation, RequestLocationId);

            }

        }

        public void OnConnected(Bundle connectionHint)
        {
            // This method is called when we connect to the LocationClient. We can start location updated directly form
            // here if desired, or we can do it in a lifecycle method, as shown above 

            // You must implement this to implement the IGooglePlayServicesClientConnectionCallbacks Interface
            Log.Info("LocationClient", "Now connected to client");
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
            Log.Debug("LocationClient", "Location updated");
        }
    }
}