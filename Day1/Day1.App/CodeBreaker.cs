namespace Day1.App;

public class CodeBreaker(FileInfo input)
{
    public enum PasswordMethod
    {
        CountLandingOnZero,
        CountPassingOrLandingOnZero,
    }

    public int? GetPasswordFromFile(PasswordMethod pwMethod)
    {
        int? password = 0;

        try
        {
            var currentPosition = 50;
            var dialModulo = 100;

            Action<int> processDialChange = pwMethod switch
            {
                PasswordMethod.CountLandingOnZero => CountLandOnZero,
                PasswordMethod.CountPassingOrLandingOnZero => CountPassOrLandOnZero,
                _ => throw new NotImplementedException($"{nameof(PasswordMethod)} {pwMethod} hasn't been implemented yet.")
            };

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
                processDialChange(change);
            }

            void CountLandOnZero(int change)
            {
                // The [(+ dialModulo) % dialModulo] block used by these functions solved my issue with negatives, getting them back in line with the positives
                currentPosition = ((currentPosition + change) % dialModulo + dialModulo) % dialModulo;
                if (currentPosition == 0)
                {
                    password++;
                }
            }
            void CountPassOrLandOnZero(int change)
            {
                // Reduce any large changes down to less than a single dialModulo and just add the full rotations to the password
                var fullRotations = Math.Abs(change) / dialModulo;
                password += fullRotations;
                change -= fullRotations * dialModulo;

                var previousPosition = currentPosition;
                currentPosition = ((currentPosition + change) % dialModulo + dialModulo) % dialModulo;

                // I feel like there's a way to "math out" these if blocks to be even more elegant, but I lack the ability to see it right now
                if (currentPosition == 0)
                {
                    password++;
                }
                else if (previousPosition != 0
                    && ((change > 0 && currentPosition < previousPosition)
                        || (change < 0 && currentPosition > previousPosition)))
                {
                    password++;
                }
            }
        }
        catch (NotImplementedException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
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
            Console.WriteLine("You must have a really long line in your file. I won't print that line of text here since it'd probably muck up the terminal.");
            return null;
        }

        return password;
    }
}
