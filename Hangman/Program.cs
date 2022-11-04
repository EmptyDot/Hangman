// By: Edvin Hermansson

// Using about 500 words from https://www.kaggle.com/datasets/rtatman/english-word-frequency
// Processed using pandas
// filter params: 5 <= word.Length <= 10 and count > 10^8


namespace Hangman;

class Program
{
    
    public static void Main(string[] args)
    {
        string fileName = "hangman_words.txt";
        string wordsPath = Path.Combine(Environment.CurrentDirectory, fileName);
        WordGenerator generator = new WordGenerator(wordsPath);
        
        while (true)
        {
            Game game = new Game();
            string word = generator.GetWord();
            game.Play(word); 
            
            if (Ask("Play again?")) continue;
            break;
        }
        
        
    }

     private static bool Ask(string question)
    {
        
        Console.WriteLine(question + " " + "[y]es / [n]o");
        var answer = Console.ReadLine().ToLower();
        switch (answer)
        {
            case "y" or "yes":
                return true;
            case "n" or "no":
                return false;
            default:
                Console.WriteLine("Invalid input.");
                return Ask(question);
                
        }
        
    }
}



class Game
{

    private const int TotalAttempts = 6;
    // does not allow duplicate values
    private readonly HashSet<char> _usedLetters = new ();
    
    /// <summary>
    /// Updates the console with the game state each turn
    /// </summary>
    private void Update(string word, int attemptsRemaining)
    {
        Console.Clear();
        Picture.Draw(attemptsRemaining);
        Console.WriteLine();
        ShowWord(word);
        ShowUsedLetters();
        Console.WriteLine($"Attempts left: {attemptsRemaining}");
        
    }

    /// <summary>
    /// Main gameplay loop handler
    /// </summary>

    public void Play(string word)
    {
        
        for (int i = TotalAttempts; i > 0; i--)
        {
            while (true)
            {
                // Display current game state
                Update(word, i);
                
                // Get input
                var guessedString = GetInput();
                
                // Ensure input is valid
                if (!IsValid(guessedString)) continue;
                _usedLetters.Add(guessedString[0]);
                
                // Check if all letters in word has been guessed
                if (word.All(letter => _usedLetters.Contains(letter)))
                {
                    // Show win message
                    Update(word, i);
                    Console.WriteLine("You win!");
                    return;
                }
                // if guess not in word, decrement attempts remaining
                if (word.All(letter => guessedString[0] != letter)) break;
                
            }
        }
        
        // Show lose message
        Update(word, 0);
        Console.WriteLine("You lose!");
        Console.WriteLine($"The word was: {word}");
    }

    void Win(string word, int attemptsRemaining)
    {
        Update(word, attemptsRemaining);
        Console.WriteLine("You win!");
    }
    
    string GetInput()
    {
        Console.Write("Guess a letter: ");
        var guessedString = Console.ReadLine();
        return EmptyIfNull(guessedString).ToUpper();
    }
    
    private static string EmptyIfNull(string? guess)
    {
        return guess ?? "";
    }
    
    /// <summary>
    /// Will ensure that the input is valid 
    /// </summary>
    private bool IsValid(string input)
    {
        if (input == "")
            Console.WriteLine("Can't guess nothing! Press any key to try again...");
        else if (_usedLetters.Contains(input[0]))
            Console.WriteLine("Letter has already been used. Press any key to try again...");
        else return true;
        
        Console.ReadKey();
        return false;

    }

    /// <summary>
    /// Used in Update(), shows the letters that the player guessed correctly and their position in the word
    /// </summary>
    private void ShowWord(string word)
    {
        foreach (var letter in word) 
            Console.Write(_usedLetters.Contains(letter) ? $"{letter} " : "_ "); 
        Console.WriteLine();
    }
    
    /// <summary>
    /// Used in Update(), shows the letters that the player has guessed so far
    /// </summary>
    void ShowUsedLetters()
    {
        Console.Write("Used letters: ");
        foreach (var usedLetter in _usedLetters)
            Console.Write($"{usedLetter} ");
        Console.WriteLine();
    }
}
