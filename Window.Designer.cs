using CaptainOfPlanner.Controls;

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
            this.listControl = new CaptainOfPlanner.ListControl();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1013, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenu,
            this.SaveMenu});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // OpenMenu
            // 
            this.OpenMenu.Name = "OpenMenu";
            this.OpenMenu.Size = new System.Drawing.Size(180, 22);
            this.OpenMenu.Text = "Open planner";
            this.OpenMenu.Click += new System.EventHandler(this.OpenMenu_Click);
            // 
            // SaveMenu
            // 
            this.SaveMenu.Name = "SaveMenu";
            this.SaveMenu.Size = new System.Drawing.Size(180, 22);
            this.SaveMenu.Text = "Save planner";
            this.SaveMenu.Click += new System.EventHandler(this.SaveMenu_Click);
            // 
            // listControl
            // 
            this.listControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.listControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.listControl.Location = new System.Drawing.Point(0, 24);
            this.listControl.Name = "listControl";
            this.listControl.Size = new System.Drawing.Size(143, 529);
            this.listControl.TabIndex = 0;
            // 
            // plantViewer
            // 
            this.plantControl.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.plantControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.plantControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plantControl.Location = new System.Drawing.Point(143, 24);
            this.plantControl.Name = "plantViewer";
            this.plantControl.Size = new System.Drawing.Size(870, 529);
            this.plantControl.TabIndex = 2;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 553);
            this.Controls.Add(this.plantControl);
            this.Controls.Add(this.listControl);
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
        public ListControl listControl;
        public CaptainOfPlanner.Controls.PlantControl plantControl;
    }
}

