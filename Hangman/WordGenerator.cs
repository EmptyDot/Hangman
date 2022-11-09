namespace Hangman;

public class WordGenerator
{
    private Random _rand = new();
    private readonly string _path;

    public WordGenerator(string path)
    {
        _path = path;
        try
        {
            File.ReadLines(path);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"File not found at path\n{path}.");
            throw;
        }
    }

    private int CountLines()
    {
        return File.ReadLines(_path).Count();
    }

    public string GetWord()
    {
        
        var file = File.ReadAllLines(_path);
        var word = file[_rand.Next(0, CountLines())];
        return word.Trim().ToUpper();
    }
}
