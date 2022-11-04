namespace Hangman;

public class Picture
{
    public static void Draw(int attemptsRemaining)
    {
        switch (attemptsRemaining)
        {
            case 6:
                Console.WriteLine("    ____\n" +
                                  "   |    |\n" +
                                  "   |\n" +
                                  "   |\n" +
                                  "   |\n" +
                                  " __^__ \n" +
                                  "/     \\");
                break;
            case 5:
                Console.WriteLine("    ____\n" +
                                  "   |    |\n" +
                                  "   |    O\n" +
                                  "   |\n" +
                                  "   |\n" +
                                  " __^__ \n" +
                                  "/     \\");
                break;
            case 4:
                Console.WriteLine("    ____\n" +
                                  "   |    |\n" +
                                  "   |    O\n" +
                                  "   |    |\n" +
                                  "   |\n" +
                                  " __^__ \n" +
                                  "/     \\");
                break;
            case 3:
                Console.WriteLine("    ____\n" +
                                  "   |    |\n" +
                                  "   |    O\n" +
                                  "   |   /|\n" +
                                  "   |\n" +
                                  " __^__ \n" +
                                  "/     \\");
                break;
            case 2:
                Console.WriteLine("    ____\n" +
                                  "   |    |\n" +
                                  "   |    O\n" +
                                  "   |   /|\\\n" +
                                  "   |\n" +
                                  " __^__ \n" +
                                  "/     \\");
                break;
            case 1:
                Console.WriteLine("    ____\n" +
                                  "   |    |\n" +
                                  "   |    O\n" +
                                  "   |   /|\\\n" +
                                  "   |   /\n" +
                                  " __^__ \n" +
                                  "/     \\");
                break;
            case 0:
                Console.WriteLine("    ____\n" +
                                  "   |    |\n" +
                                  "   |    O\n" +
                                  "   |   /|\\\n" +
                                  "   |   / \\\n" +
                                  " __^__ \n" +
                                  "/     \\");
                break;
            default:
                Console.WriteLine("Something went wrong!");
                break;
        }
    }
}