using System.IO.Pipes;

namespace Agent
{
    public class AgentClass
    {
        private string pipe_name;
        private string[] txt_file_paths;
        private List<string> file_data;

        public AgentClass(string file_directory, string pipe_name)
        {
            this.pipe_name = pipe_name;
            txt_file_paths = Directory.GetFiles(file_directory, "*.txt");
            file_data = new List<string>();
        }

        public void ReadFiles()
        {
            foreach (string file_path in txt_file_paths)
            {
                Console.WriteLine($"Reading file {Path.GetFileName(file_path)}");
                Dictionary<string, int> word_count = new Dictionary<string, int>();
                string[] lines = File.ReadAllLines(file_path);
                foreach (string line in lines)
                {
                    string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string word in words)
                    {
                        string clean_word = word.ToLower().Trim('.', ',', ';');
                        if(word_count.ContainsKey(clean_word))
                            word_count[clean_word]++;
                        else word_count[clean_word] = 1;
                    }
                }
                foreach (var word in word_count)
                {
                    file_data.Add($"{file_path}:{word.Key}:{word.Value}");
                }
                Console.WriteLine("Done.");
            }
        }

        public async void SendProcessedData()
        {
            Console.WriteLine("Sending data to Master process");
            using var pipe = new NamedPipeClientStream(".", this.pipe_name, PipeDirection.Out);
            await pipe.ConnectAsync();

            using var writer = new StreamWriter(pipe) { AutoFlush = true };
            foreach(string data in file_data)
            {
                await writer.WriteLineAsync(data);
            }
            Console.WriteLine("Done.");
        }
    }
}
