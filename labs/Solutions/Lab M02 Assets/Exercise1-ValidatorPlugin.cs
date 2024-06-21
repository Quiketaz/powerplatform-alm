using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Module2Plugins
{
    public class ValidatorPlugin : IPlugin
    {
        public ValidatorPlugin()
        {
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(executionContext.UserId);

            if (executionContext.InputParameters.Contains("Target") && executionContext.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)executionContext.InputParameters["Target"];
                
                if (entity.Contains("birthdate") && entity["birthdate"] is DateTime)
                {
                    DateTime birthDate = (DateTime)entity["birthdate"];

                    if (birthDate > DateTime.Now)
                    {
                        throw new InvalidPluginExecutionException("Birthdate must be in the past");
                    }
                }
            }
        }
    }
}