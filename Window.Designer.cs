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
            this.MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRun = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAddProcessor = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAddBalancer = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAddStorage = new System.Windows.Forms.ToolStripMenuItem();
            this.plantControl = new CaptainOfPlanner.PlantControl();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOpen,
            this.MenuSave,
            this.MenuRun,
            this.MenuAddProcessor,
            this.MenuAddBalancer,
            this.MenuAddStorage});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1013, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // MenuOpen
            // 
            this.MenuOpen.Image = global::CaptainOfPlanner.Properties.Resources.open;
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.Size = new System.Drawing.Size(64, 20);
            this.MenuOpen.Text = "Open";
            this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
            // 
            // MenuSave
            // 
            this.MenuSave.Image = global::CaptainOfPlanner.Properties.Resources.save;
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.Size = new System.Drawing.Size(59, 20);
            this.MenuSave.Text = "Save";
            this.MenuSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // MenuRun
            // 
            this.MenuRun.Image = global::CaptainOfPlanner.Properties.Resources.run;
            this.MenuRun.Name = "MenuRun";
            this.MenuRun.Size = new System.Drawing.Size(56, 20);
            this.MenuRun.Text = "Run";
            this.MenuRun.ToolTipText = "analyze the graph and calculate the flows for a certain number of interactions";
            // 
            // MenuAddProcessor
            // 
            this.MenuAddProcessor.Image = global::CaptainOfPlanner.Properties.Resources.processor;
            this.MenuAddProcessor.Name = "MenuAddProcessor";
            this.MenuAddProcessor.Size = new System.Drawing.Size(86, 20);
            this.MenuAddProcessor.Text = "Processor";
            this.MenuAddProcessor.ToolTipText = "Insert a Processor node.\r\nUse a recipe to transform input elements into output el" +
    "ements";
            this.MenuAddProcessor.Click += new System.EventHandler(this.MenuAddProcessor_Click);
            // 
            // MenuAddBalancer
            // 
            this.MenuAddBalancer.Image = global::CaptainOfPlanner.Properties.Resources.balancer;
            this.MenuAddBalancer.Name = "MenuAddBalancer";
            this.MenuAddBalancer.Size = new System.Drawing.Size(80, 20);
            this.MenuAddBalancer.Text = "Balancer";
            this.MenuAddBalancer.ToolTipText = "Insert a Balancer node.\r\nUsed to merge and/or separate flows of a single resource" +
    " type";
            this.MenuAddBalancer.Click += new System.EventHandler(this.MenuAddBalancer_Click);
            // 
            // MenuAddStorage
            // 
            this.MenuAddStorage.Image = global::CaptainOfPlanner.Properties.Resources.storage;
            this.MenuAddStorage.Name = "MenuAddStorage";
            this.MenuAddStorage.Size = new System.Drawing.Size(75, 20);
            this.MenuAddStorage.Text = "Storage";
            this.MenuAddStorage.ToolTipText = "Insert a Storage node.\r\nUsed to normalize flows";
            this.MenuAddStorage.Click += new System.EventHandler(this.MenuAddStorage_Click);
            // 
            // plantControl
            // 
            this.plantControl.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.plantControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.plantControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plantControl.Location = new System.Drawing.Point(0, 24);
            this.plantControl.Name = "plantControl";
            this.plantControl.Plant = null;
            this.plantControl.Size = new System.Drawing.Size(1013, 529);
            this.plantControl.TabIndex = 2;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 553);
            this.Controls.Add(this.plantControl);
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
        private System.Windows.Forms.ToolStripMenuItem MenuOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuSave;
        public PlantControl plantControl;
        private System.Windows.Forms.ToolStripMenuItem MenuItemTool;
        private System.Windows.Forms.ToolStripMenuItem MenuAddProcessor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem MenuRun;
        private System.Windows.Forms.ToolStripMenuItem MenuAddBalancer;
        private System.Windows.Forms.ToolStripMenuItem MenuAddStorage;
    }
}

