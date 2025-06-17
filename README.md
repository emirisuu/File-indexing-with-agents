# File-indexing-with-agents
File content indexing using agents and a master process in C#

## version 0.1
Set the base file architecture with one Master class and master file for using the class and one Agent class and agent file for using it.

## version 0.2
AgentClass which holds pipe_name, txt_file_paths array and file_data as a string list. They are instance variables.
Initialiaziation function initializes these variables.
ReadFiles function goes through all the text files in the given directory, initializes a Dictionary <string, int> for each file.
Reads the files cleaning up the unnecessary symbols and accordingly puts the word as the Dictionary string and increases its counter
After the file is processed the stats are formatted in a string and stored in file_data List.
SendProcessedData function connects via named pipe to the main process.
StreamWriter with AutoFlush is initialized
function goes through the file_data List and writes each line to the master pipe with async

Agent.cs file gets the directory and pipe_name arguments for the AgentClass via command line arguments automatically. 
Based on the agents name the agent gets either core 2 from cpu or core 3.
Error handling if directory doesn't exist. 
Runs ReadFiles and SendProcessedData functions from class file.

## version 0.3

MasterClass has one instance variable words which is List <string> and is initialized with class instance creation
ReadFromAgent function takes a pipe_name for the agent and starts a NamedPipeServerStream 
waits for the connection to happen with async
starts a StreamReader with the created NamedPipeServerStream
initializes a string variable line which can be null
runs a while loop which waits for the Reader data with async, adds it to the words List and breaks the loop when its null.
PrintResults function writes all the processed data from both agents.

Master.cs file gets the current base directory and sets it correctly for agent directories and file paths to work
sets the agent file directories and pipe names
Sets the master process to use core 1 of cpu
Initializes the MasterClass
Starts listening to both agents via tasks
Adds a delay of 500 so the agent files don't start too early
starts both agents via Process.Start with according command line arguments
waits for both task to complete
prints results and waits for a key to be pressed before finish the process

Refactored AgentClass
Started correctly reading the formed dictionary with KeyValuePair.
changed the formatted string that is being put into the List
changed the SendProcessedData function from async void to async Task so that it can wait for the agent to finishi

Refactored Agent.cs
added await keyword before SendProcessData function since it was changed from async void to async Task