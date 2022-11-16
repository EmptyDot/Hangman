namespace Hangman;

public class WordGenerator
{
    private Random _rand = new();
    private readonly string _path;
    
    public WordGenerator(string path)
    {
        _path = path;
    }

    /// <summary>
    /// Gets a random line from text file
    /// </summary>
    public string GetWord()
    {
        var file = File.ReadAllLines(_path);
        var word = file[_rand.Next(0, File.ReadLines(_path).Count())];
        return word.Trim().ToUpper();
    }
}
