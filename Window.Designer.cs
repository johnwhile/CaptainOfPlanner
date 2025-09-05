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
            this.MenuItemTool = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemRun = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemProcessor = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemBalancer = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemStorage = new System.Windows.Forms.ToolStripMenuItem();
            this.plantControl = new CaptainOfPlanner.PlantControl();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.MenuItemTool});
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
            this.OpenMenu.Size = new System.Drawing.Size(146, 22);
            this.OpenMenu.Text = "Open planner";
            this.OpenMenu.Click += new System.EventHandler(this.OpenMenu_Click);
            // 
            // SaveMenu
            // 
            this.SaveMenu.Name = "SaveMenu";
            this.SaveMenu.Size = new System.Drawing.Size(146, 22);
            this.SaveMenu.Text = "Save planner";
            this.SaveMenu.Click += new System.EventHandler(this.SaveMenu_Click);
            // 
            // MenuItemTool
            // 
            this.MenuItemTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemRun,
            this.toolStripSeparator,
            this.MenuItemProcessor,
            this.MenuItemBalancer,
            this.MenuItemStorage});
            this.MenuItemTool.Name = "MenuItemTool";
            this.MenuItemTool.Size = new System.Drawing.Size(46, 20);
            this.MenuItemTool.Text = "Tools";
            // 
            // MenuItemRun
            // 
            this.MenuItemRun.Image = global::CaptainOfPlanner.Properties.Resources.run;
            this.MenuItemRun.Name = "MenuItemRun";
            this.MenuItemRun.Size = new System.Drawing.Size(180, 22);
            this.MenuItemRun.Text = "Run";
            this.MenuItemRun.ToolTipText = "analyze the graph and calculate the flows for a certain number of interactions";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // MenuItemProcessor
            // 
            this.MenuItemProcessor.Image = global::CaptainOfPlanner.Properties.Resources.processor;
            this.MenuItemProcessor.Name = "MenuItemProcessor";
            this.MenuItemProcessor.Size = new System.Drawing.Size(180, 22);
            this.MenuItemProcessor.Text = "Processor";
            this.MenuItemProcessor.ToolTipText = "Insert a Processor node.\r\nUse a recipe to transform input elements into output el" +
    "ements";
            this.MenuItemProcessor.Click += new System.EventHandler(this.MenuItemProcessor_Click);
            // 
            // MenuItemBalancer
            // 
            this.MenuItemBalancer.Image = global::CaptainOfPlanner.Properties.Resources.balancer;
            this.MenuItemBalancer.Name = "MenuItemBalancer";
            this.MenuItemBalancer.Size = new System.Drawing.Size(180, 22);
            this.MenuItemBalancer.Text = "Balancer";
            this.MenuItemBalancer.ToolTipText = "Insert a Balancer node.\r\nUsed to merge and/or separate flows of a single resource" +
    " type";
            this.MenuItemBalancer.Click += new System.EventHandler(this.MenuItemBalancer_Click);
            // 
            // MenuItemStorage
            // 
            this.MenuItemStorage.Image = global::CaptainOfPlanner.Properties.Resources.storage;
            this.MenuItemStorage.Name = "MenuItemStorage";
            this.MenuItemStorage.Size = new System.Drawing.Size(180, 22);
            this.MenuItemStorage.Text = "Storage";
            this.MenuItemStorage.ToolTipText = "Insert a Storage node.\r\nUsed to normalize flows";
            this.MenuItemStorage.Click += new System.EventHandler(this.MenuItemStorage_Click);
            // 
            // plantControl
            // 
            this.plantControl.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.plantControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.plantControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plantControl.Location = new System.Drawing.Point(143, 24);
            this.plantControl.Name = "plantControl";
            this.plantControl.Plant = null;
            this.plantControl.Size = new System.Drawing.Size(870, 529);
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
        private System.Windows.Forms.ToolStripMenuItem OpenMenu;
        private System.Windows.Forms.ToolStripMenuItem SaveMenu;
        public PlantControl plantControl;
        private System.Windows.Forms.ToolStripMenuItem MenuItemTool;
        private System.Windows.Forms.ToolStripMenuItem MenuItemProcessor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem MenuItemRun;
        private System.Windows.Forms.ToolStripMenuItem MenuItemBalancer;
        private System.Windows.Forms.ToolStripMenuItem MenuItemStorage;
    }
}

