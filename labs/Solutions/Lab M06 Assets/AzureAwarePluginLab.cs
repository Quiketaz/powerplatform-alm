using System;
using Microsoft.Xrm.Sdk;

namespace AzureAwarePlugin
{
    public class AzureAwarePluginLab : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Retrieve the execution context.
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Extract the tracing service.
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (tracingService == null)
                throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

            var cloudService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));

            if (cloudService == null)
                throw new InvalidPluginExecutionException("Failed to retrieve the service bus service.");

            try
            {
                tracingService.Trace("Posting the execution context.");

                var response = cloudService.Execute(new EntityReference("serviceendpoint", new Guid("36cbc702-58b8-ed11-83fe-000d3a32feea")), context);

                tracingService.Trace($"Reponse: {response}");

                if (!String.IsNullOrEmpty(response))
                {
                    tracingService.Trace("Response = {0} for ServiceBusId: {1}", response, "36cbc702-58b8-ed11-83fe-000d3a32feea");
                }

                //do other things...

                tracingService.Trace("Done.");
            }
            catch (Exception e)
            {
                //If the ASB is down or not accessible, the cloudService.Execute will throw an exception.
                tracingService.Trace("Exception: {0}", e.ToString());
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}