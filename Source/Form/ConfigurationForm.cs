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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using Slickr.Util;
using Slickr.Util.Windows;
using Slickr.Util.OpenGL;
using Tao.OpenGl;

namespace Slickr
{
    public sealed class ConfigurationForm : Form
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
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.tbAbout = new System.Windows.Forms.TabPage();
			this.chkOverrideGLCheck = new System.Windows.Forms.CheckBox();
			this.btnTestOpenGL = new System.Windows.Forms.Button();
			this.btnClearCache = new System.Windows.Forms.Button();
			this.debugButton = new System.Windows.Forms.Button();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.linkCdsw = new System.Windows.Forms.LinkLabel();
			this.linkEmail = new System.Windows.Forms.LinkLabel();
			this.lblVersion = new System.Windows.Forms.Label();
			this.tpFlickr = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.gbHelp = new System.Windows.Forms.GroupBox();
			this.lbHelp = new System.Windows.Forms.Label();
			this.gbFlickrGroup = new System.Windows.Forms.GroupBox();
			this.ckCheckGroup = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.tbFlickrGroup = new System.Windows.Forms.TextBox();
			this.gbLocal = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tbLocalDir = new System.Windows.Forms.TextBox();
			this.localButton = new System.Windows.Forms.Button();
			this.rbLocal = new System.Windows.Forms.RadioButton();
			this.gbFlickrUser = new System.Windows.Forms.GroupBox();
			this.chkRandomize = new System.Windows.Forms.CheckBox();
			this.cbFlickrContactType = new System.Windows.Forms.ComboBox();
			this.cbFlickrUserTagMode = new System.Windows.Forms.ComboBox();
			this.tbFlickrUserTags = new System.Windows.Forms.TextBox();
			this.tbFlickrUserSet = new System.Windows.Forms.TextBox();
			this.rbFlickrUserTag = new System.Windows.Forms.RadioButton();
			this.rbFlickrUserSet = new System.Windows.Forms.RadioButton();
			this.rbFlickrUserContacts = new System.Windows.Forms.RadioButton();
			this.rbFlickrUserFav = new System.Windows.Forms.RadioButton();
			this.rbFlickrUserAll = new System.Windows.Forms.RadioButton();
			this.rbFlickrUser = new System.Windows.Forms.RadioButton();
			this.rbFlickrEveryone = new System.Windows.Forms.RadioButton();
			this.tbFlickrUser = new System.Windows.Forms.TextBox();
			this.rbFlickrGroup = new System.Windows.Forms.RadioButton();
			this.gbFlickrEveryone = new System.Windows.Forms.GroupBox();
			this.chkFlickrInterestEnabled = new System.Windows.Forms.CheckBox();
			this.dtpFlickrEveryoneInterestDate = new System.Windows.Forms.DateTimePicker();
			this.rbFlickrEveryoneInterest = new System.Windows.Forms.RadioButton();
			this.cbFlickrEveryoneTagMode = new System.Windows.Forms.ComboBox();
			this.tbFlickrEveryoneTags = new System.Windows.Forms.TextBox();
			this.rbFlickrEveryoneTag = new System.Windows.Forms.RadioButton();
			this.rbFlickrEveryoneRecent = new System.Windows.Forms.RadioButton();
			this.btnCheckUser = new System.Windows.Forms.Button();
			this.tbOptions = new System.Windows.Forms.TabPage();
			this.chkIncludeUserInContacts = new System.Windows.Forms.CheckBox();
			this.ckShowLogo = new System.Windows.Forms.CheckBox();
			this.ckFadeEnabled = new System.Windows.Forms.CheckBox();
			this.ckPanEnabled = new System.Windows.Forms.CheckBox();
			this.ckZoomEnabled = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.cbMaxSizeLabel = new System.Windows.Forms.ComboBox();
			this.tbMinSize = new System.Windows.Forms.TextBox();
			this.baseDirButton = new System.Windows.Forms.Button();
			this.tbBaseDir = new System.Windows.Forms.TextBox();
			this.lbPowerTimeout = new System.Windows.Forms.Label();
			this.ckPowerSetting = new System.Windows.Forms.CheckBox();
			this.tbMaxCacheSize = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.ckShowStatus = new System.Windows.Forms.CheckBox();
			this.ckShowFile = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.lbView = new System.Windows.Forms.Label();
			this.tbView = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tbKey = new System.Windows.Forms.TabPage();
			this.label37 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.tbxSecret = new System.Windows.Forms.TextBox();
			this.tbxKey = new System.Windows.Forms.TextBox();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tbAuthorize = new System.Windows.Forms.TabPage();
			this.authCopyButton = new System.Windows.Forms.Button();
			this.tbAuthStatus = new System.Windows.Forms.TextBox();
			this.lbAuthStatus = new System.Windows.Forms.Label();
			this.removeAuthButton = new System.Windows.Forms.Button();
			this.completeAuthButton = new System.Windows.Forms.Button();
			this.authButton = new System.Windows.Forms.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.tbConnect = new System.Windows.Forms.TabPage();
			this.label32 = new System.Windows.Forms.Label();
			this.tbProxyPort = new System.Windows.Forms.TextBox();
			this.label31 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.tbProxyIpAddress = new System.Windows.Forms.TextBox();
			this.chkProxy = new System.Windows.Forms.CheckBox();
			this.tbProxyPassword = new System.Windows.Forms.TextBox();
			this.tbProxyDomain = new System.Windows.Forms.TextBox();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.tbProxyUsername = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.label16 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.trackBar2 = new System.Windows.Forms.TrackBar();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.trackBar3 = new System.Windows.Forms.TrackBar();
			this.label24 = new System.Windows.Forms.Label();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.trackBar4 = new System.Windows.Forms.TrackBar();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.baseDirBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.localDirBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.tbAbout.SuspendLayout();
			this.tpFlickr.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.gbHelp.SuspendLayout();
			this.gbFlickrGroup.SuspendLayout();
			this.gbLocal.SuspendLayout();
			this.gbFlickrUser.SuspendLayout();
			this.gbFlickrEveryone.SuspendLayout();
			this.tbOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbView)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tbKey.SuspendLayout();
			this.tbAuthorize.SuspendLayout();
			this.tbConnect.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(204, 392);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(79, 24);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(289, 392);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(72, 24);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// tbAbout
			// 
			this.tbAbout.BackColor = System.Drawing.Color.Transparent;
			this.tbAbout.Controls.Add(this.chkOverrideGLCheck);
			this.tbAbout.Controls.Add(this.btnTestOpenGL);
			this.tbAbout.Controls.Add(this.btnClearCache);
			this.tbAbout.Controls.Add(this.debugButton);
			this.tbAbout.Controls.Add(this.pictureBox2);
			this.tbAbout.Controls.Add(this.label6);
			this.tbAbout.Controls.Add(this.label5);
			this.tbAbout.Controls.Add(this.pictureBox1);
			this.tbAbout.Controls.Add(this.linkCdsw);
			this.tbAbout.Controls.Add(this.linkEmail);
			this.tbAbout.Controls.Add(this.lblVersion);
			this.tbAbout.Location = new System.Drawing.Point(4, 22);
			this.tbAbout.Name = "tbAbout";
			this.tbAbout.Size = new System.Drawing.Size(1052, 346);
			this.tbAbout.TabIndex = 4;
			this.tbAbout.Text = "About";
			// 
			// chkOverrideGLCheck
			// 
			this.chkOverrideGLCheck.BackColor = System.Drawing.SystemColors.Control;
			this.chkOverrideGLCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkOverrideGLCheck.Location = new System.Drawing.Point(56, 280);
			this.chkOverrideGLCheck.Name = "chkOverrideGLCheck";
			this.chkOverrideGLCheck.Size = new System.Drawing.Size(152, 17);
			this.chkOverrideGLCheck.TabIndex = 15;
			this.chkOverrideGLCheck.Text = "Override OpenGL Check";
			// 
			// btnTestOpenGL
			// 
			this.btnTestOpenGL.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnTestOpenGL.Location = new System.Drawing.Point(72, 240);
			this.btnTestOpenGL.Name = "btnTestOpenGL";
			this.btnTestOpenGL.Size = new System.Drawing.Size(80, 24);
			this.btnTestOpenGL.TabIndex = 14;
			this.btnTestOpenGL.Text = "Test OpenGL";
			this.btnTestOpenGL.Click += new System.EventHandler(this.btnTestOpenGL_Click);
			// 
			// btnClearCache
			// 
			this.btnClearCache.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnClearCache.Location = new System.Drawing.Point(72, 208);
			this.btnClearCache.Name = "btnClearCache";
			this.btnClearCache.Size = new System.Drawing.Size(80, 24);
			this.btnClearCache.TabIndex = 13;
			this.btnClearCache.Text = "Clear Cache";
			this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
			// 
			// debugButton
			// 
			this.debugButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.debugButton.Location = new System.Drawing.Point(72, 176);
			this.debugButton.Name = "debugButton";
			this.debugButton.Size = new System.Drawing.Size(80, 24);
			this.debugButton.TabIndex = 12;
			this.debugButton.Text = "Debug Info";
			this.debugButton.Click += new System.EventHandler(this.debugButton_Click);
			// 
			// pictureBox2
			// 
			this.pictureBox2.Location = new System.Drawing.Point(72, 136);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(64, 32);
			this.pictureBox2.TabIndex = 11;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(24, 112);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 16);
			this.label6.TabIndex = 10;
			this.label6.Text = "Home:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 96);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 9;
			this.label5.Text = "Contact:";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(8, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(128, 40);
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// linkCdsw
			// 
			this.linkCdsw.Location = new System.Drawing.Point(72, 112);
			this.linkCdsw.Name = "linkCdsw";
			this.linkCdsw.Size = new System.Drawing.Size(152, 16);
			this.linkCdsw.TabIndex = 7;
			this.linkCdsw.TabStop = true;
			this.linkCdsw.Text = "http://cellardoorsw.com/slickr/";
			this.linkCdsw.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCdsw_LinkClicked);
			// 
			// linkEmail
			// 
			this.linkEmail.Location = new System.Drawing.Point(72, 96);
			this.linkEmail.Name = "linkEmail";
			this.linkEmail.Size = new System.Drawing.Size(120, 16);
			this.linkEmail.TabIndex = 6;
			this.linkEmail.TabStop = true;
			this.linkEmail.Text = "gabrielh@gmail.com";
			this.linkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkEmail_LinkClicked);
			// 
			// lblVersion
			// 
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblVersion.Location = new System.Drawing.Point(136, 16);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(88, 40);
			this.lblVersion.TabIndex = 2;
			this.lblVersion.Text = "Version";
			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tpFlickr
			// 
			this.tpFlickr.BackColor = System.Drawing.Color.Transparent;
			this.tpFlickr.Controls.Add(this.groupBox1);
			this.tpFlickr.Location = new System.Drawing.Point(4, 22);
			this.tpFlickr.Name = "tpFlickr";
			this.tpFlickr.Size = new System.Drawing.Size(1052, 346);
			this.tpFlickr.TabIndex = 1;
			this.tpFlickr.Text = "Flickr";
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox1.Controls.Add(this.gbHelp);
			this.groupBox1.Controls.Add(this.gbFlickrGroup);
			this.groupBox1.Controls.Add(this.gbLocal);
			this.groupBox1.Controls.Add(this.rbLocal);
			this.groupBox1.Controls.Add(this.gbFlickrUser);
			this.groupBox1.Controls.Add(this.rbFlickrUser);
			this.groupBox1.Controls.Add(this.rbFlickrEveryone);
			this.groupBox1.Controls.Add(this.tbFlickrUser);
			this.groupBox1.Controls.Add(this.rbFlickrGroup);
			this.groupBox1.Controls.Add(this.gbFlickrEveryone);
			this.groupBox1.Controls.Add(this.btnCheckUser);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.groupBox1.Location = new System.Drawing.Point(6, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.groupBox1.Size = new System.Drawing.Size(1042, 330);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// gbHelp
			// 
			this.gbHelp.Controls.Add(this.lbHelp);
			this.gbHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.gbHelp.Location = new System.Drawing.Point(8, 216);
			this.gbHelp.Name = "gbHelp";
			this.gbHelp.Size = new System.Drawing.Size(280, 104);
			this.gbHelp.TabIndex = 16;
			this.gbHelp.TabStop = false;
			this.gbHelp.Text = "Help";
			// 
			// lbHelp
			// 
			this.lbHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbHelp.Location = new System.Drawing.Point(8, 24);
			this.lbHelp.Name = "lbHelp";
			this.lbHelp.Size = new System.Drawing.Size(248, 40);
			this.lbHelp.TabIndex = 0;
			this.lbHelp.Text = "Help goes here";
			// 
			// gbFlickrGroup
			// 
			this.gbFlickrGroup.Controls.Add(this.ckCheckGroup);
			this.gbFlickrGroup.Controls.Add(this.label9);
			this.gbFlickrGroup.Controls.Add(this.linkLabel1);
			this.gbFlickrGroup.Controls.Add(this.tbFlickrGroup);
			this.gbFlickrGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbFlickrGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.gbFlickrGroup.Location = new System.Drawing.Point(640, 120);
			this.gbFlickrGroup.Name = "gbFlickrGroup";
			this.gbFlickrGroup.Size = new System.Drawing.Size(320, 88);
			this.gbFlickrGroup.TabIndex = 15;
			this.gbFlickrGroup.TabStop = false;
			// 
			// ckCheckGroup
			// 
			this.ckCheckGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ckCheckGroup.Location = new System.Drawing.Point(232, 56);
			this.ckCheckGroup.Name = "ckCheckGroup";
			this.ckCheckGroup.Size = new System.Drawing.Size(72, 24);
			this.ckCheckGroup.TabIndex = 8;
			this.ckCheckGroup.Text = "Check";
			this.ckCheckGroup.Click += new System.EventHandler(this.ckCheckGroup_Click);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 20);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(152, 16);
			this.label9.TabIndex = 3;
			this.label9.Text = "Flickr Group:";
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(8, 36);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(152, 16);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://www.flickr.com/groups/";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// tbFlickrGroup
			// 
			this.tbFlickrGroup.Location = new System.Drawing.Point(160, 32);
			this.tbFlickrGroup.Name = "tbFlickrGroup";
			this.tbFlickrGroup.Size = new System.Drawing.Size(144, 20);
			this.tbFlickrGroup.TabIndex = 1;
			this.tbFlickrGroup.Text = "";
			// 
			// gbLocal
			// 
			this.gbLocal.Controls.Add(this.label8);
			this.gbLocal.Controls.Add(this.tbLocalDir);
			this.gbLocal.Controls.Add(this.localButton);
			this.gbLocal.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbLocal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.gbLocal.Location = new System.Drawing.Point(640, 16);
			this.gbLocal.Name = "gbLocal";
			this.gbLocal.Size = new System.Drawing.Size(312, 96);
			this.gbLocal.TabIndex = 14;
			this.gbLocal.TabStop = false;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 20);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(224, 16);
			this.label8.TabIndex = 10;
			this.label8.Text = "Local Directory:";
			// 
			// tbLocalDir
			// 
			this.tbLocalDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tbLocalDir.Location = new System.Drawing.Point(8, 40);
			this.tbLocalDir.Name = "tbLocalDir";
			this.tbLocalDir.ReadOnly = true;
			this.tbLocalDir.Size = new System.Drawing.Size(224, 20);
			this.tbLocalDir.TabIndex = 8;
			this.tbLocalDir.Text = "";
			// 
			// localButton
			// 
			this.localButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.localButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.localButton.Location = new System.Drawing.Point(240, 40);
			this.localButton.Name = "localButton";
			this.localButton.Size = new System.Drawing.Size(64, 24);
			this.localButton.TabIndex = 9;
			this.localButton.Text = "Select";
			this.localButton.Click += new System.EventHandler(this.localButton_Click);
			// 
			// rbLocal
			// 
			this.rbLocal.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbLocal.Location = new System.Drawing.Point(8, 88);
			this.rbLocal.Name = "rbLocal";
			this.rbLocal.Size = new System.Drawing.Size(56, 16);
			this.rbLocal.TabIndex = 7;
			this.rbLocal.Text = "Local";
			this.rbLocal.CheckedChanged += new System.EventHandler(this.rbLocal_CheckedChanged);
			// 
			// gbFlickrUser
			// 
			this.gbFlickrUser.Controls.Add(this.chkRandomize);
			this.gbFlickrUser.Controls.Add(this.cbFlickrContactType);
			this.gbFlickrUser.Controls.Add(this.cbFlickrUserTagMode);
			this.gbFlickrUser.Controls.Add(this.tbFlickrUserTags);
			this.gbFlickrUser.Controls.Add(this.tbFlickrUserSet);
			this.gbFlickrUser.Controls.Add(this.rbFlickrUserTag);
			this.gbFlickrUser.Controls.Add(this.rbFlickrUserSet);
			this.gbFlickrUser.Controls.Add(this.rbFlickrUserContacts);
			this.gbFlickrUser.Controls.Add(this.rbFlickrUserFav);
			this.gbFlickrUser.Controls.Add(this.rbFlickrUserAll);
			this.gbFlickrUser.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbFlickrUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.gbFlickrUser.Location = new System.Drawing.Point(312, 16);
			this.gbFlickrUser.Name = "gbFlickrUser";
			this.gbFlickrUser.Size = new System.Drawing.Size(320, 96);
			this.gbFlickrUser.TabIndex = 3;
			this.gbFlickrUser.TabStop = false;
			// 
			// chkRandomize
			// 
			this.chkRandomize.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkRandomize.Location = new System.Drawing.Point(80, 68);
			this.chkRandomize.Name = "chkRandomize";
			this.chkRandomize.Size = new System.Drawing.Size(104, 16);
			this.chkRandomize.TabIndex = 10;
			this.chkRandomize.Text = "Randomize";
			// 
			// cbFlickrContactType
			// 
			this.cbFlickrContactType.Items.AddRange(new object[] {
																	 "All",
																	 "Friends",
																	 "Family",
																	 "Both",
																	 "Neither"});
			this.cbFlickrContactType.Location = new System.Drawing.Point(288, 64);
			this.cbFlickrContactType.Name = "cbFlickrContactType";
			this.cbFlickrContactType.Size = new System.Drawing.Size(24, 21);
			this.cbFlickrContactType.TabIndex = 9;
			this.cbFlickrContactType.Text = "All";
			this.cbFlickrContactType.Visible = false;
			// 
			// cbFlickrUserTagMode
			// 
			this.cbFlickrUserTagMode.Items.AddRange(new object[] {
																	 "Any",
																	 "All"});
			this.cbFlickrUserTagMode.Location = new System.Drawing.Point(128, 41);
			this.cbFlickrUserTagMode.Name = "cbFlickrUserTagMode";
			this.cbFlickrUserTagMode.Size = new System.Drawing.Size(48, 21);
			this.cbFlickrUserTagMode.TabIndex = 8;
			this.cbFlickrUserTagMode.Text = "Any";
			this.cbFlickrUserTagMode.Validating += new System.ComponentModel.CancelEventHandler(this.cbFlickrUserTagMode_Validating);
			// 
			// tbFlickrUserTags
			// 
			this.tbFlickrUserTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tbFlickrUserTags.Location = new System.Drawing.Point(184, 41);
			this.tbFlickrUserTags.Name = "tbFlickrUserTags";
			this.tbFlickrUserTags.Size = new System.Drawing.Size(120, 20);
			this.tbFlickrUserTags.TabIndex = 6;
			this.tbFlickrUserTags.Text = "";
			// 
			// tbFlickrUserSet
			// 
			this.tbFlickrUserSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tbFlickrUserSet.Location = new System.Drawing.Point(128, 18);
			this.tbFlickrUserSet.Name = "tbFlickrUserSet";
			this.tbFlickrUserSet.Size = new System.Drawing.Size(176, 20);
			this.tbFlickrUserSet.TabIndex = 5;
			this.tbFlickrUserSet.Text = "";
			// 
			// rbFlickrUserTag
			// 
			this.rbFlickrUserTag.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrUserTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rbFlickrUserTag.Location = new System.Drawing.Point(80, 44);
			this.rbFlickrUserTag.Name = "rbFlickrUserTag";
			this.rbFlickrUserTag.Size = new System.Drawing.Size(49, 17);
			this.rbFlickrUserTag.TabIndex = 4;
			this.rbFlickrUserTag.TabStop = true;
			this.rbFlickrUserTag.Text = "Tags";
			// 
			// rbFlickrUserSet
			// 
			this.rbFlickrUserSet.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrUserSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rbFlickrUserSet.Location = new System.Drawing.Point(80, 20);
			this.rbFlickrUserSet.Name = "rbFlickrUserSet";
			this.rbFlickrUserSet.Size = new System.Drawing.Size(41, 17);
			this.rbFlickrUserSet.TabIndex = 3;
			this.rbFlickrUserSet.TabStop = true;
			this.rbFlickrUserSet.Text = "Set";
			// 
			// rbFlickrUserContacts
			// 
			this.rbFlickrUserContacts.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrUserContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rbFlickrUserContacts.Location = new System.Drawing.Point(8, 68);
			this.rbFlickrUserContacts.Name = "rbFlickrUserContacts";
			this.rbFlickrUserContacts.Size = new System.Drawing.Size(67, 17);
			this.rbFlickrUserContacts.TabIndex = 2;
			this.rbFlickrUserContacts.TabStop = true;
			this.rbFlickrUserContacts.Text = "Contacts";
			// 
			// rbFlickrUserFav
			// 
			this.rbFlickrUserFav.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrUserFav.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rbFlickrUserFav.Location = new System.Drawing.Point(8, 44);
			this.rbFlickrUserFav.Name = "rbFlickrUserFav";
			this.rbFlickrUserFav.Size = new System.Drawing.Size(68, 17);
			this.rbFlickrUserFav.TabIndex = 1;
			this.rbFlickrUserFav.TabStop = true;
			this.rbFlickrUserFav.Text = "Favorites";
			this.rbFlickrUserFav.CheckedChanged += new System.EventHandler(this.rbFlickrUserFav_CheckedChanged);
			// 
			// rbFlickrUserAll
			// 
			this.rbFlickrUserAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrUserAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rbFlickrUserAll.Location = new System.Drawing.Point(8, 20);
			this.rbFlickrUserAll.Name = "rbFlickrUserAll";
			this.rbFlickrUserAll.Size = new System.Drawing.Size(36, 17);
			this.rbFlickrUserAll.TabIndex = 0;
			this.rbFlickrUserAll.TabStop = true;
			this.rbFlickrUserAll.Text = "All";
			this.rbFlickrUserAll.CheckedChanged += new System.EventHandler(this.rbFlickrUserAll_CheckedChanged);
			// 
			// rbFlickrUser
			// 
			this.rbFlickrUser.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrUser.Location = new System.Drawing.Point(8, 16);
			this.rbFlickrUser.Name = "rbFlickrUser";
			this.rbFlickrUser.Size = new System.Drawing.Size(47, 16);
			this.rbFlickrUser.TabIndex = 0;
			this.rbFlickrUser.Text = "User";
			this.rbFlickrUser.CheckedChanged += new System.EventHandler(this.rbFlickrUser_CheckedChanged);
			// 
			// rbFlickrEveryone
			// 
			this.rbFlickrEveryone.Checked = true;
			this.rbFlickrEveryone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrEveryone.Location = new System.Drawing.Point(8, 64);
			this.rbFlickrEveryone.Name = "rbFlickrEveryone";
			this.rbFlickrEveryone.Size = new System.Drawing.Size(70, 16);
			this.rbFlickrEveryone.TabIndex = 2;
			this.rbFlickrEveryone.TabStop = true;
			this.rbFlickrEveryone.Text = "Everyone";
			this.rbFlickrEveryone.CheckedChanged += new System.EventHandler(this.rbFlickrEveryone_CheckedChanged);
			// 
			// tbFlickrUser
			// 
			this.tbFlickrUser.Location = new System.Drawing.Point(56, 14);
			this.tbFlickrUser.Name = "tbFlickrUser";
			this.tbFlickrUser.Size = new System.Drawing.Size(168, 20);
			this.tbFlickrUser.TabIndex = 1;
			this.tbFlickrUser.Text = "";
			// 
			// rbFlickrGroup
			// 
			this.rbFlickrGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrGroup.Location = new System.Drawing.Point(8, 40);
			this.rbFlickrGroup.Name = "rbFlickrGroup";
			this.rbFlickrGroup.Size = new System.Drawing.Size(50, 16);
			this.rbFlickrGroup.TabIndex = 1;
			this.rbFlickrGroup.Text = "Group";
			this.rbFlickrGroup.CheckedChanged += new System.EventHandler(this.rbFlickrGroup_CheckedChanged);
			// 
			// gbFlickrEveryone
			// 
			this.gbFlickrEveryone.Controls.Add(this.chkFlickrInterestEnabled);
			this.gbFlickrEveryone.Controls.Add(this.dtpFlickrEveryoneInterestDate);
			this.gbFlickrEveryone.Controls.Add(this.rbFlickrEveryoneInterest);
			this.gbFlickrEveryone.Controls.Add(this.cbFlickrEveryoneTagMode);
			this.gbFlickrEveryone.Controls.Add(this.tbFlickrEveryoneTags);
			this.gbFlickrEveryone.Controls.Add(this.rbFlickrEveryoneTag);
			this.gbFlickrEveryone.Controls.Add(this.rbFlickrEveryoneRecent);
			this.gbFlickrEveryone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbFlickrEveryone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.gbFlickrEveryone.Location = new System.Drawing.Point(640, 216);
			this.gbFlickrEveryone.Name = "gbFlickrEveryone";
			this.gbFlickrEveryone.Size = new System.Drawing.Size(320, 96);
			this.gbFlickrEveryone.TabIndex = 6;
			this.gbFlickrEveryone.TabStop = false;
			// 
			// chkFlickrInterestEnabled
			// 
			this.chkFlickrInterestEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkFlickrInterestEnabled.Location = new System.Drawing.Point(112, 68);
			this.chkFlickrInterestEnabled.Name = "chkFlickrInterestEnabled";
			this.chkFlickrInterestEnabled.Size = new System.Drawing.Size(48, 16);
			this.chkFlickrInterestEnabled.TabIndex = 13;
			this.chkFlickrInterestEnabled.Text = "Date:";
			this.chkFlickrInterestEnabled.CheckedChanged += new System.EventHandler(this.chkFlickrInterestEnabled_CheckedChanged);
			// 
			// dtpFlickrEveryoneInterestDate
			// 
			this.dtpFlickrEveryoneInterestDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpFlickrEveryoneInterestDate.Location = new System.Drawing.Point(160, 65);
			this.dtpFlickrEveryoneInterestDate.Name = "dtpFlickrEveryoneInterestDate";
			this.dtpFlickrEveryoneInterestDate.Size = new System.Drawing.Size(88, 20);
			this.dtpFlickrEveryoneInterestDate.TabIndex = 12;
			this.dtpFlickrEveryoneInterestDate.Value = new System.DateTime(2006, 1, 23, 0, 0, 0, 0);
			// 
			// rbFlickrEveryoneInterest
			// 
			this.rbFlickrEveryoneInterest.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrEveryoneInterest.Location = new System.Drawing.Point(8, 68);
			this.rbFlickrEveryoneInterest.Name = "rbFlickrEveryoneInterest";
			this.rbFlickrEveryoneInterest.Size = new System.Drawing.Size(96, 16);
			this.rbFlickrEveryoneInterest.TabIndex = 10;
			this.rbFlickrEveryoneInterest.Text = "Interestingness";
			// 
			// cbFlickrEveryoneTagMode
			// 
			this.cbFlickrEveryoneTagMode.Items.AddRange(new object[] {
																		 "Any",
																		 "All"});
			this.cbFlickrEveryoneTagMode.Location = new System.Drawing.Point(56, 41);
			this.cbFlickrEveryoneTagMode.Name = "cbFlickrEveryoneTagMode";
			this.cbFlickrEveryoneTagMode.Size = new System.Drawing.Size(48, 21);
			this.cbFlickrEveryoneTagMode.TabIndex = 9;
			this.cbFlickrEveryoneTagMode.Text = "Any";
			this.cbFlickrEveryoneTagMode.Validating += new System.ComponentModel.CancelEventHandler(this.cbFlickrEveryoneTagMode_Validating);
			// 
			// tbFlickrEveryoneTags
			// 
			this.tbFlickrEveryoneTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tbFlickrEveryoneTags.Location = new System.Drawing.Point(112, 41);
			this.tbFlickrEveryoneTags.Name = "tbFlickrEveryoneTags";
			this.tbFlickrEveryoneTags.Size = new System.Drawing.Size(192, 20);
			this.tbFlickrEveryoneTags.TabIndex = 2;
			this.tbFlickrEveryoneTags.Text = "";
			// 
			// rbFlickrEveryoneTag
			// 
			this.rbFlickrEveryoneTag.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrEveryoneTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rbFlickrEveryoneTag.Location = new System.Drawing.Point(8, 44);
			this.rbFlickrEveryoneTag.Name = "rbFlickrEveryoneTag";
			this.rbFlickrEveryoneTag.Size = new System.Drawing.Size(49, 17);
			this.rbFlickrEveryoneTag.TabIndex = 1;
			this.rbFlickrEveryoneTag.Text = "Tags";
			// 
			// rbFlickrEveryoneRecent
			// 
			this.rbFlickrEveryoneRecent.Checked = true;
			this.rbFlickrEveryoneRecent.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFlickrEveryoneRecent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rbFlickrEveryoneRecent.Location = new System.Drawing.Point(8, 20);
			this.rbFlickrEveryoneRecent.Name = "rbFlickrEveryoneRecent";
			this.rbFlickrEveryoneRecent.Size = new System.Drawing.Size(60, 17);
			this.rbFlickrEveryoneRecent.TabIndex = 0;
			this.rbFlickrEveryoneRecent.TabStop = true;
			this.rbFlickrEveryoneRecent.Text = "Recent";
			// 
			// btnCheckUser
			// 
			this.btnCheckUser.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCheckUser.Location = new System.Drawing.Point(224, 12);
			this.btnCheckUser.Name = "btnCheckUser";
			this.btnCheckUser.Size = new System.Drawing.Size(72, 24);
			this.btnCheckUser.TabIndex = 7;
			this.btnCheckUser.Text = "Check";
			this.btnCheckUser.Click += new System.EventHandler(this.ckCheckUser_Click);
			// 
			// tbOptions
			// 
			this.tbOptions.BackColor = System.Drawing.Color.Transparent;
			this.tbOptions.Controls.Add(this.chkIncludeUserInContacts);
			this.tbOptions.Controls.Add(this.ckShowLogo);
			this.tbOptions.Controls.Add(this.ckFadeEnabled);
			this.tbOptions.Controls.Add(this.ckPanEnabled);
			this.tbOptions.Controls.Add(this.ckZoomEnabled);
			this.tbOptions.Controls.Add(this.label12);
			this.tbOptions.Controls.Add(this.cbMaxSizeLabel);
			this.tbOptions.Controls.Add(this.tbMinSize);
			this.tbOptions.Controls.Add(this.baseDirButton);
			this.tbOptions.Controls.Add(this.tbBaseDir);
			this.tbOptions.Controls.Add(this.lbPowerTimeout);
			this.tbOptions.Controls.Add(this.ckPowerSetting);
			this.tbOptions.Controls.Add(this.tbMaxCacheSize);
			this.tbOptions.Controls.Add(this.label30);
			this.tbOptions.Controls.Add(this.ckShowStatus);
			this.tbOptions.Controls.Add(this.ckShowFile);
			this.tbOptions.Controls.Add(this.label11);
			this.tbOptions.Controls.Add(this.lbView);
			this.tbOptions.Controls.Add(this.tbView);
			this.tbOptions.Controls.Add(this.label1);
			this.tbOptions.Controls.Add(this.label4);
			this.tbOptions.Location = new System.Drawing.Point(4, 22);
			this.tbOptions.Name = "tbOptions";
			this.tbOptions.Size = new System.Drawing.Size(1052, 346);
			this.tbOptions.TabIndex = 0;
			this.tbOptions.Text = "Options";
			// 
			// chkIncludeUserInContacts
			// 
			this.chkIncludeUserInContacts.BackColor = System.Drawing.SystemColors.Control;
			this.chkIncludeUserInContacts.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkIncludeUserInContacts.Location = new System.Drawing.Point(8, 240);
			this.chkIncludeUserInContacts.Name = "chkIncludeUserInContacts";
			this.chkIncludeUserInContacts.Size = new System.Drawing.Size(160, 16);
			this.chkIncludeUserInContacts.TabIndex = 32;
			this.chkIncludeUserInContacts.Text = "Include user with contacts";
			// 
			// ckShowLogo
			// 
			this.ckShowLogo.BackColor = System.Drawing.SystemColors.Control;
			this.ckShowLogo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ckShowLogo.Location = new System.Drawing.Point(160, 216);
			this.ckShowLogo.Name = "ckShowLogo";
			this.ckShowLogo.Size = new System.Drawing.Size(96, 16);
			this.ckShowLogo.TabIndex = 31;
			this.ckShowLogo.Text = "Show logo";
			// 
			// ckFadeEnabled
			// 
			this.ckFadeEnabled.BackColor = System.Drawing.SystemColors.Control;
			this.ckFadeEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ckFadeEnabled.Location = new System.Drawing.Point(160, 192);
			this.ckFadeEnabled.Name = "ckFadeEnabled";
			this.ckFadeEnabled.Size = new System.Drawing.Size(96, 16);
			this.ckFadeEnabled.TabIndex = 30;
			this.ckFadeEnabled.Text = "Fade enabled";
			// 
			// ckPanEnabled
			// 
			this.ckPanEnabled.BackColor = System.Drawing.SystemColors.Control;
			this.ckPanEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ckPanEnabled.Location = new System.Drawing.Point(8, 216);
			this.ckPanEnabled.Name = "ckPanEnabled";
			this.ckPanEnabled.Size = new System.Drawing.Size(88, 16);
			this.ckPanEnabled.TabIndex = 29;
			this.ckPanEnabled.Text = "Pan enabled";
			// 
			// ckZoomEnabled
			// 
			this.ckZoomEnabled.BackColor = System.Drawing.SystemColors.Control;
			this.ckZoomEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ckZoomEnabled.Location = new System.Drawing.Point(8, 192);
			this.ckZoomEnabled.Name = "ckZoomEnabled";
			this.ckZoomEnabled.Size = new System.Drawing.Size(96, 16);
			this.ckZoomEnabled.TabIndex = 28;
			this.ckZoomEnabled.Text = "Zoom enabled";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 120);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(144, 16);
			this.label12.TabIndex = 27;
			this.label12.Text = "Maximum size:";
			// 
			// cbMaxSizeLabel
			// 
			this.cbMaxSizeLabel.Items.AddRange(new object[] {
																"Original",
																"Large",
																"Medium"});
			this.cbMaxSizeLabel.Location = new System.Drawing.Point(160, 117);
			this.cbMaxSizeLabel.Name = "cbMaxSizeLabel";
			this.cbMaxSizeLabel.Size = new System.Drawing.Size(64, 21);
			this.cbMaxSizeLabel.TabIndex = 26;
			// 
			// tbMinSize
			// 
			this.tbMinSize.Location = new System.Drawing.Point(160, 93);
			this.tbMinSize.Name = "tbMinSize";
			this.tbMinSize.Size = new System.Drawing.Size(64, 20);
			this.tbMinSize.TabIndex = 25;
			this.tbMinSize.Text = "600,400";
			// 
			// baseDirButton
			// 
			this.baseDirButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.baseDirButton.Location = new System.Drawing.Point(264, 285);
			this.baseDirButton.Name = "baseDirButton";
			this.baseDirButton.Size = new System.Drawing.Size(68, 25);
			this.baseDirButton.TabIndex = 23;
			this.baseDirButton.Text = "Select";
			this.baseDirButton.Click += new System.EventHandler(this.baseDirButton_Click);
			// 
			// tbBaseDir
			// 
			this.tbBaseDir.Location = new System.Drawing.Point(8, 288);
			this.tbBaseDir.Name = "tbBaseDir";
			this.tbBaseDir.ReadOnly = true;
			this.tbBaseDir.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.tbBaseDir.Size = new System.Drawing.Size(256, 20);
			this.tbBaseDir.TabIndex = 22;
			this.tbBaseDir.Text = "";
			// 
			// lbPowerTimeout
			// 
			this.lbPowerTimeout.AutoSize = true;
			this.lbPowerTimeout.Location = new System.Drawing.Point(184, 146);
			this.lbPowerTimeout.Name = "lbPowerTimeout";
			this.lbPowerTimeout.Size = new System.Drawing.Size(67, 16);
			this.lbPowerTimeout.TabIndex = 21;
			this.lbPowerTimeout.Text = "(20 minutes)";
			// 
			// ckPowerSetting
			// 
			this.ckPowerSetting.BackColor = System.Drawing.SystemColors.Control;
			this.ckPowerSetting.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ckPowerSetting.Location = new System.Drawing.Point(8, 144);
			this.ckPowerSetting.Name = "ckPowerSetting";
			this.ckPowerSetting.Size = new System.Drawing.Size(191, 17);
			this.ckPowerSetting.TabIndex = 20;
			this.ckPowerSetting.Text = "Sleep after monitor power saving";
			// 
			// tbMaxCacheSize
			// 
			this.tbMaxCacheSize.Location = new System.Drawing.Point(142, 309);
			this.tbMaxCacheSize.Name = "tbMaxCacheSize";
			this.tbMaxCacheSize.Size = new System.Drawing.Size(50, 20);
			this.tbMaxCacheSize.TabIndex = 19;
			this.tbMaxCacheSize.Text = "200";
			this.tbMaxCacheSize.Validating += new System.ComponentModel.CancelEventHandler(this.cbMaxCacheSize_Validating);
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Location = new System.Drawing.Point(3, 312);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(140, 16);
			this.label30.TabIndex = 18;
			this.label30.Text = "Maximum cache size (MB):";
			// 
			// ckShowStatus
			// 
			this.ckShowStatus.BackColor = System.Drawing.SystemColors.Control;
			this.ckShowStatus.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ckShowStatus.Location = new System.Drawing.Point(8, 168);
			this.ckShowStatus.Name = "ckShowStatus";
			this.ckShowStatus.Size = new System.Drawing.Size(88, 17);
			this.ckShowStatus.TabIndex = 12;
			this.ckShowStatus.Text = "Show status";
			// 
			// ckShowFile
			// 
			this.ckShowFile.BackColor = System.Drawing.SystemColors.Control;
			this.ckShowFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ckShowFile.Location = new System.Drawing.Point(160, 168);
			this.ckShowFile.Name = "ckShowFile";
			this.ckShowFile.Size = new System.Drawing.Size(96, 17);
			this.ckShowFile.TabIndex = 11;
			this.ckShowFile.Text = "Show file info";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(8, 96);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(150, 16);
			this.label11.TabIndex = 9;
			this.label11.Text = "Minimum Size (width,height):";
			// 
			// lbView
			// 
			this.lbView.BackColor = System.Drawing.Color.Transparent;
			this.lbView.Location = new System.Drawing.Point(63, 60);
			this.lbView.Name = "lbView";
			this.lbView.Size = new System.Drawing.Size(214, 17);
			this.lbView.TabIndex = 7;
			this.lbView.Text = "10 seconds";
			this.lbView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tbView
			// 
			this.tbView.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.tbView.LargeChange = 30;
			this.tbView.Location = new System.Drawing.Point(8, 32);
			this.tbView.Maximum = 284;
			this.tbView.Minimum = 15;
			this.tbView.Name = "tbView";
			this.tbView.Size = new System.Drawing.Size(320, 45);
			this.tbView.TabIndex = 0;
			this.tbView.TickFrequency = 15;
			this.tbView.Value = 15;
			this.tbView.ValueChanged += new System.EventHandler(this.tbView_ValueChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(51, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(216, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "How often should pictures change?";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 272);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(87, 16);
			this.label4.TabIndex = 24;
			this.label4.Text = "Cache directory:";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tpFlickr);
			this.tabControl1.Controls.Add(this.tbOptions);
			this.tabControl1.Controls.Add(this.tbKey);
			this.tabControl1.Controls.Add(this.tbAuthorize);
			this.tabControl1.Controls.Add(this.tbConnect);
			this.tabControl1.Controls.Add(this.tbAbout);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1060, 372);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tbKey
			// 
			this.tbKey.Controls.Add(this.label37);
			this.tbKey.Controls.Add(this.label33);
			this.tbKey.Controls.Add(this.tbxSecret);
			this.tbKey.Controls.Add(this.tbxKey);
			this.tbKey.Controls.Add(this.linkLabel2);
			this.tbKey.Controls.Add(this.label2);
			this.tbKey.Controls.Add(this.label3);
			this.tbKey.Location = new System.Drawing.Point(4, 22);
			this.tbKey.Name = "tbKey";
			this.tbKey.Size = new System.Drawing.Size(1052, 346);
			this.tbKey.TabIndex = 7;
			this.tbKey.Text = "Key";
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(8, 128);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(56, 16);
			this.label37.TabIndex = 11;
			this.label37.Text = "Secret:";
			this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(24, 96);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(40, 16);
			this.label33.TabIndex = 10;
			this.label33.Text = "Key:";
			this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbxSecret
			// 
			this.tbxSecret.Location = new System.Drawing.Point(64, 128);
			this.tbxSecret.Name = "tbxSecret";
			this.tbxSecret.Size = new System.Drawing.Size(136, 20);
			this.tbxSecret.TabIndex = 9;
			this.tbxSecret.Text = "";
			// 
			// tbxKey
			// 
			this.tbxKey.Location = new System.Drawing.Point(64, 96);
			this.tbxKey.Name = "tbxKey";
			this.tbxKey.Size = new System.Drawing.Size(224, 20);
			this.tbxKey.TabIndex = 8;
			this.tbxKey.Text = "";
			// 
			// linkLabel2
			// 
			this.linkLabel2.Location = new System.Drawing.Point(24, 64);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(200, 16);
			this.linkLabel2.TabIndex = 7;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "http://www.flickr.com/services/api/keys";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(328, 32);
			this.label2.TabIndex = 3;
			this.label2.Text = "To use Slickr you must have an application key.";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "To obtain a key go to:";
			// 
			// tbAuthorize
			// 
			this.tbAuthorize.Controls.Add(this.authCopyButton);
			this.tbAuthorize.Controls.Add(this.tbAuthStatus);
			this.tbAuthorize.Controls.Add(this.lbAuthStatus);
			this.tbAuthorize.Controls.Add(this.removeAuthButton);
			this.tbAuthorize.Controls.Add(this.completeAuthButton);
			this.tbAuthorize.Controls.Add(this.authButton);
			this.tbAuthorize.Controls.Add(this.label13);
			this.tbAuthorize.Controls.Add(this.label10);
			this.tbAuthorize.Location = new System.Drawing.Point(4, 22);
			this.tbAuthorize.Name = "tbAuthorize";
			this.tbAuthorize.Size = new System.Drawing.Size(1052, 346);
			this.tbAuthorize.TabIndex = 5;
			this.tbAuthorize.Text = "Authorize";
			// 
			// authCopyButton
			// 
			this.authCopyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.authCopyButton.Location = new System.Drawing.Point(288, 230);
			this.authCopyButton.Name = "authCopyButton";
			this.authCopyButton.Size = new System.Drawing.Size(48, 24);
			this.authCopyButton.TabIndex = 7;
			this.authCopyButton.Text = "Copy";
			this.authCopyButton.Visible = false;
			this.authCopyButton.Click += new System.EventHandler(this.authCopyButton_Click);
			// 
			// tbAuthStatus
			// 
			this.tbAuthStatus.Location = new System.Drawing.Point(8, 232);
			this.tbAuthStatus.Name = "tbAuthStatus";
			this.tbAuthStatus.Size = new System.Drawing.Size(272, 20);
			this.tbAuthStatus.TabIndex = 6;
			this.tbAuthStatus.Text = "";
			this.tbAuthStatus.Visible = false;
			// 
			// lbAuthStatus
			// 
			this.lbAuthStatus.Location = new System.Drawing.Point(8, 96);
			this.lbAuthStatus.Name = "lbAuthStatus";
			this.lbAuthStatus.Size = new System.Drawing.Size(304, 16);
			this.lbAuthStatus.TabIndex = 5;
			this.lbAuthStatus.Text = "Status:";
			// 
			// removeAuthButton
			// 
			this.removeAuthButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.removeAuthButton.Location = new System.Drawing.Point(32, 184);
			this.removeAuthButton.Name = "removeAuthButton";
			this.removeAuthButton.Size = new System.Drawing.Size(144, 24);
			this.removeAuthButton.TabIndex = 4;
			this.removeAuthButton.Text = "Remove Authorization";
			this.removeAuthButton.Click += new System.EventHandler(this.removeAuthButton_Click);
			// 
			// completeAuthButton
			// 
			this.completeAuthButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.completeAuthButton.Location = new System.Drawing.Point(32, 152);
			this.completeAuthButton.Name = "completeAuthButton";
			this.completeAuthButton.Size = new System.Drawing.Size(144, 24);
			this.completeAuthButton.TabIndex = 3;
			this.completeAuthButton.Text = "Complete Authorization";
			this.completeAuthButton.Click += new System.EventHandler(this.completeAuthButton_Click);
			// 
			// authButton
			// 
			this.authButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.authButton.Location = new System.Drawing.Point(32, 120);
			this.authButton.Name = "authButton";
			this.authButton.Size = new System.Drawing.Size(144, 24);
			this.authButton.TabIndex = 2;
			this.authButton.Text = "Authorize";
			this.authButton.Click += new System.EventHandler(this.authButton_Click);
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label13.Location = new System.Drawing.Point(8, 16);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(328, 32);
			this.label13.TabIndex = 1;
			this.label13.Text = "Authorizing is required to view private photos and groups.";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 64);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(328, 40);
			this.label10.TabIndex = 0;
			this.label10.Text = "You will be asked to authorize Slickr for read access. When you are finished retu" +
				"rn to this window to complete authorization.";
			// 
			// tbConnect
			// 
			this.tbConnect.Controls.Add(this.label32);
			this.tbConnect.Controls.Add(this.tbProxyPort);
			this.tbConnect.Controls.Add(this.label31);
			this.tbConnect.Controls.Add(this.label7);
			this.tbConnect.Controls.Add(this.tbProxyIpAddress);
			this.tbConnect.Controls.Add(this.chkProxy);
			this.tbConnect.Controls.Add(this.tbProxyPassword);
			this.tbConnect.Controls.Add(this.tbProxyDomain);
			this.tbConnect.Controls.Add(this.label34);
			this.tbConnect.Controls.Add(this.label35);
			this.tbConnect.Controls.Add(this.label36);
			this.tbConnect.Controls.Add(this.tbProxyUsername);
			this.tbConnect.Location = new System.Drawing.Point(4, 22);
			this.tbConnect.Name = "tbConnect";
			this.tbConnect.Size = new System.Drawing.Size(1052, 346);
			this.tbConnect.TabIndex = 6;
			this.tbConnect.Text = "Connection";
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(32, 192);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(200, 24);
			this.label32.TabIndex = 13;
			this.label32.Text = "If not set, the default IE proxy is used.";
			// 
			// tbProxyPort
			// 
			this.tbProxyPort.Location = new System.Drawing.Point(248, 53);
			this.tbProxyPort.Name = "tbProxyPort";
			this.tbProxyPort.Size = new System.Drawing.Size(56, 20);
			this.tbProxyPort.TabIndex = 2;
			this.tbProxyPort.Text = "";
			// 
			// label31
			// 
			this.label31.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label31.Location = new System.Drawing.Point(208, 56);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(32, 16);
			this.label31.TabIndex = 11;
			this.label31.Text = "Port:";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label7.Location = new System.Drawing.Point(32, 152);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(40, 16);
			this.label7.TabIndex = 10;
			this.label7.Text = "Domain:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbProxyIpAddress
			// 
			this.tbProxyIpAddress.Location = new System.Drawing.Point(80, 53);
			this.tbProxyIpAddress.Name = "tbProxyIpAddress";
			this.tbProxyIpAddress.Size = new System.Drawing.Size(120, 20);
			this.tbProxyIpAddress.TabIndex = 1;
			this.tbProxyIpAddress.Text = "";
			// 
			// chkProxy
			// 
			this.chkProxy.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkProxy.Location = new System.Drawing.Point(16, 16);
			this.chkProxy.Name = "chkProxy";
			this.chkProxy.Size = new System.Drawing.Size(88, 24);
			this.chkProxy.TabIndex = 0;
			this.chkProxy.Text = "Define proxy";
			this.chkProxy.CheckedChanged += new System.EventHandler(this.chkProxy_CheckedChanged);
			// 
			// tbProxyPassword
			// 
			this.tbProxyPassword.Location = new System.Drawing.Point(80, 125);
			this.tbProxyPassword.Name = "tbProxyPassword";
			this.tbProxyPassword.PasswordChar = '*';
			this.tbProxyPassword.Size = new System.Drawing.Size(120, 20);
			this.tbProxyPassword.TabIndex = 4;
			this.tbProxyPassword.Text = "";
			// 
			// tbProxyDomain
			// 
			this.tbProxyDomain.Location = new System.Drawing.Point(80, 149);
			this.tbProxyDomain.Name = "tbProxyDomain";
			this.tbProxyDomain.Size = new System.Drawing.Size(120, 20);
			this.tbProxyDomain.TabIndex = 5;
			this.tbProxyDomain.Text = "";
			// 
			// label34
			// 
			this.label34.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label34.Location = new System.Drawing.Point(16, 128);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(56, 16);
			this.label34.TabIndex = 5;
			this.label34.Text = "Password:";
			this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label35
			// 
			this.label35.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label35.Location = new System.Drawing.Point(8, 56);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(64, 16);
			this.label35.TabIndex = 1;
			this.label35.Text = "IP Address:";
			this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label36
			// 
			this.label36.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label36.Location = new System.Drawing.Point(16, 104);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(56, 16);
			this.label36.TabIndex = 3;
			this.label36.Text = "Username:";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbProxyUsername
			// 
			this.tbProxyUsername.Location = new System.Drawing.Point(80, 101);
			this.tbProxyUsername.Name = "tbProxyUsername";
			this.tbProxyUsername.Size = new System.Drawing.Size(120, 20);
			this.tbProxyUsername.TabIndex = 3;
			this.tbProxyUsername.Text = "";
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label14.Location = new System.Drawing.Point(15, 224);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(206, 21);
			this.label14.TabIndex = 16;
			this.label14.Text = "How many images per search:";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(63, 153);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(214, 17);
			this.label15.TabIndex = 15;
			this.label15.Text = "10 seconds";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trackBar1
			// 
			this.trackBar1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(244)), ((System.Byte)(243)), ((System.Byte)(238)));
			this.trackBar1.Location = new System.Drawing.Point(18, 125);
			this.trackBar1.Maximum = 300;
			this.trackBar1.Minimum = 1;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(306, 45);
			this.trackBar1.TabIndex = 14;
			this.trackBar1.TickFrequency = 10;
			this.trackBar1.Value = 5;
			// 
			// label16
			// 
			this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label16.Location = new System.Drawing.Point(61, 106);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(216, 16);
			this.label16.TabIndex = 13;
			this.label16.Text = "How long to wait in between searches?";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(20, 312);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(84, 17);
			this.checkBox1.TabIndex = 12;
			this.checkBox1.Text = "Show status";
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(20, 287);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(89, 17);
			this.checkBox2.TabIndex = 11;
			this.checkBox2.Text = "Show file info";
			// 
			// comboBox1
			// 
			this.comboBox1.Items.AddRange(new object[] {
														   "0,0",
														   "400,300",
														   "640,400",
														   "640,480",
														   "800,600",
														   "1024,768",
														   "1280,1024"});
			this.comboBox1.Location = new System.Drawing.Point(227, 258);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(74, 21);
			this.comboBox1.TabIndex = 10;
			this.comboBox1.Text = "400,300";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(17, 261);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(218, 16);
			this.label17.TabIndex = 9;
			this.label17.Text = "Ignore images smaller than (width, height):";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(63, 60);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(214, 17);
			this.label18.TabIndex = 7;
			this.label18.Text = "10 seconds";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trackBar2
			// 
			this.trackBar2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(251)), ((System.Byte)(251)), ((System.Byte)(252)));
			this.trackBar2.LargeChange = 30;
			this.trackBar2.Location = new System.Drawing.Point(54, 32);
			this.trackBar2.Maximum = 125;
			this.trackBar2.Minimum = 5;
			this.trackBar2.Name = "trackBar2";
			this.trackBar2.Size = new System.Drawing.Size(232, 45);
			this.trackBar2.TabIndex = 0;
			this.trackBar2.TickFrequency = 15;
			this.trackBar2.Value = 10;
			// 
			// label19
			// 
			this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label19.Location = new System.Drawing.Point(292, 32);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(34, 16);
			this.label19.TabIndex = 6;
			this.label19.Text = "Less";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label20
			// 
			this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label20.Location = new System.Drawing.Point(51, 13);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(216, 16);
			this.label20.TabIndex = 4;
			this.label20.Text = "How often should pictures change?";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label21
			// 
			this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label21.Location = new System.Drawing.Point(17, 32);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(31, 16);
			this.label21.TabIndex = 5;
			this.label21.Text = "More";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label22
			// 
			this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label22.Location = new System.Drawing.Point(15, 224);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(206, 21);
			this.label22.TabIndex = 16;
			this.label22.Text = "How many images per search:";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(63, 153);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(214, 17);
			this.label23.TabIndex = 15;
			this.label23.Text = "10 seconds";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trackBar3
			// 
			this.trackBar3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(244)), ((System.Byte)(243)), ((System.Byte)(238)));
			this.trackBar3.Location = new System.Drawing.Point(18, 125);
			this.trackBar3.Maximum = 300;
			this.trackBar3.Minimum = 1;
			this.trackBar3.Name = "trackBar3";
			this.trackBar3.Size = new System.Drawing.Size(306, 45);
			this.trackBar3.TabIndex = 14;
			this.trackBar3.TickFrequency = 10;
			this.trackBar3.Value = 5;
			// 
			// label24
			// 
			this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label24.Location = new System.Drawing.Point(61, 106);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(216, 16);
			this.label24.TabIndex = 13;
			this.label24.Text = "How long to wait in between searches?";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// checkBox3
			// 
			this.checkBox3.Location = new System.Drawing.Point(20, 312);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(84, 17);
			this.checkBox3.TabIndex = 12;
			this.checkBox3.Text = "Show status";
			// 
			// checkBox4
			// 
			this.checkBox4.Location = new System.Drawing.Point(20, 287);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(89, 17);
			this.checkBox4.TabIndex = 11;
			this.checkBox4.Text = "Show file info";
			// 
			// comboBox2
			// 
			this.comboBox2.Items.AddRange(new object[] {
														   "0,0",
														   "400,300",
														   "640,400",
														   "640,480",
														   "800,600",
														   "1024,768",
														   "1280,1024"});
			this.comboBox2.Location = new System.Drawing.Point(227, 258);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(74, 21);
			this.comboBox2.TabIndex = 10;
			this.comboBox2.Text = "400,300";
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(17, 261);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(218, 16);
			this.label25.TabIndex = 9;
			this.label25.Text = "Ignore images smaller than (width, height):";
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(63, 60);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(214, 17);
			this.label26.TabIndex = 7;
			this.label26.Text = "10 seconds";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trackBar4
			// 
			this.trackBar4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(251)), ((System.Byte)(251)), ((System.Byte)(252)));
			this.trackBar4.LargeChange = 30;
			this.trackBar4.Location = new System.Drawing.Point(54, 32);
			this.trackBar4.Maximum = 125;
			this.trackBar4.Minimum = 5;
			this.trackBar4.Name = "trackBar4";
			this.trackBar4.Size = new System.Drawing.Size(232, 45);
			this.trackBar4.TabIndex = 0;
			this.trackBar4.TickFrequency = 15;
			this.trackBar4.Value = 10;
			// 
			// label27
			// 
			this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label27.Location = new System.Drawing.Point(292, 32);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(34, 16);
			this.label27.TabIndex = 6;
			this.label27.Text = "Less";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label28
			// 
			this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label28.Location = new System.Drawing.Point(51, 13);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(216, 16);
			this.label28.TabIndex = 4;
			this.label28.Text = "How often should pictures change?";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label29
			// 
			this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label29.Location = new System.Drawing.Point(17, 32);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(31, 16);
			this.label29.TabIndex = 5;
			this.label29.Text = "More";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ConfigurationForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1080, 422);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Name = "ConfigurationForm";
			this.Text = "Slickr";
			this.Load += new System.EventHandler(this.ConfigurationForm_Load);
			this.tbAbout.ResumeLayout(false);
			this.tpFlickr.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.gbHelp.ResumeLayout(false);
			this.gbFlickrGroup.ResumeLayout(false);
			this.gbLocal.ResumeLayout(false);
			this.gbFlickrUser.ResumeLayout(false);
			this.gbFlickrEveryone.ResumeLayout(false);
			this.tbOptions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbView)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tbKey.ResumeLayout(false);
			this.tbAuthorize.ResumeLayout(false);
			this.tbConnect.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
			this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbFlickrEveryone;
        private System.Windows.Forms.TextBox tbFlickrEveryoneTags;
        private System.Windows.Forms.RadioButton rbFlickrEveryoneTag;
        private System.Windows.Forms.RadioButton rbFlickrEveryoneRecent;
        private System.Windows.Forms.GroupBox gbFlickrUser;
        private System.Windows.Forms.TextBox tbFlickrUserTags;
        private System.Windows.Forms.TextBox tbFlickrUserSet;
        private System.Windows.Forms.RadioButton rbFlickrUserTag;
        private System.Windows.Forms.RadioButton rbFlickrUserSet;
        private System.Windows.Forms.RadioButton rbFlickrUserContacts;
        private System.Windows.Forms.RadioButton rbFlickrUserFav;
        private System.Windows.Forms.RadioButton rbFlickrUserAll;
        private System.Windows.Forms.RadioButton rbFlickrUser;
        private System.Windows.Forms.RadioButton rbFlickrEveryone;
        private System.Windows.Forms.TextBox tbFlickrUser;
        private System.Windows.Forms.RadioButton rbFlickrGroup;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbView;
        private System.Windows.Forms.TrackBar tbView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckShowStatus;
        private System.Windows.Forms.CheckBox ckShowFile;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TrackBar trackBar4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label lbPowerTimeout;
        private System.Windows.Forms.CheckBox ckPowerSetting;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button baseDirButton;
        private System.Windows.Forms.TextBox tbBaseDir;
        private System.Windows.Forms.FolderBrowserDialog baseDirBrowserDialog;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button authButton;
        private System.Windows.Forms.Button completeAuthButton;
        private System.Windows.Forms.Button removeAuthButton;
        private System.Windows.Forms.TabPage tbAuthorize;
        private System.Windows.Forms.Label lbAuthStatus;
        private System.Windows.Forms.LinkLabel linkEmail;
        private System.Windows.Forms.LinkLabel linkCdsw;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.RadioButton rbLocal;
        private System.Windows.Forms.TextBox tbLocalDir;
        private System.Windows.Forms.Button localButton;
        private System.Windows.Forms.FolderBrowserDialog localDirBrowserDialog;     
        
        private System.Windows.Forms.GroupBox gbLocal;
        private System.Windows.Forms.GroupBox gbFlickrGroup;        
        private System.Windows.Forms.TextBox tbFlickrGroup;
        private System.Windows.Forms.GroupBox gbHelp;
        private System.Windows.Forms.Label lbHelp;
        private System.Windows.Forms.TabPage tpFlickr;
        private System.Windows.Forms.TabPage tbAbout;
        private System.Windows.Forms.TabPage tbOptions;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label9;      
        private System.Windows.Forms.TextBox tbMaxCacheSize;
        private System.Windows.Forms.TextBox tbMinSize;
        private System.Windows.Forms.ComboBox cbMaxSizeLabel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox ckZoomEnabled;
        private System.Windows.Forms.CheckBox ckPanEnabled;
        private System.Windows.Forms.Button ckCheckGroup;
        private System.Windows.Forms.ComboBox cbFlickrUserTagMode;
        private System.Windows.Forms.ComboBox cbFlickrEveryoneTagMode;
        private System.Windows.Forms.CheckBox ckFadeEnabled;

        // My stuff
        private ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox tbAuthStatus;
        private System.Windows.Forms.CheckBox ckShowLogo;
        private System.Windows.Forms.RadioButton rbFlickrEveryoneInterest;
        private System.Windows.Forms.DateTimePicker dtpFlickrEveryoneInterestDate;
        private System.Windows.Forms.CheckBox chkFlickrInterestEnabled;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btnCheckUser;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button debugButton;
        private System.Windows.Forms.Button authCopyButton;
        private System.Windows.Forms.CheckBox chkRandomize;
        private System.Windows.Forms.ComboBox cbFlickrContactType;
        private System.Windows.Forms.CheckBox chkIncludeUserInContacts;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.TabPage tbConnect;
		private System.Windows.Forms.CheckBox chkProxy;
		private System.Windows.Forms.TextBox tbProxyPassword;
		private System.Windows.Forms.TextBox tbProxyDomain;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.TextBox tbProxyUsername;
		private System.Windows.Forms.TextBox tbProxyIpAddress;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox tbProxyPort;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Button btnTestOpenGL;
		private System.Windows.Forms.TabPage tbKey;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.Windows.Forms.TextBox tbxKey;
		private System.Windows.Forms.TextBox tbxSecret;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.CheckBox chkOverrideGLCheck;                 

        private bool authorizing = false;

        public ConfigurationForm()
        {
            Application.EnableVisualStyles();
            InitializeComponent();

			Icon = Utils.LoadResourceIcon("App.ico");
			pictureBox1.Image = Utils.LoadResourceBitmap("slickr.png");

			if (!Settings.Registered)
				pictureBox2.Image = Utils.LoadResourceBitmap("donate.gif");

            errorProvider = new ErrorProvider(this);    
            Size = new Size(387, 456);  
            tabControl1.Size = new Size(353, 372);
            groupBox1.Size = new Size(333, 330);
        }       

        public void LoadSettings()
        {
            lblVersion.Text = Settings.Version;

            //tbFlickrWait.Maximum = Settings.MAX_QUERY_WAIT;

			int tickerVal = tbView.Maximum;
			if (Settings.Local.SecondsView > 238) 
			{
				tickerVal = (Settings.Local.SecondsView / 3600) + 236;
			}
			else if (Settings.Local.SecondsView > 120) 
			{
				tickerVal = (Settings.Local.SecondsView / 60) + 118;
			}
			else
			{
				tickerVal = Settings.Local.SecondsView;			
			}

			if (tickerVal > tbView.Maximum) tickerVal = tbView.Maximum;
			else if (tickerVal < tbView.Minimum) tickerVal = tbView.Minimum;
			
			tbView.Value = tickerVal;

			tbView_ValueChanged(null, null);

            /**
            switch(Settings.Local.SearchType) {
                case Settings.SearchType.FLICKR: rbModeFlickr.Checked = true; break;
                case Settings.SearchType.SEARCH: rbModeSearch.Checked = true; break;
            }
             */
            
            tbMinSize.Text = Settings.Local.MinWidth + "," + Settings.Local.MinHeight;
            cbMaxSizeLabel.Text = Settings.Local.MaxSizeLabel;

            switch (Settings.Local.FlickrMode)
            {
                case Settings.FlickrMode.GROUP: rbFlickrGroup.Checked = true; break;
                case Settings.FlickrMode.USER: rbFlickrUser.Checked = true; break;
                case Settings.FlickrMode.EVERYONE: rbFlickrEveryone.Checked = true; break;                
                case Settings.FlickrMode.LOCAL: rbLocal.Checked = true; break;     
            }

            switch (Settings.Local.FlickrUserMode)
            {
                case Settings.FlickrUserMode.ALL: rbFlickrUserAll.Checked = true; break;
                case Settings.FlickrUserMode.FAVORITES: rbFlickrUserFav.Checked = true; break;
                case Settings.FlickrUserMode.CONTACTS: rbFlickrUserContacts.Checked = true; break;
                case Settings.FlickrUserMode.SET: rbFlickrUserSet.Checked = true; break;
                case Settings.FlickrUserMode.TAG: rbFlickrUserTag.Checked = true; break;                
            }

            tbFlickrUser.Text = Settings.Local.FlickrUser;
            tbFlickrUserSet.Text = Settings.Local.FlickrUserSet;
            tbFlickrUserTags.Text = Settings.Local.FlickrUserTag;
            cbFlickrUserTagMode.Text = Settings.Local.FlickrUserTagMode;

            tbFlickrGroup.Text = Settings.Local.FlickrGroup;

            tbFlickrEveryoneTags.Text = Settings.Local.FlickrEveryoneTag;
            cbFlickrEveryoneTagMode.Text = Settings.Local.FlickrEveryoneTagMode;

            dtpFlickrEveryoneInterestDate.MaxDate = DateTime.Today;
            dtpFlickrEveryoneInterestDate.Format = DateTimePickerFormat.Short;

            if (Settings.Local.FlickrEveryoneInterestDate.Equals(DateTime.MinValue)) 
            {
                dtpFlickrEveryoneInterestDate.Enabled = false;
                dtpFlickrEveryoneInterestDate.Value = DateTime.Today;
                chkFlickrInterestEnabled.Checked = false;
            }
            else 
            {
                dtpFlickrEveryoneInterestDate.Enabled = true;                               
                dtpFlickrEveryoneInterestDate.Value = Settings.Local.FlickrEveryoneInterestDate;
                chkFlickrInterestEnabled.Checked = true;
            }

            switch (Settings.Local.FlickrEveryoneMode)
            {
                case Settings.FlickrEveryoneMode.RECENT: rbFlickrEveryoneRecent.Checked = true; break;
                case Settings.FlickrEveryoneMode.TAG: rbFlickrEveryoneTag.Checked = true; break;                
                case Settings.FlickrEveryoneMode.INTERESTINGNESS: rbFlickrEveryoneInterest.Checked = true; break;
            }

            switch(Settings.Local.FlickrContactType) 
            {
                case Settings.FlickrContactType.ALL: cbFlickrContactType.Text = "All"; break;
                case Settings.FlickrContactType.NEITHER: cbFlickrContactType.Text = "Neither"; break;
                case Settings.FlickrContactType.BOTH: cbFlickrContactType.Text = "Both"; break;
                case Settings.FlickrContactType.FRIENDS: cbFlickrContactType.Text = "Friends"; break;
                case Settings.FlickrContactType.FAMILY: cbFlickrContactType.Text = "Family"; break;
            }

            chkRandomize.Checked = Settings.Local.Randomize;
            //chkRandomize.Enabled = Settings.Local.RandomizeOptionEnabled;

            chkIncludeUserInContacts.Checked = Settings.Local.IncludeUserInContacts;

            //tbFlickrWait.Value = (int)Math.Ceiling(Settings.Local.FlickrQueryWait / 60d);

            /**
            switch (Settings.Local.Safeness)
            {
                case Settings.SearchSafeness.OFF: rbSafeOff.Checked = true; break;
                case Settings.SearchSafeness.MODERATE: rbSafeModerate.Checked = true; break;
                case Settings.SearchSafeness.STRICT: rbSafeStrict.Checked = true; break;
            }*/

            ckShowFile.Checked = Settings.Local.ShowFileName;
            ckShowStatus.Checked = Settings.Local.ShowStatus;
            ckZoomEnabled.Checked = Settings.Local.ZoomEnabled;
            ckPanEnabled.Checked = Settings.Local.PanEnabled;
            ckFadeEnabled.Checked = Settings.Local.FadeEnabled;
            ckShowLogo.Checked = Settings.Local.ShowLogo;
            
            //cbPageSize.Text = Convert.ToString(Settings.Flickr.FlickrPageSize);
            
            //lbFlickrWait.Text = FormatTime(tbFlickrWait.Value * 60, false, tbFlickrWait.Maximum * 60);

            gbFlickrUser.Visible = rbFlickrUser.Checked;
            tbFlickrUser.Visible = rbFlickrUser.Checked;
            btnCheckUser.Visible = rbFlickrUser.Checked;
            gbFlickrEveryone.Visible = rbFlickrEveryone.Checked;    
            gbFlickrGroup.Visible = rbFlickrGroup.Checked;            
            gbLocal.Visible = rbLocal.Checked;

            Point pOptions = new Point(8, 112);
            Point pHelp = new Point(8, 216);

            Size sOptions = new Size(316, 96);
            Size sHelp = new Size(316, 100);

            gbFlickrUser.Location = pOptions;
            gbFlickrEveryone.Location = pOptions;
            gbFlickrGroup.Location = pOptions;
            gbLocal.Location = pOptions;

            gbFlickrUser.Size = sOptions;
            gbFlickrEveryone.Size = sOptions;
            gbFlickrGroup.Size = sOptions;
            gbLocal.Size = sOptions;

            gbHelp.Location = pHelp;
            gbHelp.Size = sHelp;
            lbHelp.Size = new Size(306, 64);                                                

            bool powerOffActive = SystemParams.GetPowerOffActive();
            int powerOffTimeout = SystemParams.GetPowerOffTimeout();            

            ckPowerSetting.Checked = (powerOffActive && Settings.Local.UsePowerOffSetting);
    
            if (!powerOffActive || powerOffTimeout == -1) 
            {
                lbPowerTimeout.Enabled = false;
                ckPowerSetting.Enabled = false;
                lbPowerTimeout.Text = "";
            } 
            else if (powerOffTimeout != -1) 
            {
                lbPowerTimeout.Text = String.Format("({0} minutes)", powerOffTimeout/60);                
            }

            chkRandomize.Enabled = RandomEnabled();

            tbBaseDir.Text = Settings.CacheDirectory;
            tbMaxCacheSize.Text = String.Format("{0}", Settings.Local.MaxCacheSizeMB);

            tbLocalDir.Text = Settings.Local.LocalDirectory;

            authButton.Enabled = false;
            completeAuthButton.Enabled = false;
            removeAuthButton.Enabled = false;

			chkProxy.Checked = Settings.Local.DefineProxy;
			tbProxyDomain.Text = Settings.Local.ProxyDomain;
			tbProxyIpAddress.Text = Settings.Local.ProxyIpAddress;
			tbProxyPort.Text = Settings.Local.ProxyPort;
			tbProxyUsername.Text = Settings.Local.ProxyUsername;
			tbProxyPassword.Text = Settings.Local.ProxyPassword;

			tbProxyDomain.Enabled = chkProxy.Checked;
			tbProxyIpAddress.Enabled = chkProxy.Checked;
			tbProxyUsername.Enabled = chkProxy.Checked;
			tbProxyPassword.Enabled = chkProxy.Checked;
			tbProxyPort.Enabled = chkProxy.Checked;

			tbxKey.Text = Settings.Local.ApiKey;
			tbxSecret.Text = Settings.Local.SharedSecret;
			chkOverrideGLCheck.Checked = Settings.Local.OverrideOpenGLCheck;

            SetHelp();
        }

        public void SetHelp() 
        {
            if (rbFlickrUser.Checked) 
            {
                lbHelp.Text = "View a users photos. Enter in a user name or email address (or user id). Select all or by tag, set, favorites or contacts.";
            } 
            else if (rbFlickrGroup.Checked) 
            {
                lbHelp.Text = "View a photo group.";
            } 
            else if (rbFlickrEveryone.Checked) 
            {
                lbHelp.Text = "View everyone's photos by tag, recentness or interestingness. You may specify a date for interestingness.";
            } 
            else if (rbLocal.Checked) 
            {
                lbHelp.Text = "View photos stored in a directory on your machine.";
            }
        }

        public void LoadAuthorize(Object nullObj) 
        {           
            if (FlickrManager.IsAuthorized()) 
            {
                authButton.Enabled = false;
                completeAuthButton.Enabled = false;
                removeAuthButton.Enabled = true;
            } 
            else 
            {
                authButton.Enabled = true;
                completeAuthButton.Enabled = false;
                removeAuthButton.Enabled = false;
            }
            lbAuthStatus.Text = "";
            authorizing = false;
        }

        public void CheckDirectory(System.Windows.Forms.TextBox tbDir, bool okEmpty)
        {
            if (okEmpty && (tbDir.Text == null || tbDir.Text.Trim().Equals(""))) return;            

            try
            {
                DirectoryInfo di = new DirectoryInfo(tbDir.Text);
                if (!di.Exists) di.Create();
                errorProvider.SetError(tbDir, "");
            }
            catch (Exception e)
            {
                errorProvider.SetError(tbDir, "Invalid directory: " + e.Message);
            }
        }

		public bool SaveSettings()
		{

			CheckDirectory(tbBaseDir, true);    
            
			if (rbLocal.Checked) CheckDirectory(tbLocalDir, false);     
			else errorProvider.SetError(tbLocalDir, "");

			if (rbFlickrUser.Checked) 
			{
				if (tbFlickrUser.Text == null || tbFlickrUser.Text.Trim().Equals("")) 
					errorProvider.SetError(tbFlickrUser, "You must enter in a user name or email address");
				else 
					errorProvider.SetError(tbFlickrUser, "");
			} 
			else 
			{
				errorProvider.SetError(tbFlickrUser, "");
			}           

			if (!errorProvider.GetError(tbMaxCacheSize).Equals(""))
			{
				MessageBox.Show(this, "Max cache size value is invalid: " + errorProvider.GetError(tbMaxCacheSize),
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
				/**
				else if (!errorProvider.GetError(cbPageSize).Equals(""))
				{
					MessageBox.Show("Images per search value is invalid: " + errorProvider.GetError(cbPageSize),
						"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}*/
			else if (!errorProvider.GetError(tbMinSize).Equals(""))
			{
				MessageBox.Show(this, "Minimum size value is invalid: " + errorProvider.GetError(tbMinSize),
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			else if (!errorProvider.GetError(tbBaseDir).Equals(""))
			{
				MessageBox.Show(this, "Base directory is invalid: " + errorProvider.GetError(tbBaseDir),
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			else if (!errorProvider.GetError(tbLocalDir).Equals(""))
			{
				MessageBox.Show(this, "Local directory is invalid: " + errorProvider.GetError(tbLocalDir),
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			else if (!errorProvider.GetError(tbFlickrUser).Equals(""))
			{
				MessageBox.Show(this, errorProvider.GetError(tbFlickrUser),
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			else if (!errorProvider.GetError(cbFlickrUserTagMode).Equals(""))
			{
				MessageBox.Show(this, errorProvider.GetError(cbFlickrUserTagMode),
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			else if (!errorProvider.GetError(cbFlickrEveryoneTagMode).Equals(""))
			{
				MessageBox.Show(this, errorProvider.GetError(cbFlickrEveryoneTagMode),
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}


			Settings.Local.SecondsView = ConvertTicker(tbView.Value);             

            /**
            if (rbModeFlickr.Checked) Settings.Local.SearchType = Settings.SearchType.FLICKR;
            else if (rbModeSearch.Checked) Settings.Local.SearchType = Settings.SearchType.SEARCH;
             */

            String[] size = tbMinSize.Text.Split(',');
            int minWidth = Int32.Parse(size[0]);
            int minHeight = Int32.Parse(size[1]);

            Settings.Local.MaxSizeLabel = cbMaxSizeLabel.Text;

            Settings.Local.MinWidth = minWidth;
            Settings.Local.MinHeight = minHeight;

            if (rbFlickrUser.Checked) Settings.Local.FlickrMode = Settings.FlickrMode.USER;
            else if (rbFlickrGroup.Checked) Settings.Local.FlickrMode = Settings.FlickrMode.GROUP;
            else if (rbFlickrEveryone.Checked) Settings.Local.FlickrMode = Settings.FlickrMode.EVERYONE;
            else if (rbLocal.Checked) Settings.Local.FlickrMode = Settings.FlickrMode.LOCAL;

            if (rbFlickrUserAll.Checked) Settings.Local.FlickrUserMode = Settings.FlickrUserMode.ALL;
            else if (rbFlickrUserFav.Checked) Settings.Local.FlickrUserMode = Settings.FlickrUserMode.FAVORITES;
            else if (rbFlickrUserContacts.Checked) Settings.Local.FlickrUserMode = Settings.FlickrUserMode.CONTACTS;
            else if (rbFlickrUserSet.Checked) Settings.Local.FlickrUserMode = Settings.FlickrUserMode.SET;
            else if (rbFlickrUserTag.Checked) Settings.Local.FlickrUserMode = Settings.FlickrUserMode.TAG;

            Settings.Local.FlickrUser = tbFlickrUser.Text;
            Settings.Local.FlickrUserSet = tbFlickrUserSet.Text;
            Settings.Local.FlickrUserTag = tbFlickrUserTags.Text;
            Settings.Local.FlickrUserTagMode = cbFlickrUserTagMode.Text;

            Settings.Local.FlickrGroup = tbFlickrGroup.Text;            

            if (rbFlickrEveryoneRecent.Checked) Settings.Local.FlickrEveryoneMode = Settings.FlickrEveryoneMode.RECENT;
            else if (rbFlickrEveryoneTag.Checked) Settings.Local.FlickrEveryoneMode = Settings.FlickrEveryoneMode.TAG;            
            else if (rbFlickrEveryoneInterest.Checked) Settings.Local.FlickrEveryoneMode = Settings.FlickrEveryoneMode.INTERESTINGNESS;            
            Settings.Local.FlickrEveryoneTag = tbFlickrEveryoneTags.Text;
            Settings.Local.FlickrEveryoneTagMode = cbFlickrEveryoneTagMode.Text;

            if (!dtpFlickrEveryoneInterestDate.Enabled) 
            {
                Settings.Local.FlickrEveryoneInterestDate = DateTime.MinValue;                              
            } 
            else 
            {
                Settings.Local.FlickrEveryoneInterestDate = dtpFlickrEveryoneInterestDate.Value;
            }

            //Settings.Local.FlickrQueryWait = tbFlickrWait.Value * 60;

            /**
            if (rbSafeModerate.Checked) Settings.Local.Safeness = Settings.SearchSafeness.MODERATE;
            else if (rbSafeStrict.Checked) Settings.Local.Safeness = Settings.SearchSafeness.STRICT;
            else if (rbSafeOff.Checked) Settings.Local.Safeness = Settings.SearchSafeness.OFF;
             */

            Settings.Local.ShowFileName = ckShowFile.Checked;
            Settings.Local.ShowStatus = ckShowStatus.Checked;
            Settings.Local.ZoomEnabled = ckZoomEnabled.Checked;
            Settings.Local.PanEnabled = ckPanEnabled.Checked;
            Settings.Local.FadeEnabled = ckFadeEnabled.Checked;
            Settings.Local.ShowLogo = ckShowLogo.Checked;

            if (cbFlickrContactType.Text.Equals("All")) Settings.Local.FlickrContactType = Settings.FlickrContactType.ALL;
            else if (cbFlickrContactType.Text.Equals("Neither")) Settings.Local.FlickrContactType = Settings.FlickrContactType.NEITHER;
            else if (cbFlickrContactType.Text.Equals("Both")) Settings.Local.FlickrContactType = Settings.FlickrContactType.BOTH;
            else if (cbFlickrContactType.Text.Equals("Friends")) Settings.Local.FlickrContactType = Settings.FlickrContactType.FRIENDS;
            else if (cbFlickrContactType.Text.Equals("Family")) Settings.Local.FlickrContactType = Settings.FlickrContactType.FAMILY;                   
                
            Settings.Local.Randomize = chkRandomize.Checked;
            Settings.Local.RandomizeOptionEnabled = chkRandomize.Enabled;

            Settings.Local.IncludeUserInContacts = chkIncludeUserInContacts.Checked;

			Settings.Local.DefineProxy = chkProxy.Checked;
			Settings.Local.ProxyDomain = tbProxyDomain.Text.Trim();
			Settings.Local.ProxyIpAddress = tbProxyIpAddress.Text.Trim();
			Settings.Local.ProxyPort = tbProxyPort.Text.Trim();
			Settings.Local.ProxyUsername = tbProxyUsername.Text.Trim();
			Settings.Local.ProxyPassword = tbProxyPassword.Text.Trim();			
			
			Settings.Local.ApiKey = tbxKey.Text.Trim();
			Settings.Local.SharedSecret = tbxSecret.Text.Trim();

			Settings.Local.OverrideOpenGLCheck = chkOverrideGLCheck.Checked;

            //Settings.Flickr.FlickrPageSize = Int32.Parse(cbPageSize.Text);
            Settings.Local.MaxCacheSizeMB = Int32.Parse(tbMaxCacheSize.Text);

            if (ckPowerSetting.Enabled)
            {
                Settings.Local.UsePowerOffSetting = ckPowerSetting.Checked;
            }

            if (tbBaseDir.Text != null && !tbBaseDir.Text.Trim().Equals(""))
                Settings.Local.CacheDirectory = tbBaseDir.Text;

            if (tbLocalDir.Text != null && !tbLocalDir.Text.Trim().Equals(""))
                Settings.Local.LocalDirectory = tbLocalDir.Text;                            

            // Reset flickr local settings (i.e. page positions)
            Settings.Flickr.Reset();

            Settings.Save();
            return true;
        }

		private int ConvertTicker(int val) 
		{
			if (val > 238) 
			{	
				return (val-236) * 3600;	
			}
			else if (val > 120) 
			{
				return (val-118) * 60;
			} 
			else 
			{
				return val;
			}
		}

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new ConfigurationForm());
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (SaveSettings()) Dispose();
        }

        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
			try 
			{
				LoadSettings();
			} 
			catch(Exception ex) 
			{
				Log.Error("Error loading settings", ex);
			}
        }

        private void tbView_ValueChanged(object sender, EventArgs e)
        {            			

			//int secAdj = (tbView.Value > 120 ? (tbView.Value-118) * 60 : tbView.Value);

			int secAdj = ConvertTicker(tbView.Value);

            lbView.Text = FormatTime(secAdj, true, -1);			
			//Log.Debug(tbView.Value + "--> " + lbView.Text + " --> " + secAdj);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private string FormatTime(int seconds, bool secMin, int never)
        {
            if (seconds == never)
            {
                return "Off";
            }			

            TimeSpan ts = new TimeSpan(0, 0, 0, seconds, 0);
            if (secMin)
            {
                return (ts.Days > 0 ? " " + ts.Days + (ts.Days > 1 ? " days" : " day") : "") +
					(ts.Hours > 0 ? " " + ts.Hours + (ts.Hours > 1 ? " hours" : " hour") : "") +
					(ts.Minutes > 0 ? " " + ts.Minutes + (ts.Minutes > 1 ? " minutes" : " minute") : "") +
                    (ts.Seconds > 0 ? " " + ts.Seconds + (ts.Seconds > 1 ? " seconds" : " second") : "");
            }
            else
            {
                return (ts.Days > 0 ? " " + ts.Days + (ts.Days > 1 ? " days" : " days") : "") +
					(ts.Hours > 0 ? " " + ts.Hours + (ts.Hours > 1 ? " hours" : " hour") : "") +
                    (ts.Minutes > 0 ? " " + ts.Minutes + (ts.Minutes > 1 ? " minutes" : " minute") : "");
            }
        }

        private void cbMaxCacheSize_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int n = Int32.Parse(tbMaxCacheSize.Text);
                errorProvider.SetError(tbMaxCacheSize, "");
            }
            catch
            {
                errorProvider.SetError(tbMaxCacheSize, "Not an integer value.");
            }
        }

        private void cbPageSize_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //int n = Int32.Parse(cbPageSize.Text);
                //errorProvider.SetError(cbPageSize, "");
            }
            catch
            {
                //errorProvider.SetError(cbPageSize, "Not an integer value.");
            }
        }

        private void cbMinSize_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                String[] size = tbMinSize.Text.Split(',');
                if (size == null || size.Length != 2)
                {
                    errorProvider.SetError(tbMinSize, "Width,Height (e.g. 640,480)");
                    return;
                }
                int minWidth = Int32.Parse(size[0]);
                int minHeight = Int32.Parse(size[1]);                
                errorProvider.SetError(tbMinSize, "");
            }
            catch
            {
                errorProvider.SetError(tbMinSize, "Width,Height (e.g. 640,480)");
            }
            
        }

        private void rbFlickrUser_CheckedChanged(object sender, EventArgs e)
        {
            gbFlickrUser.Visible = rbFlickrUser.Checked;
            tbFlickrUser.Visible = rbFlickrUser.Checked;
            btnCheckUser.Visible = rbFlickrUser.Checked;            
            SetHelp();          
        }

        private void rbFlickrEveryone_CheckedChanged(object sender, EventArgs e)
        {
            gbFlickrEveryone.Visible = rbFlickrEveryone.Checked;
            SetHelp();
        }

        private void rbFlickrGroup_CheckedChanged(object sender, EventArgs e)
        {
            //tbFlickrGroup.Enabled = rbFlickrGroup.Checked;
            //lbFlickrGroup.Enabled = rbFlickrGroup.Checked;
            gbFlickrGroup.Visible = rbFlickrGroup.Checked;
            SetHelp();
        }

        private void rbLocal_CheckedChanged(object sender, System.EventArgs e)
        {           
            //tbLocalDir.Enabled = rbLocal.Checked;
            //localButton.Enabled = rbLocal.Checked;
            gbLocal.Visible = rbLocal.Checked;
            SetHelp();
        }

        private void rbFlickrUserFav_CheckedChanged(object sender, System.EventArgs e)
        {
            chkRandomize.Enabled = RandomEnabled();
        }

        private void baseDirButton_Click(object sender, EventArgs e)
        {            
            baseDirBrowserDialog.SelectedPath = Settings.BaseDirectory;
            if (baseDirBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                String directory = baseDirBrowserDialog.SelectedPath + Path.DirectorySeparatorChar + "Slickr";
                tbBaseDir.Text = new DirectoryInfo(directory).FullName;
            }
        }

        private void authButton_Click(object sender, System.EventArgs e)
        {           
            lbAuthStatus.Text = "Showing authorize...";
            try 
            {
                Settings.Flickr.AuthEnabled = false;
                FlickrManager.Deinit();
                String url = FlickrManager.Authorize();
                if (url != null) 
                {                   
                    tbAuthStatus.Visible = true;
                    authCopyButton.Visible = true;
                    tbAuthStatus.Text = url;
                    System.Diagnostics.Process.Start(url);
                    authButton.Enabled = false;
                    completeAuthButton.Enabled = true;
                    lbAuthStatus.Text = "Showing authorize. When you are finished, click complete.";
                }
            } 
            catch(FlickrNet.FlickrException fe) 
            {
                lbAuthStatus.Text = "There was an error";
                MessageBox.Show(this, "Could not authorize. There was an error: " + Log.GetError(fe),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void completeAuthButton_Click(object sender, System.EventArgs e)
        {
            lbAuthStatus.Text = "Completing authorize.";
            try 
            { 
                if (FlickrManager.CompleteAuthorize()) 
                {                               
                    completeAuthButton.Enabled = false;
                    removeAuthButton.Enabled = true;
                    lbAuthStatus.Text = "Authorize completed.";
                    tbAuthStatus.Visible = false;
                    authCopyButton.Visible = false;
                } 
                else 
                {
                    authButton.Enabled = true;
                    completeAuthButton.Enabled = false;             
                    tbAuthStatus.Visible = false;
                    authCopyButton.Visible = false;
                }
            } 
            catch(FlickrNet.FlickrException fe) 
            {
                authButton.Enabled = true;
                completeAuthButton.Enabled = false;             
                tbAuthStatus.Visible = false;
                authCopyButton.Visible = false;
                lbAuthStatus.Text = "There was an error";
                MessageBox.Show(this, "Could not complete authorize. Did you go to the url? " + tbAuthStatus.Text + "\n"
                    + "There was an error: " + Log.GetError(fe),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void removeAuthButton_Click(object sender, System.EventArgs e)
        {           
            tbAuthStatus.Visible = false;
            authCopyButton.Visible = false;
            if (FlickrManager.RemoveAuthorize()) 
            {
                authButton.Enabled = true;  
                removeAuthButton.Enabled = false;               
            }            
        }

        private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tabControl1.SelectedTab.Text.Equals("Authorize") && !authorizing)
            {
                authorizing = true;
                authButton.Enabled = false;
                completeAuthButton.Enabled = false;
                removeAuthButton.Enabled = false;

                tbAuthStatus.Visible = false;
                authCopyButton.Visible = false;
                tabControl1.Refresh();
                lbAuthStatus.Text = "Checking authorization...";
                lbAuthStatus.Refresh();
                
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadAuthorize), null);
                //LoadAuthorize();              
            }
        }

        private void linkEmail_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            try 
            {
                System.Diagnostics.Process.Start("mailto:gabrielh@gmail.com");
            } 
            catch(Exception ee) 
            {
                Log.Error("Error on email click", ee);
            }

        }

        private void linkCdsw_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            try 
            {
                System.Diagnostics.Process.Start("http://cellardoorsw.com/slickr/");
            } 
            catch(Exception ee) 
            {
                Log.Error("Error on url click", ee);
            }
        }

        private void pictureBox2_Click(object sender, System.EventArgs e)
        {
            try 
            {
                String url = @"https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=gh38@cornell.edu&item_name=Slickr Fund";
                System.Diagnostics.Process.Start(url);
            } 
            catch(Exception ee) 
            {
                Log.Error("Error on url click", ee);
            }
        }

        private void localButton_Click(object sender, System.EventArgs e)
        {
            localDirBrowserDialog.SelectedPath = Settings.Local.LocalDirectory;
            if (localDirBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                String directory = localDirBrowserDialog.SelectedPath;
                tbLocalDir.Text = new DirectoryInfo(directory).FullName;
            }
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            try 
            {
                String url = @"http://www.flickr.com/groups/";
                System.Diagnostics.Process.Start(url);
            } 
            catch(Exception ee) 
            {
                Log.Error("Error on url click", ee);
            }
        }

        private void ckCheckUser_Click(object sender, System.EventArgs e)
        {           
            btnCheckUser.Enabled = false;
            btnCheckUser.Text = "Checking...";
            ThreadPool.QueueUserWorkItem(new WaitCallback(CheckUser), null);
        }

        private void CheckUser(Object nullObj) {
            String user = tbFlickrUser.Text.Trim();
            if (!user.Equals("")) 
            {
                try 
                {
                    FlickrNet.User fuser = FlickrImageQuery.SearchUser(user);
                    if (fuser != null) 
                    {
                        String info = fuser.UserName + " (" + fuser.UserId + ")";                       
                        MessageBox.Show(this, "The user was found: " + info,
                            "User found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } 
                    else 
                    {
                        MessageBox.Show(this, "The user: '" + user + "' was not found",
                            "User not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                } 
                catch(FlickrNet.FlickrException fe) 
                {
                    MessageBox.Show(this, "There was an error: " + Log.GetError(fe),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            btnCheckUser.Text = "Check";
            btnCheckUser.Enabled = true;
        }

        private void ckCheckGroup_Click(object sender, System.EventArgs e)
        {
            ckCheckGroup.Enabled = false;
            ckCheckGroup.Text = "Checking...";
            ThreadPool.QueueUserWorkItem(new WaitCallback(CheckGroup), null);
        }

        private void CheckGroup(Object nullObj) {
            String group = tbFlickrGroup.Text.Trim();
            if (!group.Equals("")) 
            {
                try 
                {
                    FlickrNet.GroupInfo fgroup = FlickrImageQuery.GetGroup(group);
                    if (fgroup != null) 
                    {
                        String info = fgroup.GroupName + " (" + fgroup.GroupId + ")";
                        MessageBox.Show(this, "The group was found: " + info,
                            "Group found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } 
                    else 
                    {
                        MessageBox.Show(this, "The group: " + group + " was not found",
                            "Group not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                } 
                catch(FlickrNet.FlickrException fe) 
                {
                    MessageBox.Show(this, "There was an error: " + Log.GetError(fe),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ckCheckGroup.Text = "Check";
            ckCheckGroup.Enabled = true;
        }

        private void cbFlickrUserTagMode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {           
            String tagMode = cbFlickrUserTagMode.Text;
            if (tagMode == null || (!tagMode.Equals("Any") && !tagMode.Equals("All")))
            {
                errorProvider.SetError(cbFlickrUserTagMode, "Invalid tag mode");
                return;
            }               
            errorProvider.SetError(cbFlickrUserTagMode, "");
        }

        private void cbFlickrEveryoneTagMode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {           
            String tagMode = cbFlickrEveryoneTagMode.Text;
            if (tagMode == null || (!tagMode.Equals("Any") && !tagMode.Equals("All")))
            {
                errorProvider.SetError(cbFlickrEveryoneTagMode, "Invalid tag mode");
                return;
            }               
            errorProvider.SetError(cbFlickrEveryoneTagMode, "");

        }

        private void authCopyButton_Click(object sender, System.EventArgs e)
        {
            tbAuthStatus.SelectAll();
            tbAuthStatus.Copy();            
        }

        private void chkFlickrInterestEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkFlickrInterestEnabled.Checked) 
            {               
                dtpFlickrEveryoneInterestDate.Enabled = true;
            } 
            else 
            {
                dtpFlickrEveryoneInterestDate.Enabled = false;
            }
        }

        private void btnReset_Click_1(object sender, System.EventArgs e)
        {
            Settings.Flickr.Reset();
            Settings.SaveFlickrSettings();
        }

        private void debugButton_Click(object sender, System.EventArgs e)
        {
            DebugInfoForm form = new DebugInfoForm();
			form.AddProperties(Settings.Local);
			form.AddProperties(Settings.Flickr);
            form.ShowDialog(this);
        }

        private void btnClearCache_Click(object sender, System.EventArgs e)
        {
			btnClearCache.Enabled = false;
            DirectoryInfo di = new DirectoryInfo(Settings.Local.CacheDirectory);
            if (di.Exists) 
            {
                di.Delete(true);
                di.Create();
            }
			btnClearCache.Enabled = true;
        }

        private void rbFlickrUserAll_CheckedChanged(object sender, System.EventArgs e)
        {
            chkRandomize.Enabled = RandomEnabled();
        }       

        public bool RandomEnabled() 
        {

            return (rbFlickrUserAll.Checked || rbFlickrUserFav.Checked);
        }

		private void chkProxy_CheckedChanged(object sender, System.EventArgs e)
		{
			tbProxyDomain.Enabled = chkProxy.Checked;
			tbProxyIpAddress.Enabled = chkProxy.Checked;
			tbProxyPort.Enabled = chkProxy.Checked;
			tbProxyUsername.Enabled = chkProxy.Checked;
			tbProxyPassword.Enabled = chkProxy.Checked;
		}

		private void btnTestOpenGL_Click(object sender, System.EventArgs e)
		{
			DebugInfoForm form = new DebugInfoForm();
						
			IntPtr hDC;
			IntPtr hRC;
			string errorMessage;
			bool ok = OpenGlUtils.InitializeGL(Handle, out hDC, out hRC, 32, out errorMessage);

			if (!ok) 
			{
				MessageBox.Show(this, errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int maxTextureSize = OpenGlUtils.GetInteger(Gl.GL_MAX_TEXTURE_SIZE, -1);
			int max3dSize = OpenGlUtils.GetInteger(Gl.GL_MAX_3D_TEXTURE_SIZE, -1);

			int maxTextureUnits = OpenGlUtils.GetInteger(Gl.GL_MAX_TEXTURE_UNITS, -1);

			String renderer = OpenGlUtils.GetString(Gl.GL_RENDERER);
			String vendor = OpenGlUtils.GetString(Gl.GL_VENDOR);
			String version = OpenGlUtils.GetString(Gl.GL_VERSION);

			String extensions = OpenGlUtils.GetString(Gl.GL_EXTENSIONS);			

			String[] extensionsArray = extensions.Split(' ');

			form.Add("GL_MAX_TEXTURE_SIZE", String.Format("{0}", maxTextureSize));
			form.Add("GL_MAX_3D_TEXTURE_SIZE", String.Format("{0}", max3dSize));
			form.Add("GL_MAX_TEXTURE_UNITS", String.Format("{0}", maxTextureUnits));
			form.Add("GL_RENDERER", renderer);
			form.Add("GL_VENDOR", vendor);
			form.Add("GL_VERSION", version);
			for(int i = 0; i < extensionsArray.Length; i++) 
				form.Add("Extension(" + i + ")", extensionsArray[i]);

			form.ShowDialog(this);

			OpenGlUtils.DeinitializeGL(Handle, ref hDC, ref hRC, out errorMessage);
		}

		private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try 
			{
				String url = @"http://www.flickr.com/services/api/keys";
				System.Diagnostics.Process.Start(url);
			} 
			catch(Exception ee) 
			{
				Log.Error("Error on url click", ee);
			}
		}

    
    }
}