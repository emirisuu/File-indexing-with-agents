using Agent;
using System.Diagnostics;

if (args.Length < 2)
{
    Console.WriteLine("Incorrect number of arguments. Correct format is: directory, pipename");
    return;
}

string directory = args[0];
string pipe_name = args[1];

Process.GetCurrentProcess().ProcessorAffinity = pipe_name == "agent1" ? (IntPtr)0x2 : (IntPtr)0x4;

if (!Directory.Exists(directory))
{
    Console.WriteLine("Directory does not exist");
    return;
}

AgentClass agent = new AgentClass(directory, pipe_name);
agent.ReadFiles();
agent.SendProcessedData();
