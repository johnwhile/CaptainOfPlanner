namespace CaptainOfPlanner
{
    partial class BalanceControl
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
            this.comboResource = new System.Windows.Forms.ComboBox();
            this.buttonDecrease = new System.Windows.Forms.Button();
            this.buttonIncrease = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboResource
            // 
            this.comboResource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboResource.FormattingEnabled = true;
            this.comboResource.Location = new System.Drawing.Point(4, 21);
            this.comboResource.Name = "comboResource";
            this.comboResource.Size = new System.Drawing.Size(121, 21);
            this.comboResource.TabIndex = 2;
            this.comboResource.Text = "-- set resource --";
            this.comboResource.SelectedIndexChanged += new System.EventHandler(this.comboResource_SelectedIndexChanged);
            // 
            // buttonDecrease
            // 
            this.buttonDecrease.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDecrease.Location = new System.Drawing.Point(176, 21);
            this.buttonDecrease.Name = "buttonDecrease";
            this.buttonDecrease.Size = new System.Drawing.Size(21, 21);
            this.buttonDecrease.TabIndex = 3;
            this.buttonDecrease.Text = "-";
            this.buttonDecrease.UseVisualStyleBackColor = true;
            this.buttonDecrease.Click += new System.EventHandler(this.buttonDecrease_Click);
            // 
            // buttonIncrease
            // 
            this.buttonIncrease.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonIncrease.Location = new System.Drawing.Point(154, 21);
            this.buttonIncrease.Name = "buttonIncrease";
            this.buttonIncrease.Size = new System.Drawing.Size(21, 21);
            this.buttonIncrease.TabIndex = 4;
            this.buttonIncrease.Text = "+";
            this.buttonIncrease.UseVisualStyleBackColor = true;
            this.buttonIncrease.Click += new System.EventHandler(this.buttonIncrease_Click);
            // 
            // BalanceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.buttonIncrease);
            this.Controls.Add(this.buttonDecrease);
            this.Controls.Add(this.comboResource);
            this.Name = "BalanceControl";
            this.Controls.SetChildIndex(this.comboResource, 0);
            this.Controls.SetChildIndex(this.buttonDecrease, 0);
            this.Controls.SetChildIndex(this.buttonIncrease, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboResource;
        private System.Windows.Forms.Button buttonDecrease;
        private System.Windows.Forms.Button buttonIncrease;
    }
}
