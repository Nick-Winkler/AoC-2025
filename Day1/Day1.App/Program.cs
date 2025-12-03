using Day1.App;

var fileInfo = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Input.txt"));
var cb = new CodeBreaker(fileInfo);
var password = cb.GetPasswordFromFile();

if (password == null)
{
    Console.WriteLine("Couldn't calculate password due to an error. See console for details.");
}
else
{
    Console.WriteLine($"Password: {password}");
}