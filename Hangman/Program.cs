// By: Edvin Hermansson

// Using about 500 words from https://www.kaggle.com/datasets/rtatman/english-word-frequency
// Processed using pandas
// filter params: 5 <= word.Length <= 10 and count > 10^8


namespace Hangman;

class Program
{
    
    public static void Main(string[] args)
    {
        const string fileName = "hangman_words.txt";
        var generator = CreateGenerator(fileName);
        
        while (true)
        {
            var word = generator.GetWord();
            var game = new Game(word);
            game.Play(); 
            
            
            if (Ask("Play again?")) continue;
            break;
        }
        
        
    }

    private static WordGenerator CreateGenerator(string fileName)
    {
        
        var wordsPath = Path.Combine(Environment.CurrentDirectory, fileName);
        return new WordGenerator(wordsPath);
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
    /// Main gameplay loop handler. Will be called from Main
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
            
            // Check if all letters in word has been guessed
            if (!_word.All(letter => _usedLetters.Contains(letter))) continue;
            // Show win message
            Update();
            Console.WriteLine("You win!"); 
            return;
            
        }
        
        // Show lose message
        Update();
        Console.WriteLine("You lose!");
        Console.WriteLine($"The word was: {_word}");
    }
    
    char GetGuess()
    {
        string guessString;
        do
        {
            // Display current game state
            Update();

            // Get input
            guessString = GetInput();
            // Check if valid else repeat
        } while (!IsValid(guessString));

        return guessString[0];
    }
    string GetInput()
    {
        Console.Write("Guess a letter: ");
        var guessedString = Console.ReadLine() ?? "";
        return guessedString.ToUpper();
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
    private void ShowWord()
    {
        foreach (var letter in _word) 
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
