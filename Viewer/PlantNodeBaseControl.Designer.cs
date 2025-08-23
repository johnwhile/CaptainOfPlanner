namespace CaptainOfPlanner
{
    partial class PlantNodeBaseControl
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.labelTitle.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "label";
            this.labelTitle.Size = new System.Drawing.Size(197, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Plant Node";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackgroundImage = global::CaptainOfPlanner.Properties.Resources.close;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClose.Location = new System.Drawing.Point(174, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(20, 20);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // PlantNodeBaseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelTitle);
            this.Name = "PlantNodeBaseControl";
            this.Size = new System.Drawing.Size(197, 159);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label labelTitle;
        public System.Windows.Forms.Button buttonClose;
    }
}
