// $G$ SFN-002 (-3) The program does not display a window for selecting number of chances as required.
// $G$ SFN-008 (-3) The program allows selecting the same color more than once in a single guess, which is not allowed.

namespace Ex05
{
    // $G$ CSS-999 (-2) The class must have an access modifier.
    static class Program
    {
        // $G$ CSS-999 (-2) The method must have an access modifier, although this is the main method.
        static void Main()
        {
            // $G$ CSS-999 (-2) Missing blank line, after variable declarations.
            GameManager game = new GameManager();
            game.Run();
        }
    }
}