using Master_process;
using System.Diagnostics;

string base_directory = AppDomain.CurrentDomain.BaseDirectory;
base_directory = Path.Combine(base_directory, "..", "..", "..", "..");
string agent_base_directory = Path.Combine(base_directory, "Agent", "bin", "Debug", "net9.0");
string agent_exe_path = Path.Combine(agent_base_directory, "Agent.exe");

string[] agent_file_directories = { Path.Combine(base_directory, "agent1data"), Path.Combine(base_directory, "agent2data") };
string[] pipe_names = { "agent1", "agent2" };




Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)0x1;

MasterClass master = new MasterClass();

Task task1 = master.ReadFromAgent(pipe_names[0]);
Task task2 = master.ReadFromAgent(pipe_names[1]);

await Task.Delay(500);

for (int i = 0; i < 2; i++)
{
    Process.Start(agent_exe_path, $"\"{agent_file_directories[i]}\" {pipe_names[i]}");
}

await Task.WhenAll(task1, task2);

master.PrintResults();

Console.ReadKey();