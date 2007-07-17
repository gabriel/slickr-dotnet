#region License
/*
 *  Copyright (C) 2006-2007 Gabriel Handford <gabrielh@gmail.com>
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 *  MA 02110-1301, USA.
 */
#endregion License

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Reflection;

namespace Slickr
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class DebugInfoForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public DebugInfoForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

        }

		public void Add(string key, string val) 
		{
			ListViewItem item = new ListViewItem();
			item.Text = key;                
			item.SubItems.Add(val);
			listView.Items.Add(item);
		}

        public void AddProperties(object obj) 
        {
            PropertyInfo[] pia = obj.GetType().GetProperties();         
            //ListViewItem[] items = new ListViewItem[pia.Length];
            for(int i = 0; i < pia.Length; i++)
            {               
                string key = pia[i].Name;
                string val = String.Format("{0}", pia[i].GetValue(obj, new object[0]));
                //items[i] = new ListViewItem();
                //items[i].Text = key;                
                //items[i].SubItems.Add(val);             
				Add(key, val);
            }   
            //listView.Items.AddRange(items);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			// TODO: Fix icon
            //System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DebugInfoForm));
            this.okButton = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(344, 352);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(79, 24);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                       this.columnHeader1,
                                                                                       this.columnHeader2});
            this.listView.FullRowSelect = true;
            this.listView.Location = new System.Drawing.Point(8, 8);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(416, 336);
            this.listView.TabIndex = 4;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Key";
            this.columnHeader1.Width = 186;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 207;
            // 
            // DebugInfoForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(432, 382);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.okButton);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DebugInfoForm";
            this.Text = "Debug Info";
            this.ResumeLayout(false);

        }
        #endregion

        private void okButton_Click(object sender, System.EventArgs e)
        {           
            Dispose();
        }
    }
}
