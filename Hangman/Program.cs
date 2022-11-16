// By: Edvin Hermansson

// Using about 500 words from https://www.kaggle.com/datasets/rtatman/english-word-frequency
// Processed using pandas
// filter params: 5 <= word.Length <= 10, count > 10^8


namespace Hangman;

class Program
{
    public static void Main(string[] args)
    {
        var path = GetPath(args);
        if (path == "")
        {
            Console.WriteLine("Path not found");
            return;
        }
        
        var generator = new WordGenerator(path);
        
        bool keepPlaying;
        do
        {
            var word = generator.GetWord();
            var game = new Game(word);
            game.Play();
            keepPlaying = game.KeepPlaying();
        } while (keepPlaying);
    }

    

    private static string GetPath(string[] args)
    {
        const string fileName = "hangman_words.txt";
        string? path;
        // If any args
        if (args.Length >= 1)
        {
            // Take the first one, ignore any remaining args
            path = args[0];
        }
        else
        {
            // Check for file in cwd
            path = Path.Combine(Environment.CurrentDirectory, fileName);
            if (File.Exists(path)) return path;
            // Else prompt user
            Console.WriteLine($"Input the path to {fileName}");
            path = Console.ReadLine();
        }
        
        return File.Exists(path) ? path : "";
    }
}
