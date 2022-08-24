namespace sokoball_edit
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lb_legend = new System.Windows.Forms.Label();
            this.mainm = new System.Windows.Forms.MainMenu(this.components);
            this.mi_file = new System.Windows.Forms.MenuItem();
            this.mi_load = new System.Windows.Forms.MenuItem();
            this.mi_save = new System.Windows.Forms.MenuItem();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // lb_legend
            // 
            this.lb_legend.Location = new System.Drawing.Point(490, 0);
            this.lb_legend.Name = "lb_legend";
            this.lb_legend.Size = new System.Drawing.Size(137, 443);
            this.lb_legend.TabIndex = 0;
            // 
            // mainm
            // 
            this.mainm.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mi_file});
            // 
            // mi_file
            // 
            this.mi_file.Index = 0;
            this.mi_file.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mi_load,
            this.mi_save});
            this.mi_file.Text = "File";
            // 
            // mi_load
            // 
            this.mi_load.Index = 0;
            this.mi_load.Text = "Load";
            this.mi_load.Click += new System.EventHandler(this.mi_load_Click);
            // 
            // mi_save
            // 
            this.mi_save.Index = 1;
            this.mi_save.Text = "Save";
            this.mi_save.Click += new System.EventHandler(this.mi_save_Click);
            // 
            // ofd
            // 
            this.ofd.Filter = "SOK files|*.sok";
            // 
            // sfd
            // 
            this.sfd.Filter = "SOK files|*.sok";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.lb_legend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Menu = this.mainm;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sokoball Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lb_legend;
        private System.Windows.Forms.MainMenu mainm;
        private System.Windows.Forms.MenuItem mi_file;
        private System.Windows.Forms.MenuItem mi_load;
        private System.Windows.Forms.MenuItem mi_save;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.SaveFileDialog sfd;
    }
}

