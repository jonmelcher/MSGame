// **************************************************************************************
//  MineField   - A class to represent a rectangular minefield as in the game MineSweeper
//              - minefield is represented as a byte[,] array
//              - each array element is broken down as follows:
//                0b00VMabcd where
//                  V = visible bit
//                  M = mine bit
//                  abcd represents a value between 0 and 8 of adjacent mines
//              - methods for generating a minefield and toggling V/M and accessing
//                properties are available
//
//  Written by Jonathan Melcher on June 24, 2015
//  Last updated June 24, 2015
// **************************************************************************************

using System;


namespace MSGame
{
    public class MineField
    {
        private static Random rng = new Random();       // random number generator
        private byte[,] field;                          // 2D array representing minefield
        private int mines;                              // mines in field

        // *************************************************************************
        //  constructor :   public MineField(int width, int height, int mines)
        //  parameters  :   int width - width of field (cannot be negative)
        //                  int height - height of field (cannot be negative)
        //                  int mines - number of mines on field (cannot be negative)
        // **************************************************************************
        public MineField(int width, int height, int mines)
        {
            if (width > 0 && height > 0 && mines > -1)
            {
                GenerateField(width, height, mines);
                this.mines = Math.Min(width * height, mines);
            }
            else
                throw new ArgumentOutOfRangeException(string.Format("{0}, {1}, {2}", width, height, mines));
        }

        // ***************************************************************
        // getter for amount of elements with mine-bit (0x10) set in field
        // ***************************************************************
        public int Mines
        {
            get { return mines; }
        }

        // *************************
        // getter for 2D field width
        // *************************
        public int Width
        {
            get { return field.GetLength(0); }
        }

        // **************************
        // getter for 2D field height
        // **************************
        public int Height
        {
            get { return field.GetLength(1); }
        }

        // *****************************************************************************************
        //  method :        public bool IsMine(int x, int y)
        //  parameters :    int x - given x-coordinate of 2D field
        //                  int y - given y-coordinate of 2D field
        //  purpose :       check if element at given (x, y) coordinate in 2D field is a mine or not
        //                  this is done by checking the mine bit (0x10)
        //  returns :       field[x, y] 0x10 bit set : true, else false
        // *****************************************************************************************
        public bool IsMine(int x, int y)
        {
            return (field[x, y] & 0x10) > 0;
        }

        // *******************************************************************************************
        //  method :        public bool IsVisible(int x, int y)
        //  parameters :    int x - given x-coordinate of 2D field
        //                  int y - given y-coordinate of 2D field
        //  purpose :       check if element at given (x, y) coordinate in 2D field is visible to user
        //                  this is done by checking the visible bit (0x20)
        //  returns :       field[x, y] 0x20 bit set : true, else false
        // *******************************************************************************************
        public bool IsVisible(int x, int y)
        {
            return (field[x, y] & 0x20) > 0;
        }

        // **************************************************************
        //  method :        public bool ToggleVisible(int x, int y)
        //  parameters :    int x - given x-coordinate of 2D field
        //                  int y - given y-coordinate of 2D field
        //  purpose :       toggles visible bit of element field[x, y]
        //                  this is done by xoring the visible bit (0x20)
        // **************************************************************
        public void ToggleVisible(int x, int y)
        {
            field[x, y] ^= 0x20;
        }

        // ***************************************************************************
        //  method :        public byte GetValue(int x, int y)
        //  parameters :    int x - given x-coordinate of 2D field
        //                  int y - given y-coordinate of 2D field
        //  purpose :       returns number of adjacent mines surrounding field[x, y]
        //  returns :       0 if element is a mine itself, or number of adjacent mines
        // ***************************************************************************
        public byte GetValue(int x, int y)
        {
            return (byte)(field[x, y] & 0x0F);
        }

        // *****************************************************************************************
        //  method :        private void GenerateField(int width, int height, int mines)
        //  purpose :       generate a 2D byte array where mines (& 0x10 == 1) are dispersed
        //                  randomly throughout the array, and every other value is a tally
        //                  of how many mines are surrounding them (up, down, left, right, diagonal)
        // *****************************************************************************************
        private void GenerateField(int width, int height, int mines)
        {
            field = new byte[width, height];        // initialize class-level field
            DisperseMines(mines);                   // start by dispersing mines randomly

            // iterate through field and tally up mines surrounding elements which are not mines
            // and set their value to that tally
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    if (!IsMine(i, j))
                        field[i, j] |= TallySurroundingMines(i, j);
        }

        // *********************************************************************
        //  method :        private static void DisperseMines(int mines)
        //  purpose :       set the mine bit of mines amount of element randomly
        //                  in field
        // *********************************************************************
        private void DisperseMines(int mines)
        {
            int area = Width * Height;              // calculate total area
            byte[] layout = new byte[area];         // create linear array to shuffle

            // or the desired number of elements with 0x10 (mine flag)
            for (int i = 0; i < mines; ++i)
                layout[i] |= 0x10;

            // shuffle linear array to ensure uniform randomness
            Shuffle(layout);

            // assign now random linear array to 2D field
            for (int i = 0; i < area; ++i)
                if ((layout[i] & 0x10) > 0)
                    field[i % Width, i / Width] |= 0x10;
        }

        // *****************************************************************************
        //  method :        private byte TallySurroundingMines(int x, int y)
        //  parameters :    int x - specified x coordinate within field
        //                  int y - specified y coordinate within field
        //  purpose :       tally up all mines surrounding field[x,y] (u/d/l/r/diagonal)
        //  returns :       byte of tally
        //  notes :         will include field[x, y] itself if it is a mine
        // *****************************************************************************
        private byte TallySurroundingMines(int x, int y)
        {
            byte tally = 0;

            for (int i = x - 1; i < x + 2; ++i)
                for (int j = y - 1; j < y + 2; ++j)
                    if (!IsOutOfBounds(i, j) && IsMine(i, j))
                        ++tally;
            return tally;
        }

        // helper for checking if x, y are out of bounds of a 2D array
        public bool IsOutOfBounds(int x, int y)
        {
            if (x < 0 || !(x < Width) || y < 0 || !(y < Height))
                return true;
            return false;
        }

        // standard shuffle algorithm for a linear array
        private static void Shuffle(byte[] arr)
        {
            byte tempValue;
            int tempIndex;

            for (int i = 0; i < arr.Length - 1; ++i)
            {
                tempIndex = rng.Next(i, arr.Length);
                tempValue = arr[tempIndex];
                arr[tempIndex] = arr[i];
                arr[i] = tempValue;
            }
        }
    }
}