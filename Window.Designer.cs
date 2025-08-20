namespace CaptainOfPlanner
{
    partial class Window
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemList = new CaptainOfPlanner.ItemList();
            this.PropPanel = new CaptainOfPlanner.PropPanel();
            this.ViewPanel = new CaptainOfPlanner.ViewPanel();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(943, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenu,
            this.SaveMenu});
            this.FileMenu.Name = "fileToolStripMenuItem";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // openMenu
            // 
            this.OpenMenu.Name = "openMenu";
            this.OpenMenu.Size = new System.Drawing.Size(146, 22);
            this.OpenMenu.Text = "Open planner";
            // 
            // saveMenu
            // 
            this.SaveMenu.Name = "saveMenu";
            this.SaveMenu.Size = new System.Drawing.Size(146, 22);
            this.SaveMenu.Text = "Save planner";
            // 
            // ItemList
            // 
            this.ItemList.Dock = System.Windows.Forms.DockStyle.Left;
            this.ItemList.FormattingEnabled = true;
            this.ItemList.Location = new System.Drawing.Point(0, 24);
            this.ItemList.Name = "ItemList";
            this.ItemList.Size = new System.Drawing.Size(131, 480);
            this.ItemList.TabIndex = 1;
            // 
            // PropPanel
            // 
            this.PropPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PropPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.PropPanel.Location = new System.Drawing.Point(676, 24);
            this.PropPanel.Name = "PropPanel";
            this.PropPanel.Size = new System.Drawing.Size(267, 480);
            this.PropPanel.TabIndex = 2;
            // 
            // ViewPanel
            // 
            this.ViewPanel.BackColor = System.Drawing.Color.LemonChiffon;
            this.ViewPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewPanel.Location = new System.Drawing.Point(131, 24);
            this.ViewPanel.Name = "ViewPanel";
            this.ViewPanel.Size = new System.Drawing.Size(545, 480);
            this.ViewPanel.TabIndex = 3;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 504);
            this.Controls.Add(this.ViewPanel);
            this.Controls.Add(this.PropPanel);
            this.Controls.Add(this.ItemList);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Window";
            this.Text = "Form1";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem OpenMenu;
        private System.Windows.Forms.ToolStripMenuItem SaveMenu;
        private ItemList ItemList;
        private ViewPanel ViewPanel;
        private PropPanel PropPanel;
    }
}

