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
               // { "Ryan", new List<string> { "beta_sad_connecticut_304" } },
                { "NetEase", new List<string> {"nostos_sa_testing", "nostos_testing", "nostos_kol", } },

            };
            Console.WriteLine("GO and check!");
            Dictionary<string, string[]> resultMap = readCustomerProjectInfo();
            
            foreach (KeyValuePair<string, string[]> entry in resultMap)
            {
                
                Boolean isWorkflagUsed = false;
                var customer_name = entry.Key;
                string[] projectNames = entry.Value;
                Console.WriteLine(customer_name);
                for (int i = 1; i < projectNames.Length; i++)
                {
                    string projectName = projectNames[i];
                    Console.WriteLine(projectName);
                    var template = $"auth_client account elevate liangren@improbable.io {projectName} --message=\"ryan support\"";
                    Console.WriteLine(template);
                    // elevate permission
                    ExecuteCommandSync(template);
                    var listDeploymentsRequest = new ListDeploymentsRequest
                    {
                        ProjectName = projectName,
                        View = ViewType.Full,
                    };
                    try
                    {
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
                            Console.WriteLine("Customer:{0}, Project:{1} is using workflag.", customer_name, projectName);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message); 
                    }
                    
                }
            }

        }

        static Dictionary<string, string[]> readCustomerProjectInfo()
        {
            int counter = 0;
            string line;
            Dictionary<string,string[]> customer_projects = new Dictionary<string,string[]>();

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"C:\Ryan\Projects\WorkerFlag\WorkerFlagScript\WorkerFlagScript\projectlist.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] customerResult = line.Split(',');
                if (customerResult.Length >= 2)
                {
                    customer_projects.Add(customerResult[0], customerResult);
                }
                else
                {
                    System.Console.WriteLine("Incorrect Format for line: {0}", line);
                }
               
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            return customer_projects;
        }

        static void ExecuteCommandSync(string command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                // Log the exception
            }
        }
    }
}
