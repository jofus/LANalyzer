using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LANalyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Get the IP address from the user input
            Console.WriteLine("Enter the IP address to ping:");
            string address = Console.ReadLine();

            // Create a Ping object
            Ping ping = new Ping();

            // Set name of the CSV file
            Console.WriteLine("Please enter name for CSV file");
            string CustomName = Console.ReadLine();
            

            // Create a CSV file to store the results
            string filename = (CustomName + ".csv");
            using (StreamWriter writer = new StreamWriter(filename))
            {
                // Write the CSV header
                writer.WriteLine("Timestamp,Status,Address,RoundtripTime,Drops");

                // UI feedback
                Console.WriteLine("Tests running");
                Console.WriteLine("Visual feedback - Dropped packets: ");

                // Ping the IP address indefinitely
                while (true)
                {
                    // Get the current date and time
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    try
                    {
                        // Send a ping request and get a reply
                        PingReply reply = ping.Send(address);

                        // Check the reply status
                        if (reply.Status == IPStatus.Success)
                        {
                            // Write the ping results to the CSV file
                            writer.WriteLine("{0},{1},{2},{3},0", timestamp, reply.Status, reply.Address, reply.RoundtripTime);
                        }
                        else
                        {
                            // Write the ping failure to the CSV file
                            writer.WriteLine("{0},{1},{2},{3},1", timestamp, reply.Status, "N/A", "N/A");
                            Console.Write(".");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Write the ping exception to the CSV file
                        writer.WriteLine("{0},{1},{2},{3},0", timestamp, "Exception", "N/A", ex.Message);
                    }

                    // Flush the writer to ensure the data is written to the file
                    writer.Flush();

                    // Wait for five seconds before the next ping
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }
    }
}
