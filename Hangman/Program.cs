// By: Edvin Hermansson

// Using about 500 words from https://www.kaggle.com/datasets/rtatman/english-word-frequency
// Processed using pandas
// filter params: 5 <= word.Length <= 10, count > 10^8



namespace Hangman;

class Program
{
    public static void Main(string[] args)
    {
        // Check for words in cwd
        var path = ParseArgs(args);
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

    

    private static string ParseArgs(string[] args)
    {
        string? path;
        if (args.Length == 0)
        {
            Console.WriteLine("Input the path to the hangman_words.txt");
            path = Console.ReadLine();
        }
        else
        {
            path = args[0];
        }

        return File.Exists(path) ? path : "";
    }
    
}

internal class Game
{
    private const int TotalAttempts = 6;
    // Keeping a class variable because several methods need access to this 
    private int _attemptsRemaining = TotalAttempts;
    
    // does not allow duplicate values
    private readonly HashSet<char> _usedLetters = new ();
    
    private readonly string _word;

    
    public Game(string word)
    {
        _word = word;
    }
    
    /// <summary>
    /// Updates the console with the game state each turn
    /// </summary>
    private void Update()
    {
        Console.Clear();
        Picture.Draw(_attemptsRemaining);
        Console.WriteLine();
        ShowWord();
        ShowUsedLetters();
        Console.WriteLine($"Attempts left: {_attemptsRemaining}");
    }

    /// <summary>
    /// Main gameplay loop handler. Will be called from Main()
    /// </summary>

    public void Play()
    {
        
        while (_attemptsRemaining > 0)
        {
            // Get guess
            var guess = GetGuess();
            // Add to used letters
            _usedLetters.Add(guess);
            
            // if guess not in word, decrement attempts remaining
            if (_word.All(letter => guess != letter)) _attemptsRemaining -= 1;
            
            // if all letters in word has been guessed, return
            if (_word.All(letter => _usedLetters.Contains(letter))) return;
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
        foreach (var letter in _word) 
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
            if (_attemptsRemaining == 0)
            {
                Console.WriteLine("You lose!");
                Console.WriteLine($"The word was: {_word}");
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
