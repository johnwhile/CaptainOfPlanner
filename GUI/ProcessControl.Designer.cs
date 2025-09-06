namespace CaptainOfPlanner
{
    partial class ProcessControl
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
            this.comboFilter = new System.Windows.Forms.ComboBox();
            this.comboRecipe = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboFilter
            // 
            this.comboFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboFilter.FormattingEnabled = true;
            this.comboFilter.Location = new System.Drawing.Point(3, 24);
            this.comboFilter.Name = "comboFilter";
            this.comboFilter.Size = new System.Drawing.Size(144, 21);
            this.comboFilter.TabIndex = 2;
            this.comboFilter.Text = "-- filter by resources --";
           
            // 
            // comboRecipe
            // 
            this.comboRecipe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboRecipe.FormattingEnabled = true;
            this.comboRecipe.Location = new System.Drawing.Point(3, 46);
            this.comboRecipe.Name = "comboRecipe";
            this.comboRecipe.Size = new System.Drawing.Size(144, 21);
            this.comboRecipe.TabIndex = 3;
            this.comboRecipe.Text = "-- select recipe --";
            
            // 
            // ProcessControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.comboRecipe);
            this.Controls.Add(this.comboFilter);
            this.Size = new System.Drawing.Size(150, 75);
            this.Controls.SetChildIndex(this.comboFilter, 0);
            this.Controls.SetChildIndex(this.comboRecipe, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboFilter;
        private System.Windows.Forms.ComboBox comboRecipe;
    }
}
