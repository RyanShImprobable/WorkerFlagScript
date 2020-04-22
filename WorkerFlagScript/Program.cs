using System;
using Improbable.SpatialOS.Deployment.V1Alpha1;

namespace WorkerFlagScript
{
    class Program
    {
        private static readonly DeploymentServiceClient DeploymentServiceClient = DeploymentServiceClient.Create();
        static void Main(string[] args)
        {
            string customer_name = "Nostos";
            Boolean isWorkflagUsed = false;
            string[] projects = { "beta_sad_connecticut_304" };
            foreach (var projectName in projects)
            {
                Console.WriteLine("Hello World!");
                var listDeploymentsRequest = new ListDeploymentsRequest
                {
                    ProjectName = projectName,
                    View = ViewType.Full,
                };
                var deployments = DeploymentServiceClient.ListDeployments(listDeploymentsRequest);
                foreach (var deployment in deployments)
                {
                   
                    int count = deployment.WorkerFlags.Count;
                    if (count > 0)
                    {
                        isWorkflagUsed = true;
                        break;
                    }
                }
                deployments.
                if (isWorkflagUsed)
                {

                }
            }
            







        }
    }
}
