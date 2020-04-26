using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.SpatialOS.Deployment.V1Alpha1;

namespace WorkerFlagScript
{

    
    class Program
    {
        private static readonly DeploymentServiceClient DeploymentServiceClient = DeploymentServiceClient.Create();
        static void Main(string[] args)
        {

            //string customer_name = "Nostos";         
            //string[] projects = { "beta_sad_connecticut_304" };

            Dictionary<string, List<string>> customer_projects = new Dictionary<string, List<string>>()
            {
                { "Ryan", new List<string> { "beta_sad_connecticut_304" } },
                { "NetEase", new List<string> {"nostos_sa_testing"} },

            };
            Console.WriteLine("GO and check!");
            foreach (KeyValuePair<string, List<string>> entry in customer_projects)
            {
                
                Boolean isWorkflagUsed = false;
                var customer_name = entry.Key;
                Console.WriteLine(customer_name);
                foreach (var projectName in entry.Value)
                {
                    Console.WriteLine(projectName);
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

                    if (isWorkflagUsed)
                    {
                        Console.WriteLine("Customer: {0} is using workflag.", projectName);
                    }
                }
            }
            
            







        }
    }
}
