﻿namespace CaptainOfPlanner
{
    partial class StorageControl
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
            this.SuspendLayout();
            // 
            // comboResource
            // 
            this.comboResource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboResource.FormattingEnabled = true;
            this.comboResource.Location = new System.Drawing.Point(4, 21);
            this.comboResource.Name = "comboResource";
            this.comboResource.Size = new System.Drawing.Size(143, 21);
            this.comboResource.TabIndex = 1;
            this.comboResource.Text = "-- set resource --";
            this.comboResource.SelectedIndexChanged += new System.EventHandler(this.comboResource_SelectedIndexChanged);
            // 
            // StorageControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.comboResource);
            this.Controls.SetChildIndex(this.comboResource, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboResource;
    }
}
