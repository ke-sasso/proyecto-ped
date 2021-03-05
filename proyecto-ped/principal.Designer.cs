
namespace proyecto_ped
{
    partial class principal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(principal));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nUEVOVERTICEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapa = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nUEVOVERTICEToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 26);
            // 
            // nUEVOVERTICEToolStripMenuItem
            // 
            this.nUEVOVERTICEToolStripMenuItem.Name = "nUEVOVERTICEToolStripMenuItem";
            this.nUEVOVERTICEToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.nUEVOVERTICEToolStripMenuItem.Text = "NUEVO VERTICE";
            this.nUEVOVERTICEToolStripMenuItem.Click += new System.EventHandler(this.nUEVOVERTICEToolStripMenuItem_Click);
            // 
            // mapa
            // 
            this.mapa.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mapa.BackgroundImage")));
            this.mapa.Location = new System.Drawing.Point(22, 12);
            this.mapa.Name = "mapa";
            this.mapa.Size = new System.Drawing.Size(787, 566);
            this.mapa.TabIndex = 2;
            this.mapa.Paint += new System.Windows.Forms.PaintEventHandler(this.mapa_Paint);
            this.mapa.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapa_MouseDown);
            this.mapa.MouseLeave += new System.EventHandler(this.mapa_MouseLeave);
            this.mapa.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapa_MouseMove);
            this.mapa.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapa_MouseUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(567, 614);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "label5";
            // 
            // principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1373, 695);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.mapa);
            this.Name = "principal";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.principal_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem nUEVOVERTICEToolStripMenuItem;
        private System.Windows.Forms.Panel mapa;
        private System.Windows.Forms.Label label5;
    }
}

