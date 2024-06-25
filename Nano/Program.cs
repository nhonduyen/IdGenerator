// Generate a NanoID with default settings
using Nano;

string nanoId = NanoId.Generate();

// Output the generated NanoID
Console.WriteLine($"Generated NanoID: {nanoId}");

// Generate a custom NanoID
string customNanoId = NanoId.Generate(10, "0123456789ABCDEF");
Console.WriteLine($"Generated custom NanoID: {customNanoId}");

for (int i = 0; i < 10; i++)
{
    var id = NanoId.Generate();
    Console.WriteLine($"Generated NanoID {i}: {id}");
}