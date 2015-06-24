// ***************************************************************************
//  MineSweeperGUI - a C# Windows Forms implementation of the game Minesweeper
//                 - depends on MineField class
//
//  Written by Jonathan Melcher on June 24, 2015
//  Last updated June 24, 2015
// ***************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MSGame
{
    public partial class MineSweeperGUI : Form
    {
        private const int rowNumberDefault = 10;        // default number of rows
        private const int rowNumberMax = 100;           // maximum number of rows
        private const int columnNumberDefault = 10;     // default number of columns
        private const int columnNumberMax = 100;        // maximum number of columns
        private const int mineNumberDefault = 10;       // default number of mines
        private const int tileDimension = 25;           // GUI dimension of mine tile

        private int rowNumber;                          // current number of rows
        private int columnNumber;                       // current number of columns
        private int mineNumber;                         // current number of mines

        private bool gameOver;                          // is current game over?
        private int flags;                              // number of flags created by user in current game
        private int unclicked;                          // number of unclicked tiles in current game
        private MineField mineFieldData;                // data for current game

        // constructor
        public MineSweeperGUI()
        {
            InitializeComponent();
            InitMineFieldSettings();
        }

        // *************************************************
        //  method :    private void InitMineFieldSettings()
        //  purpose :   initialize globals and update GUI
        // *************************************************
        private void InitMineFieldSettings()
        {
            rowNumber = rowNumberDefault;
            columnNumber = columnNumberDefault;
            mineNumber = mineNumberDefault;
            gameOver = false;
            flags = 0;
            unclicked = 0;
            UpdateMineFieldSettingsInMenu();
        }

        // ***************************************************************************
        //  method :    private void UpdateClickedMineFieldTile(int controlIndex)
        //  purpose :   routine for updating mineFieldGUI tiles based on corresponding
        //              mineFieldData.field points.  Updates global game flags/values
        //              in order to process win condition.  Depends on the method
        //              UpdateZeroAdjacentMineTiles for the recursion case where the
        //              button clicked has no adjacent mines
        // ***************************************************************************
        private void UpdateClickedMineFieldTile(int controlIndex)
        {
            int fieldCol = controlIndex % mineFieldData.Width;          // get column from linear index
            int fieldRow = controlIndex / mineFieldData.Width;          // get row from linear index
            int value = mineFieldData.GetValue(fieldCol, fieldRow);     // get number of adjacent mines

            // if tile is visible it has already been addressed so do nothing
            if (mineFieldData.IsVisible(fieldCol, fieldRow) || gameOver)
                return;

            mineFieldData.ToggleVisible(fieldCol, fieldRow);

            // adjust global game values for processing win condition
            switch (mineFieldGUI.Controls[controlIndex].Text)
            {
                case "":
                    --unclicked;
                    break;
                case "F":
                    --flags;
                    break;
            }

            if (mineFieldData.IsMine(fieldCol, fieldRow))
            {
                mineFieldGUI.Controls[controlIndex].Text = "M";
                gameOver = true;
            }
            else if (value != 0)
                mineFieldGUI.Controls[controlIndex].Text = value.ToString();

            // if number of adjacent mines is zero, enter recursion case to clear out all zeros in same group
            else
                UpdateZeroAdjacentMineTiles(controlIndex, fieldCol, fieldRow);
        }

        // ***************************************************************************************************
        //  method :    private void UpdateZeroAdjacentMineTiles(int controlIndex, int fieldCol, int fieldRow)
        //  purpose :   subroutine for clearing out mineFieldGUI tiles with no adjacent mines
        //              these tiles form a strongly-connected component on the grid and will all be cleared
        //              via this recursion subroutine and the main routine as a stop-check
        // ***************************************************************************************************
        private void UpdateZeroAdjacentMineTiles(int controlIndex, int fieldCol, int fieldRow)
        {
            mineFieldGUI.Controls[controlIndex].Text = "";
            ((Button)mineFieldGUI.Controls[controlIndex]).FlatStyle = FlatStyle.Flat;

            for (int col = fieldCol - 1; col < fieldCol + 2; ++col)
                for (int row = fieldRow - 1; row < fieldRow + 2; ++row)
                    if (!mineFieldData.IsOutOfBounds(col, row))
                        UpdateClickedMineFieldTile(row * mineFieldData.Width + col);
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
                    UpdateClickedMineFieldTile(controlIndex);
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
            else if (flags + unclicked == mineFieldData.Mines)
            {
                gameOver = true;
                MessageBox.Show("You have won!");
            }
        }

        // ***********************************************************************
        //  method :    private void ProcessRightClick(int controlIndex)
        //  purpose :   cycle through texts of given button in a well-defined way
        //              and adjust global game values for processing win-condition
        // ***********************************************************************
        private void ProcessRightClick(int controlIndex)
        {
            switch (mineFieldGUI.Controls[controlIndex].Text)
            {
                case "":
                    mineFieldGUI.Controls[controlIndex].Text = "F";
                    --unclicked;
                    ++flags;
                    break;
                case "F":
                    mineFieldGUI.Controls[controlIndex].Text = "?";
                    --flags;
                    break;
                case "?":
                    mineFieldGUI.Controls[controlIndex].Text = "";
                    ++unclicked;
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
            if (gameOver)
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
            flags = 0;
            unclicked = columnNumber * rowNumber;
            mineFieldData = new MineField(columnNumber, rowNumber, mineNumber);

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
