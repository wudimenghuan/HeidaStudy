namespace HeidaStudy
{
    partial class FormCourse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCourse));
            this.lstCourse = new DevComponents.DotNetBar.ListBoxAdv();
            this.SuspendLayout();
            // 
            // lstCourse
            // 
            this.lstCourse.AutoScroll = true;
            // 
            // 
            // 
            this.lstCourse.BackgroundStyle.Class = "ListBoxAdv";
            this.lstCourse.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lstCourse.CheckStateMember = null;
            this.lstCourse.ContainerControlProcessDialogKey = true;
            this.lstCourse.DragDropSupport = true;
            this.lstCourse.Location = new System.Drawing.Point(12, 12);
            this.lstCourse.Name = "lstCourse";
            this.lstCourse.Size = new System.Drawing.Size(376, 372);
            this.lstCourse.TabIndex = 0;
            this.lstCourse.Text = "listBoxAdv1";
            this.lstCourse.DoubleClick += new System.EventHandler(this.lstCourse_DoubleClick);
            // 
            // FormCourse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(400, 396);
            this.ControlBox = false;
            this.Controls.Add(this.lstCourse);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(416, 435);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(416, 435);
            this.Name = "FormCourse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择课程";
            this.TitleText = "双击选择课程";
            this.Load += new System.EventHandler(this.FormCourse_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ListBoxAdv lstCourse;
    }
}