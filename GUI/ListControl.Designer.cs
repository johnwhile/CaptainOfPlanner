namespace CaptainOfPlanner
{
    partial class ListControl
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

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonInsert = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.itemList = new System.Windows.Forms.ListBox();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonInsert
            // 
            this.buttonInsert.Location = new System.Drawing.Point(3, 3);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(75, 23);
            this.buttonInsert.TabIndex = 1;
            this.buttonInsert.Text = "Insert";
            this.buttonInsert.UseVisualStyleBackColor = true;
            this.buttonInsert.Click += new System.EventHandler(this.InsertNode);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.buttonInsert);
            this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel.Location = new System.Drawing.Point(0, 235);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(231, 37);
            this.panel.TabIndex = 2;
            // 
            // listBox1
            // 
            this.itemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemList.FormattingEnabled = true;
            this.itemList.Location = new System.Drawing.Point(0, 0);
            this.itemList.Name = "listBox1";
            this.itemList.Size = new System.Drawing.Size(231, 235);
            this.itemList.TabIndex = 3;
            // 
            // ListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.itemList);
            this.Controls.Add(this.panel);
            this.Name = "ListControl";
            this.Size = new System.Drawing.Size(231, 272);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonInsert;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.ListBox itemList;
    }
}
