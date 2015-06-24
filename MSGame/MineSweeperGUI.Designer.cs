namespace MSGame
{
    partial class MineSweeperGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mineFieldGUI = new System.Windows.Forms.Panel();
            this.menuGUI = new System.Windows.Forms.MenuStrip();
            this.fileGUI = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameGUI = new System.Windows.Forms.ToolStripMenuItem();
            this.exitGUI = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsGUI = new System.Windows.Forms.ToolStripMenuItem();
            this.rowGUI = new System.Windows.Forms.ToolStripMenuItem();
            this.rowNumberGUI = new System.Windows.Forms.ToolStripTextBox();
            this.columnGUI = new System.Windows.Forms.ToolStripMenuItem();
            this.columnNumberGUI = new System.Windows.Forms.ToolStripTextBox();
            this.mineGUI = new System.Windows.Forms.ToolStripMenuItem();
            this.mineNumberGUI = new System.Windows.Forms.ToolStripTextBox();
            this.menuGUI.SuspendLayout();
            this.SuspendLayout();
            // 
            // mineFieldGUI
            // 
            this.mineFieldGUI.Location = new System.Drawing.Point(0, 27);
            this.mineFieldGUI.Name = "mineFieldGUI";
            this.mineFieldGUI.Size = new System.Drawing.Size(584, 332);
            this.mineFieldGUI.TabIndex = 0;
            // 
            // menuGUI
            // 
            this.menuGUI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileGUI,
            this.settingsGUI});
            this.menuGUI.Location = new System.Drawing.Point(0, 0);
            this.menuGUI.Name = "menuGUI";
            this.menuGUI.Size = new System.Drawing.Size(584, 24);
            this.menuGUI.TabIndex = 1;
            this.menuGUI.Text = "Menu";
            // 
            // fileGUI
            // 
            this.fileGUI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameGUI,
            this.exitGUI});
            this.fileGUI.Name = "fileGUI";
            this.fileGUI.Size = new System.Drawing.Size(37, 20);
            this.fileGUI.Text = "File";
            // 
            // newGameGUI
            // 
            this.newGameGUI.Name = "newGameGUI";
            this.newGameGUI.Size = new System.Drawing.Size(159, 22);
            this.newGameGUI.Text = "Start New Game";
            this.newGameGUI.Click += new System.EventHandler(this.newGameGUI_Click);
            // 
            // exitGUI
            // 
            this.exitGUI.Name = "exitGUI";
            this.exitGUI.Size = new System.Drawing.Size(159, 22);
            this.exitGUI.Text = "Exit";
            this.exitGUI.Click += new System.EventHandler(this.exitGUI_Click);
            // 
            // settingsGUI
            // 
            this.settingsGUI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rowGUI,
            this.columnGUI,
            this.mineGUI});
            this.settingsGUI.Name = "settingsGUI";
            this.settingsGUI.Size = new System.Drawing.Size(61, 20);
            this.settingsGUI.Text = "Settings";
            this.settingsGUI.DropDownClosed += new System.EventHandler(this.settingsGUI_DropDownClosed);
            // 
            // rowGUI
            // 
            this.rowGUI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rowNumberGUI});
            this.rowGUI.Name = "rowGUI";
            this.rowGUI.Size = new System.Drawing.Size(122, 22);
            this.rowGUI.Text = "Rows";
            // 
            // rowNumberGUI
            // 
            this.rowNumberGUI.Name = "rowNumberGUI";
            this.rowNumberGUI.Size = new System.Drawing.Size(100, 23);
            // 
            // columnGUI
            // 
            this.columnGUI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.columnNumberGUI});
            this.columnGUI.Name = "columnGUI";
            this.columnGUI.Size = new System.Drawing.Size(122, 22);
            this.columnGUI.Text = "Columns";
            // 
            // columnNumberGUI
            // 
            this.columnNumberGUI.Name = "columnNumberGUI";
            this.columnNumberGUI.Size = new System.Drawing.Size(100, 23);
            // 
            // mineGUI
            // 
            this.mineGUI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mineNumberGUI});
            this.mineGUI.Name = "mineGUI";
            this.mineGUI.Size = new System.Drawing.Size(122, 22);
            this.mineGUI.Text = "Mines";
            // 
            // mineNumberGUI
            // 
            this.mineNumberGUI.Name = "mineNumberGUI";
            this.mineNumberGUI.Size = new System.Drawing.Size(100, 23);
            // 
            // MineSweeperGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.mineFieldGUI);
            this.Controls.Add(this.menuGUI);
            this.MainMenuStrip = this.menuGUI;
            this.Name = "MineSweeperGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MineSweeper";
            this.menuGUI.ResumeLayout(false);
            this.menuGUI.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel mineFieldGUI;
        private System.Windows.Forms.MenuStrip menuGUI;
        private System.Windows.Forms.ToolStripMenuItem fileGUI;
        private System.Windows.Forms.ToolStripMenuItem newGameGUI;
        private System.Windows.Forms.ToolStripMenuItem exitGUI;
        private System.Windows.Forms.ToolStripMenuItem settingsGUI;
        private System.Windows.Forms.ToolStripMenuItem rowGUI;
        private System.Windows.Forms.ToolStripMenuItem columnGUI;
        private System.Windows.Forms.ToolStripMenuItem mineGUI;
        private System.Windows.Forms.ToolStripTextBox rowNumberGUI;
        private System.Windows.Forms.ToolStripTextBox columnNumberGUI;
        private System.Windows.Forms.ToolStripTextBox mineNumberGUI;
    }
}

