using System;
using System.Linq;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace Module5DataverseClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "AuthType=OAuth;Username=<username>;Password=<password>;Url=<url>;appid=<app id>;redirecturi=<redirect uri>";
            var client = new ServiceClient(connectionString);

            if (!client.IsReady)
            {
                throw new Exception("Cannot connect to Dynamics 365 Online");
            }

            // Create an account
            var account = new Entity("account");
            account["name"] = "Module 5 Account";

            var accountGuid = client.Create(account);
            Console.WriteLine($"Account created with Id = {accountGuid}");

            // Retrieve the account
            var retrievedAccount = client.Retrieve("account", accountGuid, new ColumnSet("name", "createdon"));
            Console.WriteLine($"Name = {(string)retrievedAccount["name"]}, Created On = {(DateTime)retrievedAccount["createdon"]}");

            // Create first contact
            var contact1 = new Entity("contact");
            contact1["firstname"] = "Contact1";
            contact1["lastname"] = "Module5";
            var contact1Guid = client.Create(contact1);
            Console.WriteLine($"Contact 1 created with Id = {contact1Guid}");

            // Create second contact
            var contact2 = new Entity("contact");
            contact2["firstname"] = "Contact2";
            contact2["lastname"] = "Module5";
            var contact2Guid = client.Create(contact2);
            Console.WriteLine($"Contact 2 created with Id = {contact2Guid}");

            // Retrieve the contacts using FetchXml
            Console.WriteLine($"Retrieving records using FetchXml");

            var fetchXml = $@"<fetch>
                    <entity name='contact'>
                    <attribute name='contactid' />
                    <attribute name='lastname' />
                    <attribute name='firstname' />
                    <filter type='and'>
                        <condition attribute='lastname' operator='eq' value='Module5' />
                    </filter>
                    </entity>
                </fetch>";
            var fetchExpression = new FetchExpression(fetchXml);

            var fetchExpressionResult = client.RetrieveMultiple(fetchExpression);
            foreach (var entity in fetchExpressionResult.Entities)
            {
                Console.WriteLine($"First name = {entity["firstname"]}, Last name = {entity["lastname"]}");
            }

            // Retrieve the contacts using QueryExpression
            Console.WriteLine($"Retrieving records using QueryExpression");

            var queryExpression = new QueryExpression
            {
                EntityName = "contact",
                ColumnSet = new ColumnSet("firstname", "lastname"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "lastname",
                            Operator = ConditionOperator.Equal,
                            Values = { "Module5" }
                        }
                    }
                }
            };

            var queryExpressionResult = client.RetrieveMultiple(queryExpression);
            foreach (var entity in queryExpressionResult.Entities)
            {
                Console.WriteLine($"First name = {entity["firstname"]}, Last name = {entity["lastname"]}");
            }

            // Retrieve the contacts using LINQ
            Console.WriteLine($"Retrieving records using LINQ");

            // var organizationService = (IOrganizationService)client.OrganizationWebProxyClient;
            var orgContext = new OrganizationServiceContext(client);

            var contactsByLastName = from c in orgContext.CreateQuery("contact")
                                     where (string)c["lastname"] == "Module5"
                                     select new { FirstName = c["firstname"], LastName = c["lastname"] };

            foreach (var entity in contactsByLastName)
            {
                Console.WriteLine($"First name = {entity.FirstName}, Last name = {entity.LastName}");
            }

            // Update the contacts to link them with the account
            contact1.Id = contact1Guid;
            contact1["parentcustomerid"] = new EntityReference("account", accountGuid);
            client.Update(contact1);

            contact2.Id = contact2Guid;
            contact2["parentcustomerid"] = new EntityReference("account", accountGuid);
            client.Update(contact2);

            Console.WriteLine("Contacts associated with account");

            // Retrieve the contacts associated with the account
            Console.WriteLine($"Retrieving contacts associated with Module 5 Account");

            var linkedEntityFetchXml = $@"<fetch>
                        <entity name='contact'>
                            <attribute name='contactid' />
	                        <attribute name='fullname' />
                            <link-entity name='account' from='accountid' to='parentcustomerid' link-type='inner' alias='ab'>
                                <attribute name='name' />
                                <filter type='and'>
                                    <condition attribute='name' operator='eq' value='Module 5 Account' />
                                </filter>
                            </link-entity>
                        </entity>
                    </fetch>";
            var linkedEntityFetchExpression = new FetchExpression(linkedEntityFetchXml);

            var linkedEntityFetchExpressionResult = client.RetrieveMultiple(linkedEntityFetchExpression);
            foreach (var entity in linkedEntityFetchExpressionResult.Entities)
            {
                var fullname = (string)entity["fullname"];
                var accountName = (string)((AliasedValue)entity["ab.name"]).Value;
                Console.WriteLine($"Full name = {fullname}, Account name = {accountName}");
            }

            Console.ReadKey(true);
        }
    }
}