// ***************************************************************************
//  MineSweeperGUI - a C# Windows Forms implementation of the game Minesweeper
//                 - depends on MineField class
//
//  Written by Jonathan Melcher on June 24, 2015
//  Last updated June 25, 2015
// ***************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;


namespace MSGame
{
    public partial class MineSweeperGUI : Form
    {
        private const int rowNumberDefault = 10;        // default number of rows
        private const int rowNumberMax = 50;            // maximum number of rows
        private const int columnNumberDefault = 10;     // default number of columns
        private const int columnNumberMax = 50;         // maximum number of columns
        private const int mineNumberDefault = 10;       // default number of mines
        private const int tileDimension = 25;           // GUI dimension of mine tile

        private int rowNumber;                          // current number of rows
        private int columnNumber;                       // current number of columns
        private int mineNumber;                         // current number of mines

        private bool gameOver;                          // is current game over?
        private int visible;                            // tally of visible tiles
        private MineField mineFieldEngine;              // data for current game

        // constructor
        public MineSweeperGUI()
        {
            InitializeComponent();
            InitMineFieldSettings();
        }

        // *******************************************************************************
        //  method :    private void InitMineFieldSettings()
        //  purpose :   initialize globals and update GUI to reflect default global values
        // *******************************************************************************
        private void InitMineFieldSettings()
        {
            rowNumber = rowNumberDefault;
            columnNumber = columnNumberDefault;
            mineNumber = mineNumberDefault;
            gameOver = false;
            visible = 0;
            UpdateMineFieldSettingsInMenu();
        }

        // *******************************************************************************
        //  method :    private void UncoverTile(int controlIndex)
        //  purpose :   routine for uncovering mineFieldGUI tiles based on corresponding
        //              mineFieldData.field points.  Checks whether tile has been checked
        //              or is a mine before starting stack process.  Stack process takes
        //              next tile to uncover, updates global game flags/values in order to
        //              process win condition, then adds more tiles to the stack if the
        //              adjacent mines are zero.
        // *******************************************************************************
        private void UncoverTile(int controlIndex)
        {
            Stack<Tuple<int, int>> tiles = new Stack<Tuple<int, int>>();        // stack for tile uncovering process
            int fieldCol = controlIndex % mineFieldEngine.Width;                // 2D column index from linear index
            int fieldRow = controlIndex / mineFieldEngine.Width;                // 2D row index from linear index
            int linearIndex;                                                    // linear index for mineFieldGUI.Controls
            int value;                                                          // mineFieldData.GetValue(fieldCol, fieldRow)
            Tuple<int, int> packedIndex;                                        // packaged fieldCol/fieldRow

            // check that tile hasn't been clicked before
            if (mineFieldEngine.IsVisible(fieldCol, fieldRow))
                return;

            // check if tile is a mine before starting stack process
            if (mineFieldEngine.IsMine(fieldCol, fieldRow))
            {
                mineFieldGUI.Controls[controlIndex].Text = "M";
                gameOver = true;
                return;
            }

            // begin stack process to uncover one or more tiles
            mineFieldEngine.SetVisible(fieldCol, fieldRow);
            tiles.Push(Tuple.Create(fieldCol, fieldRow));
            while (tiles.Count != 0)
            {
                packedIndex = tiles.Pop();                                      // grab next tile to uncover
                fieldCol = packedIndex.Item1;                                   // unpack column index
                fieldRow = packedIndex.Item2;                                   // unpack row index
                linearIndex = fieldRow * mineFieldEngine.Width + fieldCol;      // get linear control index
                value = mineFieldEngine.GetValue(fieldCol, fieldRow);           // get number of adjacent mines
                ++visible;                                                      // update global for processing win condition

                if (value != 0)
                    mineFieldGUI.Controls[linearIndex].Text = value.ToString();
                else
                {
                    // if number of adjacent mines is zero, add all non-visible tiles surrounding to stack
                    mineFieldGUI.Controls[linearIndex].Text = "";
                    ((Button)mineFieldGUI.Controls[linearIndex]).FlatStyle = FlatStyle.Flat;

                    for (int col = fieldCol - 1; col < fieldCol + 2; ++col)
                        for (int row = fieldRow - 1; row < fieldRow + 2; ++row)
                            if (!mineFieldEngine.IsOutOfBounds(col, row) && !mineFieldEngine.IsVisible(col, row))
                            {
                                mineFieldEngine.SetVisible(col, row);
                                tiles.Push(Tuple.Create(col, row));
                            }
                }
            }
        }

        // **********************************************************************
        //  method :    private void ProcessLeftClick(int controlIndex)
        //  purpose :   start updating mineFieldGUI only on specific button texts
        // **********************************************************************
        private void ProcessLeftClick(int controlIndex)
        {
            switch (mineFieldGUI.Controls[controlIndex].Text)
            {
                case "":
                case "F":
                case "?":
                    UncoverTile(controlIndex);
                    break;
            }
        }

        // ****************************************************************
        //  method :    private void ProcessWinCondition()
        //  purpose :   Prompt user as to whether he/she has won or lost if
        //              game is over
        // ****************************************************************
        private void ProcessWinCondition()
        {
            if (gameOver)
                MessageBox.Show("You have lost!");
            else if (mineFieldEngine.Width * mineFieldEngine.Height - visible == mineFieldEngine.Mines)
            {
                gameOver = true;
                MessageBox.Show("You have won!");
            }
        }

        // ***********************************************************************
        //  method :    private void ProcessRightClick(int controlIndex)
        //  purpose :   cycle through texts of given button in a well-defined way
        // ***********************************************************************
        private void ProcessRightClick(int controlIndex)
        {
            switch (mineFieldGUI.Controls[controlIndex].Text)
            {
                case "":
                    mineFieldGUI.Controls[controlIndex].Text = "F";
                    break;
                case "F":
                    mineFieldGUI.Controls[controlIndex].Text = "?";
                    break;
                case "?":
                    mineFieldGUI.Controls[controlIndex].Text = "";
                    break;
            }
        }

        // *************************************************************************************
        //  event-handler : private void MineFieldButtonClick(object sender, MouseEventArgs e)
        //  purpose : handle event that user uses mouse to click on a button in the mineFieldGUI
        //            - left-click uncovers tiles and moves ahead game
        //            - right-click cycles through "", "F", "?" texts for given button
        // *************************************************************************************
        private void MineFieldButtonClick(object sender, MouseEventArgs e)
        {
            if (gameOver || ((Button)sender).FlatStyle == FlatStyle.Flat)
                return;

            int controlIndex = mineFieldGUI.Controls.IndexOf((Control)sender);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    ProcessLeftClick(controlIndex);
                    break;
                case MouseButtons.Right:
                    ProcessRightClick(controlIndex);
                    break;
            }
            ProcessWinCondition();
        }

        // *************************************************************************************
        //  event-handler : private void newGameGUI_Click(object sender, EventArgs e)
        //  purpose :   set up GUI for a new-game.  this involves clearing out the old game GUI
        //              (mineFieldGUI controls), resetting flags, initializing a new MineField
        //              class with given parameters, and dynamically adding button controls to
        //              mineFieldGUI to represent each coordinate of the MineField.field.  This
        //              is done in a canonical way: field[x, y] = Controls[x * columnNumber + y]
        //              all button properties and event-handlers are added here
        // *************************************************************************************
        private void newGameGUI_Click(object sender, EventArgs e)
        {
            Button temp;

            // clear out old controls and reset mineFieldGUI size to reflect new parameters
            mineFieldGUI.Controls.Clear();
            mineFieldGUI.Size = new Size(columnNumber * tileDimension, rowNumber * tileDimension);

            // reset game flags/data to reflect a new game/new parameters
            gameOver = false;
            visible = 0;
            mineFieldEngine = new MineField(columnNumber, rowNumber, mineNumber);

            // create buttons for mineFieldGUI to represent each point on mineFieldData.field
            for (int j = 0; j < rowNumber; ++j)
                for (int i = 0; i < columnNumber; ++i)
                {
                    temp = new Button();
                    temp.Location = new Point(i * tileDimension, j * tileDimension);
                    temp.Size = new Size(tileDimension, tileDimension);
                    temp.MouseUp += new MouseEventHandler(MineFieldButtonClick);
                    mineFieldGUI.Controls.Add(temp);
                }
        }

        // ***********************************************************************
        //  event-handler : private void exitGUI_Click(object sender, EventArgs e)
        //  purpose :   allow the user to quit the program using File->Exit in the
        //              top menu strip
        // ***********************************************************************
        private void exitGUI_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        // ************************************************************************************
        //  event-handler : private void settingsGUI_DropDownClosed(object sender, EventArgs e)
        //  purpose :   validate and set the settings textboxes found in the top menu strip
        //              to reflect the MineField parameters and constraints of the program
        // ************************************************************************************
        private void settingsGUI_DropDownClosed(object sender, EventArgs e)
        {
            int tempRow;
            int tempCol;
            int tempMine;

            if (int.TryParse(rowNumberGUI.Text, out tempRow) && tempRow > 0 && tempRow < rowNumberMax)
                rowNumber = tempRow;
            if (int.TryParse(columnNumberGUI.Text, out tempCol) && tempCol > 0 && tempCol < columnNumberMax)
                columnNumber = tempCol;
            if (int.TryParse(mineNumberGUI.Text, out tempMine) && tempMine > -1 && tempMine < rowNumber * columnNumber)
                mineNumber = tempMine;

            UpdateMineFieldSettingsInMenu();
        }

        // ***********************************************************************************
        //  method :    private void UpdateMineFieldSettingsInMenu()
        //  purpose :   update all settings textboxes to reflect the parameters they represent
        // ***********************************************************************************
        private void UpdateMineFieldSettingsInMenu()
        {
            rowNumberGUI.Text = rowNumber.ToString();
            columnNumberGUI.Text = columnNumber.ToString();
            mineNumberGUI.Text = mineNumber.ToString();
        }
    }
}
