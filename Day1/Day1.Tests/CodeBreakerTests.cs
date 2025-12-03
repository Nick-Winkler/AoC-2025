using Day1.App;

namespace Day1.Tests;

public class CodeBreakerTests
{
    [Fact]
    public void GetPasswordFromFile_GivenSampleData_ReturnsCorrectPassword()
    {
        var exampleFileInfo = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "TestInput.txt"));
        var cb = new CodeBreaker(exampleFileInfo);

        var password = cb.GetPasswordFromFile();

        Assert.Equal(3, password);
    }

    [Fact]
    public void GetPasswordFromFile_GivenLargeDataset_PerformsWell()
    {
        var exampleFileInfo = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "LargeTestInput.txt"));
        var cb = new CodeBreaker(exampleFileInfo);

        var sw = System.Diagnostics.Stopwatch.StartNew();
        var password = cb.GetPasswordFromFile();
        sw.Stop();

        Assert.Equal(103, password);
        Assert.True(sw.ElapsedMilliseconds < 10); // May need to be adjusted for slower machines, but this is just a simple curiosity check anyway
    }
}