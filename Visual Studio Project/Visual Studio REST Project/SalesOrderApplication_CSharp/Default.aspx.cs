using System;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;
using RestSharp;

namespace SalesOrderApplication_CSharp
{

    public partial class _Default : System.Web.UI.Page
    {
        //OAUTHTOKEN ESTABLISHED WHEN FORM IS LOADED
        private string token = "";

        //ARAS CREDENTIALS TO ESTABLISH TOKEN
        public class Credentials
        {
            public string grant_type { get; set; } = "password";
            public string client_id { get; set; } = "IOMApp";
            public string username { get; set; } = "admin";
            public string password { get; set; } = "607920b64fe136f9ab2389e371852af2";
            public string scope { get; set; } = "Innovator";
            public string database { get; set; } = "DevelopingSolutions12";
        }
                
        //RETURNED FROM GET TOKEN
        public class OAuthToken
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string token_type { get; set; }
        }

        //RETURNED FROM SALES ORDER GET
        public class Rootobject
        {
            public string odatacontext { get; set; }
            public Value[] value { get; set; }
        }

        public class Value
        {
            [JsonProperty("managed_by_id@aras.keyed_name")]
            public string managed_by_idaraskeyed_name { get; set; }
            public string multiple_shipment { get; set; }
            public string po_number { get; set; }
            public string shipping_method { get; set; }
            public string state { get; set; }
            public string so_number { get; set; }
            public string id { get; set; }
        }

        //USED TO PATCH(EDIT) A SALES ORDER
        public class PatchValue
        {
            public string po_number { get; set; }
            public string shipping_method { get; set; }
            public string multiple_shipment { get; set; }
        }

        //GET AUTHORIZATION TOKEN WHEN FORM IS LOADED
        protected void Page_Load(object sender, EventArgs e)
        {
            GetOAuthToken();
        }

        //GET AUTHORIZATION TOKEN FROM ARAS OAUTHSERVER
        private void GetOAuthToken()
        {
            //GET ARAS CREDENTIALS
            var cred = new Credentials();
            
            var client = new RestClient("http://localhost/Innovator12/oauthserver/connect/token");

            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
           
            request.AddParameter("client_id", cred.client_id, ParameterType.GetOrPost);
            request.AddParameter("grant_type", cred.grant_type, ParameterType.GetOrPost);
            request.AddParameter("username", cred.username, ParameterType.GetOrPost);
            request.AddParameter("password", cred.password, ParameterType.GetOrPost);
            request.AddParameter("scope", cred.scope, ParameterType.GetOrPost);
            request.AddParameter("database", cred.database, ParameterType.GetOrPost);
            
            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                StatusMessage.Text = response.ErrorMessage;
                return;
            }

            //Convert JSON content string into Object
            OAuthToken obj = JsonConvert.DeserializeObject<OAuthToken>(response.Content);
            token = obj.access_token;
        }
        
        //GET A SALES ORDER
        private void GetSalesOrder()
        {

            string url = "http://localhost/Innovator12/server/odata/Sales Order?$filter=so_number eq '" + TextBox1.Text + "'&$select=*";

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                StatusMessage.Text = response.ErrorMessage;
                return;
            }

            //Convert JSON response content into Object
            Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(response.Content);

            if (obj.value.Length < 1)
            {
                StatusMessage.Text = "No records found";
                return;
            }

            TextBox2.Text = obj.value[0].state;
            TextBox3.Text = obj.value[0].po_number;
            TextBox4.Text = obj.value[0].managed_by_idaraskeyed_name;
            DropDownList1.Text = obj.value[0].shipping_method;
            CheckBox1.Checked = (obj.value[0].multiple_shipment == "1") ? true : false;
            HiddenField1.Value = obj.value[0].id;

            //Call the Server Method to Calculate Cost
            //TextBox5.Text = ExecuteMethod(HiddenField1.Value, "Server-CalculateCost");

            StatusMessage.Text = "";

        }

        //EDIT A SALES ORDER
        private void EditSalesOrder()
        {
            string url = "http://localhost/Innovator12/server/odata/Sales Order('" + HiddenField1.Value + "')";

            PatchValue valdata2 = new PatchValue();
            valdata2.po_number = TextBox3.Text;
            valdata2.shipping_method = DropDownList1.Text;
            valdata2.multiple_shipment = (CheckBox1.Checked) ? "1" : "0";

            //Convert Updated Object into JSON data
            var postData = JsonConvert.SerializeObject(valdata2);
            
            var client = new RestClient(url);
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody(postData);
            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful) StatusMessage.Text = response.ErrorMessage;
            
        }

        //DELETE A SALES ORDER (USE DELETE RestRequest Method.DELETE)
        private void DeleteSalesOrder()
        {

        }

        //EXECUTE A SERVER METHOD ON CURRENT ITEM
        private string ExecuteMethod(string id, string methodName)
        {
            string url = "http://localhost/Innovator12/server/odata/Sales Order('" + HiddenField1.Value + "')/method." + methodName;
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                StatusMessage.Text = response.ErrorMessage;
                return "";
            }
            return response.Content.Trim('"');

        }

        protected void Button1_Click(object sender, EventArgs e)
        {            
           GetSalesOrder();            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            EditSalesOrder();            
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            DeleteSalesOrder();
        }

        

        
    }
}
