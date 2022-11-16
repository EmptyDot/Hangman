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
            // Take the first one
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

internal class Game
{
    private int AttemptsRemaining { get; set; }
    private string Word { get; }

    // does not allow duplicate values
    private readonly HashSet<char> _usedLetters = new ();


    public Game(string word)
    {
        AttemptsRemaining = 6;
        Word = word;
    }
    
    /// <summary>
    /// Updates the console with the game state each turn
    /// </summary>
    private void Update()
    {
        Console.Clear();
        Picture.Draw(AttemptsRemaining);
        Console.WriteLine();
        ShowWord();
        ShowUsedLetters();
        Console.WriteLine($"Attempts left: {AttemptsRemaining}");
    }

    /// <summary>
    /// Main gameplay loop handler. Will be called from Main()
    /// </summary>

    public void Play()
    {
        
        while (AttemptsRemaining > 0)
        {
            // Get guess
            var guess = GetGuess();
            // Add to used letters
            _usedLetters.Add(guess);
            
            // if guess not in word, decrement attempts remaining
            if (Word.All(letter => guess != letter)) AttemptsRemaining -= 1;
            
            // if all letters in word has been guessed, return
            if (Word.All(letter => _usedLetters.Contains(letter))) return;
        }
    }
    
    private char GetGuess()
    {
        while (true)
        {
            // Display current game state
            Update();
            
            // Prompt user
            Console.Write("Guess a letter: ");
            // Get input
            var guessChar = GetLetter();
            Console.Write(guessChar);
            if (_usedLetters.Contains(guessChar) || !Confirm()) continue;
            
            return guessChar;
        } 
    }

    private static bool Confirm()
    {
        while (true)
        {
            var confirmKey = Console.ReadKey(true);
            switch (confirmKey.Key)
            {
                case (ConsoleKey.Enter):
                    return true;
                case (ConsoleKey.Backspace):
                    return false;
                default:
                    continue;
            }
        }
    }

    private static char GetLetter()
    {
        ConsoleKeyInfo guessedKey;
        do
        {
            guessedKey = Console.ReadKey(true);
        } while (!char.IsLetter(guessedKey.KeyChar));
        
        return char.ToUpper(guessedKey.KeyChar);
    }

    /// <summary>
    /// Used in Update(), shows the letters that the player guessed correctly and their position in the word
    /// </summary>
    private void ShowWord()
    {
        foreach (var letter in Word) 
            Console.Write(_usedLetters.Contains(letter) ? $"{letter} " : "_ "); 
        Console.WriteLine();
    }
    
    /// <summary>
    /// Used in Update(), shows the letters that the player has guessed so far
    /// </summary>
    private void ShowUsedLetters()
    {
        Console.Write("Used letters: ");
        foreach (var usedLetter in _usedLetters)
            Console.Write($"{usedLetter} ");
        Console.WriteLine();
    }

    public bool KeepPlaying()
    {
        while (true)
        {
            Update();
            if (AttemptsRemaining == 0)
            {
                Console.WriteLine("You lose!");
                Console.WriteLine($"The word was: {Word}");
            }
            else
            {
                Console.WriteLine("You win!"); 
            }
            Console.WriteLine("Play again? [y]es / [n]o");
            var answer = Console.ReadLine() ?? "";
            switch (answer.ToLower())
            {
                case "y" or "yes":
                    return true;
                case "n" or "no":
                    return false;
                default:
                    continue;
            }
        }
    }
}
