using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master_process
{
    public class MasterClass
    {
        private List<string> words;

        public MasterClass()
        {
            words = new List<string>();
        }

        public async Task ReadFromAgent(string pipe_name)
        {
            var server = new NamedPipeServerStream(pipe_name, PipeDirection.In);
            Console.WriteLine($"Connecting to agent {pipe_name}");
            await server.WaitForConnectionAsync();
            Console.WriteLine($"Agent {pipe_name} connection successful");

            var reader = new StreamReader(server);

            string? line;
            while (true)
            {
                line = await reader.ReadLineAsync();
                if (line == null) break;
                words.Add(line);
            }
        }

        public void PrintResults()
        {
            foreach (string word in words) Console.WriteLine(word);
        }
    }
}
