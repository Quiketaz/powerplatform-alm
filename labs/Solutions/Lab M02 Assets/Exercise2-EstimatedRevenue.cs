using System;
using System.Runtime.Remoting.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Module2CustomApi
{
    public class EstimatedRevenuePlugin : IPlugin
    {
        public EstimatedRevenuePlugin() { }

        public void Execute(IServiceProvider serviceProvider)
        {
            var executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(executionContext.UserId);

            var accountName = (string)executionContext.InputParameters["AccountName"];

            var fetchXml = $@"<fetch aggregate='true'>
                                <entity name='opportunity'>
                                <attribute name='estimatedvalue_base' alias='EstimatedRevenue' aggregate='sum' />
                                <link-entity name='account' from='accountid' to='parentaccountid' link-type='inner' alias='ac'>
                                    <filter type='and'>
                                        <condition attribute='name' operator='like' value='%{accountName}%' />
                                    </filter>
                                </link-entity>
                                </entity>
                            </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            //create a money variale, set it to zero
            var sum = new Money(0);

            if (result.Entities.Count > 0)
            {
                if (((AliasedValue)result.Entities[0]["EstimatedRevenue"]).Value != null)
                {
                    sum = (Money)((AliasedValue)result.Entities[0]["EstimatedRevenue"]).Value;
                }
            }

            executionContext.OutputParameters["SumofEstimatedRevenue"] = (Decimal)sum.Value;
        }
    }
}