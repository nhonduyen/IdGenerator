// See https://aka.ms/new-console-template for more information
using Snowflake;

var machineId = 1L; // Ensure this is unique for each machine
var generator = new SnowflakeIdGenerator(machineId);

for (int i = 0; i < 10; i++)
{
    Console.WriteLine(generator.NextId());
}