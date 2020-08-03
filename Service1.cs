using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Device.Location;
using System.Runtime.InteropServices;
using System.Diagnostics;
using DemoService.Model;
using DemoService.Helper;
using System.Web;

namespace DemoService
{
    public partial class Service1 : ServiceBase
    {
        #region Declare
        static HttpClient client = new HttpClient();
        #endregion

        #region Default Methods
        public Service1()
        {
            InitializeComponent();
            RunAsync().GetAwaiter().GetResult();
        }

        protected override void OnStart(string[] args)
        {

        }
        
        protected override void OnStop()
        {

        }
        #endregion

        #region API Methods
        static async Task<ResponseObject> GetResponseObjectAsync(string path)
        {
            ResponseObject responseObj = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                responseObj = await response.Content.ReadAsAsync<ResponseObject>();
            }
            return responseObj;
        }
        static async Task<Uri> CreateResponseObjectAsync(DataObject product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync( Common.GetKeyUpdateURL(), product);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
        #endregion

        #region General Methods
        static async Task RunAsync()
        {
            InitAPI();
            string kcURL = Common.GetKeyCheckURL();
            kcURL = Common.AddParameter(kcURL, "k", "e35cf7b66449df565f93c607d5a81d09");
            kcURL = Common.AddParameter(kcURL, "typ", "New");
            
            //// Update port # in the following line.
            //client.BaseAddress = new Uri(_URL);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(  new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("Apiauthorization", "c20f87f288522496fbf39868b12cda07");
            try
            {

                // Create a new product
                DataObject requestSet = new DataObject
                {
                    key = "e35cf7b66449df565f93c607d5a81d00",
                    macadress = Common.getMacAddress(),
                    additionaldetail = "USER_NAME"
                };

                var url = await CreateResponseObjectAsync(requestSet);
                Console.WriteLine($"Created at {url}");

                // Get the product
                ResponseObject dObj = new ResponseObject();
                dObj = await GetResponseObjectAsync(kcURL);
                if (dObj.error.ToLower() == "false")
                {
                    string mac = Common.getMacAddress();
                }                
            }
            catch (Exception e)
            {
                
            }
        }
        static public void InitAPI()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri(Common._BaseURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Apiauthorization", "c20f87f288522496fbf39868b12cda07");
        }
        #endregion
    }
}
