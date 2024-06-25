using Sonyflake;

var machineId = 1L; // Ensure this is unique for each machine
var generator = new SonyflakeGenerator(machineId);

Console.WriteLine($"Machine id: {machineId}");
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(generator.NextId());
}