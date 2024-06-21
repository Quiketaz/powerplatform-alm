using System;
using Microsoft.Xrm.Sdk;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Module5CustomApiTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "AuthType=OAuth;Username=<username>;Password=<password>;Url=<url>;appid=<app id>;redirecturi=<redirect uri>";
            var client = new ServiceClient(connectionString);
            var request = new OrganizationRequest
            {
                RequestName = "pfedyn_GetSumofEstimatedRevenueforAccount"
            };
            request.Parameters.Add("AccountName", "Coho"); //test usign different account names
            var response = client.Execute(request);
            Console.WriteLine("The sum of the estimated revenue for the account Adventure is: " + response["SumOfEstimatedRevenue"]);

            Console.ReadKey(true);
        }
    }
}