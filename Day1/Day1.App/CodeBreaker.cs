namespace Day1.App;

public class CodeBreaker(FileInfo input)
{
    public int? GetPasswordFromFile()
    {
        int? password = 0;
        try
        {
            var currentPosition = 50;
            var dialModulo = 100;

            using var reader = input.OpenText();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var prefix = line[0];
                if (prefix != 'R' && prefix != 'L')
                {
                    Console.WriteLine($"Missing L/R prefix for line with text: {line}");
                    return null;
                }
                if (line.Length < 2)
                {
                    Console.WriteLine($"Missing number for line with text: {line}");
                    return null;
                }
                if (!int.TryParse(line[1..], out int num))
                {
                    Console.WriteLine($"Unable to parse an integer from line with text: {line}");
                    return null;
                }

                var change = prefix == 'R' ? num : -num;
                // The [(+ dialModulo) % dialModulo] block solved my issue with negatives, getting them back in line with the positives
                currentPosition = ((currentPosition + change) % dialModulo + dialModulo) % dialModulo;
                if (currentPosition == 0) password++;
            }
        }
        catch (Exception ex) when (
            ex is DirectoryNotFoundException
            || ex is FileNotFoundException
            || ex is System.Security.SecurityException
            || ex is UnauthorizedAccessException
            || ex is IOException
        )
        {
            Console.WriteLine($"Please verify that the file is accessible and exists at: {input.FullName}");
            return null;
        }
        catch (OutOfMemoryException)
        {
            Console.WriteLine("The file is too big to process for your machine");
            return null;
        }

        return password;
    }
}
