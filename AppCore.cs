using System;
using System.Drawing;

namespace Ex05
{
    public static class AppCore
    {
        public enum eGameColor
        {
            Red,
            Green,
            Blue,
            Yellow,
            Orange,
            Purple,
            Cyan,
            Magenta
        }
        public enum eGuessResult { CorrectPosition, CorrectColor, Wrong }
        private static Color[] s_Colors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Orange, Color.Purple, Color.Cyan, Color.Magenta };

        public static eGameColor[] ConvertColorsToGameColors(Color[] i_Colors)
        {
            eGameColor[] gameColors = new eGameColor[i_Colors.Length];
            for (int i = 0; i < i_Colors.Length; i++)
            {
                string colorName = i_Colors[i].Name;
                if (Enum.TryParse(colorName, out eGameColor currentElement))
                {
                    gameColors[i] = currentElement;
                }
                else
                {
                    throw new ArgumentException($"Color '{colorName}' is not a valid GameColor.");
                }
            }

            return gameColors;
        }

        public static Color[] GetColors()
        {
            return s_Colors;
        }
    }
}
