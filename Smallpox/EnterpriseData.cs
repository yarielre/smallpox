using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Smallpox.Models;

namespace Smallpox
{
    [Activity(Label = "EnterpriseData")]
    public class EnterpriseData : Activity
    {
        HttpClient client;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            client = new HttpClient();

            SetContentView(Resource.Layout.enterprise);

            EditText enterpriseNameInput = FindViewById<EditText>(Resource.Id.txtEnterpriseNameField);
            EditText managerInput = FindViewById<EditText>(Resource.Id.txtManagerField);
            EditText addressInput = FindViewById<EditText>(Resource.Id.txtAddressField);
            EditText emailInput = FindViewById<EditText>(Resource.Id.txtEmailField);
            EditText phoneInput = FindViewById<EditText>(Resource.Id.txtPhoneField);
            Button saveButton = FindViewById<Button>(Resource.Id.btnSave);

            saveButton.Click += async (sender, e) =>
            {
                // Save data in API
                string url = "https://localhost:5001/weatherforecast"; //"https://localhost:5001/api/data/saveEnterpriseData"; // TODO
                Uri uri = new Uri(string.Format(url, string.Empty));

                string json = JsonConvert.SerializeObject(new EnterpriseModel() { 
                    EnterpriseName = enterpriseNameInput.Text,
                    Manager = managerInput.Text,
                    Address = addressInput.Text,
                    Email = emailInput.Text,
                    Phone = phoneInput.Text
                });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                //response = await client.PostAsync(uri, content);
                response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                }

                View view = (View)sender;
                Snackbar.Make(view, response.StatusCode.ToString(), Snackbar.LengthLong)
                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            };

        }
    }
}