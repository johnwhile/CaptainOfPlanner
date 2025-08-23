namespace CaptainOfPlanner
{
    partial class LinkerControl
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
            this.labelType = new System.Windows.Forms.Label();
            this.labelResource = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelType
            // 
            this.labelType.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelType.Font = new System.Drawing.Font("Arial Unicode MS", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelType.Location = new System.Drawing.Point(0, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(16, 20);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "IN";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelResource
            // 
            this.labelResource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResource.Location = new System.Drawing.Point(16, 0);
            this.labelResource.Name = "labelResource";
            this.labelResource.Size = new System.Drawing.Size(84, 20);
            this.labelResource.TabIndex = 1;
            this.labelResource.Text = "30 Resource";
            this.labelResource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LinkerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelResource);
            this.Controls.Add(this.labelType);
            this.Name = "LinkerControl";
            this.Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelResource;
    }
}
