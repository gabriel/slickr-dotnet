using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CommonCode
{
	public class ExceptionDialog : System.Windows.Forms.Form
	{
		private delegate void TabRenderer();

		private enum TabToRender
		{
			General = 0,
			Stack = 1,
			Inner = 2,
			Other = 3
		};

		private const string UNKNOWN_EXCEPTION = "Unknown";

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ColumnHeader _descriptionHeader;
		private System.Windows.Forms.Label _exceptionLabel;
		private System.Windows.Forms.Label _exceptionMessageLabel;
		private System.Windows.Forms.TextBox _exceptionMessageValueText;
		private System.Windows.Forms.Label _exceptionSourceLabel;
		private System.Windows.Forms.TextBox _exceptionSourceValueText;
		private System.Windows.Forms.Label _exceptionTargetMethodLabel;
		private System.Windows.Forms.TextBox _exceptionTargetMethodValueText;
		private System.Windows.Forms.TextBox _exceptionTypeValueText;
		private System.Windows.Forms.TabPage _generalTabPage;
		private bool _hasGeneralBeenCalled = false;
		private bool _hasInnerExceptionBeenCalled = false;
		private bool _hasOtherInformationBeenCalled = false;
		private bool _hasStackTraceBeenCalled = false;
		private System.Windows.Forms.TextBox _helpLinkValueText;
		private System.Windows.Forms.Label _helpLinkLabel;
		private System.Windows.Forms.TabControl _informationTab;
		private System.Windows.Forms.TreeView _innerExceptionsTreeView;
		private System.Windows.Forms.TabPage _innerTabPage;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.ListView _otherInformationList;
		private System.Windows.Forms.ColumnHeader _methodHeader;
		private System.Windows.Forms.ColumnHeader _nameHeader;
		private TabRenderer[] _renderers = new TabRenderer[Enum.GetValues(typeof(TabToRender)).Length];
		private System.Windows.Forms.TabPage _specificTabPage;
		private System.Windows.Forms.TabPage _stackTabPage;
		private System.Windows.Forms.ListView _stackTraceList;
		private System.Windows.Forms.Button _copyButton;
		private Exception _targetEx;

		//  TODO (3/26/2004): Remember: OSFeature.IsPresent(OSFeature.Themes) 
		public ExceptionDialog() : base()
		{
			InitializeComponent();
			_renderers[(int)TabToRender.General] = new TabRenderer(this.DisplayGeneralInformation);
			_renderers[(int)TabToRender.Inner] = new TabRenderer(this.DisplayInnerExceptionTrace);
			_renderers[(int)TabToRender.Other] = new TabRenderer(this.DisplayOtherInformation);
			_renderers[(int)TabToRender.Stack] = new TabRenderer(this.DisplayStackTrace);
		}

		public ExceptionDialog(Exception TargetException) : this()
		{
			_targetEx = TargetException;
		}

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
			this._okButton = new System.Windows.Forms.Button();
			this._exceptionLabel = new System.Windows.Forms.Label();
			this._informationTab = new System.Windows.Forms.TabControl();
			this._generalTabPage = new System.Windows.Forms.TabPage();
			this._helpLinkValueText = new System.Windows.Forms.TextBox();
			this._helpLinkLabel = new System.Windows.Forms.Label();
			this._exceptionSourceValueText = new System.Windows.Forms.TextBox();
			this._exceptionTargetMethodValueText = new System.Windows.Forms.TextBox();
			this._exceptionMessageValueText = new System.Windows.Forms.TextBox();
			this._exceptionTargetMethodLabel = new System.Windows.Forms.Label();
			this._exceptionSourceLabel = new System.Windows.Forms.Label();
			this._exceptionMessageLabel = new System.Windows.Forms.Label();
			this._stackTabPage = new System.Windows.Forms.TabPage();
			this._stackTraceList = new System.Windows.Forms.ListView();
			this._methodHeader = new System.Windows.Forms.ColumnHeader();
			this._innerTabPage = new System.Windows.Forms.TabPage();
			this._innerExceptionsTreeView = new System.Windows.Forms.TreeView();
			this._specificTabPage = new System.Windows.Forms.TabPage();
			this._otherInformationList = new System.Windows.Forms.ListView();
			this._nameHeader = new System.Windows.Forms.ColumnHeader();
			this._descriptionHeader = new System.Windows.Forms.ColumnHeader();
			this._exceptionTypeValueText = new System.Windows.Forms.TextBox();
			this._copyButton = new System.Windows.Forms.Button();
			this._informationTab.SuspendLayout();
			this._generalTabPage.SuspendLayout();
			this._stackTabPage.SuspendLayout();
			this._innerTabPage.SuspendLayout();
			this._specificTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point(510, 272);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(80, 24);
			this._okButton.TabIndex = 4;
			this._okButton.Text = "&OK";
			// 
			// _exceptionLabel
			// 
			this._exceptionLabel.Location = new System.Drawing.Point(8, 8);
			this._exceptionLabel.Name = "_exceptionLabel";
			this._exceptionLabel.Size = new System.Drawing.Size(200, 16);
			this._exceptionLabel.TabIndex = 0;
			this._exceptionLabel.Text = "The following exception has occurred:";
			// 
			// _informationTab
			// 
			this._informationTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._informationTab.Controls.Add(this._generalTabPage);
			this._informationTab.Controls.Add(this._stackTabPage);
			this._informationTab.Controls.Add(this._innerTabPage);
			this._informationTab.Controls.Add(this._specificTabPage);
			this._informationTab.Location = new System.Drawing.Point(8, 32);
			this._informationTab.Name = "_informationTab";
			this._informationTab.SelectedIndex = 0;
			this._informationTab.Size = new System.Drawing.Size(584, 231);
			this._informationTab.TabIndex = 2;
			this._informationTab.SelectedIndexChanged += new System.EventHandler(this.OnInformationTabSelectedIndexChanged);
			// 
			// _generalTabPage
			// 
			this._generalTabPage.Controls.Add(this._helpLinkValueText);
			this._generalTabPage.Controls.Add(this._helpLinkLabel);
			this._generalTabPage.Controls.Add(this._exceptionSourceValueText);
			this._generalTabPage.Controls.Add(this._exceptionTargetMethodValueText);
			this._generalTabPage.Controls.Add(this._exceptionMessageValueText);
			this._generalTabPage.Controls.Add(this._exceptionTargetMethodLabel);
			this._generalTabPage.Controls.Add(this._exceptionSourceLabel);
			this._generalTabPage.Controls.Add(this._exceptionMessageLabel);
			this._generalTabPage.Location = new System.Drawing.Point(4, 22);
			this._generalTabPage.Name = "_generalTabPage";
			this._generalTabPage.Size = new System.Drawing.Size(576, 205);
			this._generalTabPage.TabIndex = 0;
			this._generalTabPage.Text = "General Information";
			// 
			// _helpLinkValueText
			// 
			this._helpLinkValueText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._helpLinkValueText.Location = new System.Drawing.Point(96, 175);
			this._helpLinkValueText.Name = "_helpLinkValueText";
			this._helpLinkValueText.ReadOnly = true;
			this._helpLinkValueText.Size = new System.Drawing.Size(472, 20);
			this._helpLinkValueText.TabIndex = 7;
			this._helpLinkValueText.TabStop = false;
			this._helpLinkValueText.Text = "";
			// 
			// _helpLinkLabel
			// 
			this._helpLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._helpLinkLabel.Location = new System.Drawing.Point(8, 175);
			this._helpLinkLabel.Name = "_helpLinkLabel";
			this._helpLinkLabel.Size = new System.Drawing.Size(88, 16);
			this._helpLinkLabel.TabIndex = 6;
			this._helpLinkLabel.Text = "Help Link:";
			this._helpLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _exceptionSourceValueText
			// 
			this._exceptionSourceValueText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._exceptionSourceValueText.Location = new System.Drawing.Point(96, 127);
			this._exceptionSourceValueText.Name = "_exceptionSourceValueText";
			this._exceptionSourceValueText.ReadOnly = true;
			this._exceptionSourceValueText.Size = new System.Drawing.Size(472, 20);
			this._exceptionSourceValueText.TabIndex = 3;
			this._exceptionSourceValueText.TabStop = false;
			this._exceptionSourceValueText.Text = "";
			// 
			// _exceptionTargetMethodValueText
			// 
			this._exceptionTargetMethodValueText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._exceptionTargetMethodValueText.Location = new System.Drawing.Point(96, 151);
			this._exceptionTargetMethodValueText.Name = "_exceptionTargetMethodValueText";
			this._exceptionTargetMethodValueText.ReadOnly = true;
			this._exceptionTargetMethodValueText.Size = new System.Drawing.Size(472, 20);
			this._exceptionTargetMethodValueText.TabIndex = 5;
			this._exceptionTargetMethodValueText.TabStop = false;
			this._exceptionTargetMethodValueText.Text = "";
			// 
			// _exceptionMessageValueText
			// 
			this._exceptionMessageValueText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._exceptionMessageValueText.Location = new System.Drawing.Point(96, 8);
			this._exceptionMessageValueText.Multiline = true;
			this._exceptionMessageValueText.Name = "_exceptionMessageValueText";
			this._exceptionMessageValueText.ReadOnly = true;
			this._exceptionMessageValueText.Size = new System.Drawing.Size(472, 111);
			this._exceptionMessageValueText.TabIndex = 1;
			this._exceptionMessageValueText.TabStop = false;
			this._exceptionMessageValueText.Text = "";
			// 
			// _exceptionTargetMethodLabel
			// 
			this._exceptionTargetMethodLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._exceptionTargetMethodLabel.Location = new System.Drawing.Point(8, 151);
			this._exceptionTargetMethodLabel.Name = "_exceptionTargetMethodLabel";
			this._exceptionTargetMethodLabel.Size = new System.Drawing.Size(88, 16);
			this._exceptionTargetMethodLabel.TabIndex = 4;
			this._exceptionTargetMethodLabel.Text = "Target Method:";
			this._exceptionTargetMethodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _exceptionSourceLabel
			// 
			this._exceptionSourceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._exceptionSourceLabel.Location = new System.Drawing.Point(8, 127);
			this._exceptionSourceLabel.Name = "_exceptionSourceLabel";
			this._exceptionSourceLabel.Size = new System.Drawing.Size(88, 16);
			this._exceptionSourceLabel.TabIndex = 2;
			this._exceptionSourceLabel.Text = "Source:";
			this._exceptionSourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _exceptionMessageLabel
			// 
			this._exceptionMessageLabel.Location = new System.Drawing.Point(8, 8);
			this._exceptionMessageLabel.Name = "_exceptionMessageLabel";
			this._exceptionMessageLabel.Size = new System.Drawing.Size(88, 16);
			this._exceptionMessageLabel.TabIndex = 0;
			this._exceptionMessageLabel.Text = "Message:";
			this._exceptionMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _stackTabPage
			// 
			this._stackTabPage.Controls.Add(this._stackTraceList);
			this._stackTabPage.Location = new System.Drawing.Point(4, 22);
			this._stackTabPage.Name = "_stackTabPage";
			this._stackTabPage.Size = new System.Drawing.Size(584, 213);
			this._stackTabPage.TabIndex = 2;
			this._stackTabPage.Text = "Stack Trace";
			// 
			// _stackTraceList
			// 
			this._stackTraceList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._stackTraceList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							  this._methodHeader});
			this._stackTraceList.Location = new System.Drawing.Point(8, 7);
			this._stackTraceList.Name = "_stackTraceList";
			this._stackTraceList.Size = new System.Drawing.Size(568, 199);
			this._stackTraceList.TabIndex = 1;
			this._stackTraceList.View = System.Windows.Forms.View.Details;
			// 
			// _methodHeader
			// 
			this._methodHeader.Text = "Method";
			this._methodHeader.Width = 400;
			// 
			// _innerTabPage
			// 
			this._innerTabPage.Controls.Add(this._innerExceptionsTreeView);
			this._innerTabPage.Location = new System.Drawing.Point(4, 22);
			this._innerTabPage.Name = "_innerTabPage";
			this._innerTabPage.Size = new System.Drawing.Size(584, 213);
			this._innerTabPage.TabIndex = 1;
			this._innerTabPage.Text = "Inner Exception Trace";
			// 
			// _innerExceptionsTreeView
			// 
			this._innerExceptionsTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._innerExceptionsTreeView.ImageIndex = -1;
			this._innerExceptionsTreeView.Location = new System.Drawing.Point(8, 8);
			this._innerExceptionsTreeView.Name = "_innerExceptionsTreeView";
			this._innerExceptionsTreeView.SelectedImageIndex = -1;
			this._innerExceptionsTreeView.Size = new System.Drawing.Size(568, 199);
			this._innerExceptionsTreeView.TabIndex = 0;
			// 
			// _specificTabPage
			// 
			this._specificTabPage.Controls.Add(this._otherInformationList);
			this._specificTabPage.Location = new System.Drawing.Point(4, 22);
			this._specificTabPage.Name = "_specificTabPage";
			this._specificTabPage.Size = new System.Drawing.Size(584, 213);
			this._specificTabPage.TabIndex = 3;
			this._specificTabPage.Text = "Other Information";
			// 
			// _otherInformationList
			// 
			this._otherInformationList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._otherInformationList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																									this._nameHeader,
																									this._descriptionHeader});
			this._otherInformationList.Location = new System.Drawing.Point(8, 8);
			this._otherInformationList.Name = "_otherInformationList";
			this._otherInformationList.Size = new System.Drawing.Size(568, 199);
			this._otherInformationList.TabIndex = 0;
			this._otherInformationList.View = System.Windows.Forms.View.Details;
			// 
			// _nameHeader
			// 
			this._nameHeader.Text = "Name";
			this._nameHeader.Width = 120;
			// 
			// _descriptionHeader
			// 
			this._descriptionHeader.Text = "Description";
			this._descriptionHeader.Width = 220;
			// 
			// _exceptionTypeValueText
			// 
			this._exceptionTypeValueText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._exceptionTypeValueText.Location = new System.Drawing.Point(208, 6);
			this._exceptionTypeValueText.Name = "_exceptionTypeValueText";
			this._exceptionTypeValueText.ReadOnly = true;
			this._exceptionTypeValueText.Size = new System.Drawing.Size(384, 20);
			this._exceptionTypeValueText.TabIndex = 1;
			this._exceptionTypeValueText.TabStop = false;
			this._exceptionTypeValueText.Text = "";
			// 
			// _copyButton
			// 
			this._copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._copyButton.Location = new System.Drawing.Point(424, 272);
			this._copyButton.Name = "_copyButton";
			this._copyButton.Size = new System.Drawing.Size(80, 24);
			this._copyButton.TabIndex = 3;
			this._copyButton.Text = "&Copy";
			this._copyButton.Click += new System.EventHandler(this.OnCopyButtonClick);
			// 
			// ExceptionDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(600, 301);
			this.Controls.Add(this._copyButton);
			this.Controls.Add(this._exceptionTypeValueText);
			this.Controls.Add(this._informationTab);
			this.Controls.Add(this._exceptionLabel);
			this.Controls.Add(this._okButton);
			this.MinimizeBox = false;
			this.Name = "ExceptionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Exception Information";
			this.Load += new System.EventHandler(this.OnExceptionDialogLoad);
			this._informationTab.ResumeLayout(false);
			this._generalTabPage.ResumeLayout(false);
			this._stackTabPage.ResumeLayout(false);
			this._innerTabPage.ResumeLayout(false);
			this._specificTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void DisplayGeneralInformation()
		{
			if(false == _hasGeneralBeenCalled)
			{
				_exceptionMessageValueText.Text = _targetEx.Message;
				_exceptionSourceValueText.Text = _targetEx.Source;
				_exceptionTargetMethodValueText.Text = this.GetTargetMethodFormat(_targetEx.TargetSite);
				_helpLinkValueText.Text = _targetEx.HelpLink;
				_hasGeneralBeenCalled = true;
			}
		}

		private void DisplayInformationTab(int selectedTab)
		{
			try
			{
				TabRenderer renderTab = _renderers[selectedTab];

				if(renderTab != null)
				{
					renderTab();
				}
			}
			catch//(Exception ex)
			{
				//Log.Error("Error displaying tab", ex);
			}
		}

		private void DisplayInnerExceptionTrace()
		{
			if(false == _hasInnerExceptionBeenCalled)
			{
				Exception innerEx = _targetEx;
				TreeNode parentNode = null, 
					childNode = null, childMessage = null,
					childTarget = null;

				this._innerExceptionsTreeView.BeginUpdate();

				while(null != innerEx)
				{
					childNode = new TreeNode(innerEx.GetType().FullName);
					childMessage = new TreeNode(innerEx.Message);
					childTarget = new TreeNode(this.GetTargetMethodFormat(innerEx.TargetSite));
				
					childNode.Nodes.Add(childMessage);
					childNode.Nodes.Add(childTarget);

					if(null != parentNode)
					{
						parentNode.Nodes.Add(childNode);
					}
					else
					{
						_innerExceptionsTreeView.Nodes.Add(childNode);	
					}

					parentNode = childNode;
					innerEx = innerEx.InnerException;
				}

				_innerExceptionsTreeView.EndUpdate();
				_hasInnerExceptionBeenCalled = true;
			}
		}

		private void DisplayOtherInformation()
		{
			if(false == _hasOtherInformationBeenCalled)
			{
				Hashtable ht = this.GetCustomExceptionInfo(_targetEx);
				IDictionaryEnumerator ide = ht.GetEnumerator();
			
				_otherInformationList.Items.Clear();
				_otherInformationList.BeginUpdate();

				ListViewItem lvi;

				while(ide.MoveNext())
				{
					lvi = new ListViewItem(ide.Key.ToString());
					
					if(null != ide.Value)
					{
						lvi.SubItems.Add(ide.Value.ToString());
					}
					
					_otherInformationList.Items.Add(lvi);
				}

				_otherInformationList.EndUpdate();
				_hasOtherInformationBeenCalled = true;
			}
		}

		private void DisplayStackTrace()
		{
			if(false == _hasStackTraceBeenCalled)
			{
				StackTrace exTrace = new StackTrace(_targetEx);
				
				for(int i = 0; i < exTrace.FrameCount - 1; i++)
				{
					StackFrame exFrame = exTrace.GetFrame(i);
					_stackTraceList.Items.Add(
						this.GetTargetMethodFormat(exFrame.GetMethod()));
				}
			
				_hasStackTraceBeenCalled = true;
			}
		}

		private Hashtable GetCustomExceptionInfo(Exception Ex)
		{
			Hashtable customInfo = new Hashtable();
			
			foreach(PropertyInfo pi in Ex.GetType().GetProperties())
			{
				Type baseEx = typeof(System.Exception);

				if(null == baseEx.GetProperty(pi.Name))
				{
					customInfo.Add(pi.Name, pi.GetValue(Ex, null));
				}
			}

			return customInfo;
		}

		private string GetTargetMethodFormat(MethodBase method)
		{
			StringBuilder methodFormat = new StringBuilder();

			methodFormat.Append("[").Append(method.DeclaringType.Assembly.GetName().Name)
				.Append("]").Append(method.DeclaringType).Append("::").Append(method.Name)
				.Append("(");

			ParameterInfo[] methodParams = method.GetParameters();

			for(int i = 0; i < methodParams.Length; i++)
			{
				ParameterInfo methodParam = methodParams[i];
				methodFormat.Append(methodParam.ParameterType.FullName);

				if(i < methodParams.Length - 1)
				{
					methodFormat.Append(", ");
				}
			}

			methodFormat.Append(")");

			return methodFormat.ToString();
		}

		private void OnCopyButtonClick(object sender, System.EventArgs e)
		{
			Clipboard.SetDataObject(this.ToString(), true);
		}

		private void OnExceptionDialogLoad(object sender, System.EventArgs e)
		{
			if(null != this._targetEx)
			{
				_exceptionTypeValueText.Text = this._targetEx.GetType().FullName;
				this.DisplayInformationTab((int)TabToRender.General);
			}
			else
			{
				_informationTab.Enabled = false;
				_exceptionTypeValueText.Text = "Unknown";
			}
		}

		private void OnInformationTabSelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.DisplayInformationTab(_informationTab.SelectedIndex);
		}

		public override string ToString()
		{
			StringBuilder formattedException = new StringBuilder();

			if (_targetEx != null)
			{
				formattedException.Append("EXCEPTION INFORMATION").Append(Environment.NewLine)
					.Append(Environment.NewLine)
					.Append("Date/Time: ").Append(DateTime.Now.ToString("F")).Append(Environment.NewLine)
					.Append("Type: ").Append(_targetEx.GetType().FullName).Append(Environment.NewLine)
					.Append("Message: ").Append(_targetEx.Message).Append(Environment.NewLine)
					.Append("Source: ").Append(_targetEx.Source).Append(Environment.NewLine)
					.Append("Target Method: ")
					.Append(this.GetTargetMethodFormat(_targetEx.TargetSite))
					.Append(Environment.NewLine).Append(Environment.NewLine)
					.Append("Call Stack:").Append(Environment.NewLine);

				StackTrace exceptionStack = new StackTrace(_targetEx);

				for(int i = 0; i < exceptionStack.FrameCount; i++)
				{
					StackFrame exceptionFrame = exceptionStack.GetFrame(i);

					formattedException.Append("\t").Append("Method Name: ").Append(this.GetTargetMethodFormat(exceptionFrame.GetMethod())).Append(Environment.NewLine)
                        .Append("\t").Append("\t").Append("File Name: ").Append(exceptionFrame.GetFileName()).Append(Environment.NewLine)
                        .Append("\t").Append("\t").Append("Column: ").Append(exceptionFrame.GetFileColumnNumber()).Append(Environment.NewLine)
                        .Append("\t").Append("\t").Append("Line: ").Append(exceptionFrame.GetFileLineNumber()).Append(Environment.NewLine)
                        .Append("\t").Append("\t").Append("CIL Offset: ").Append(exceptionFrame.GetILOffset()).Append(Environment.NewLine)
                        .Append("\t").Append("\t").Append("Native Offset: ").Append(exceptionFrame.GetNativeOffset()).Append(Environment.NewLine)
						.Append(Environment.NewLine);
				}

				formattedException.Append("Inner Exception(s)").Append(Environment.NewLine);

				Exception innerEx = _targetEx.InnerException;

				while (innerEx != null)
				{
					formattedException.Append("\t").Append("Exception: ")
						.Append(innerEx.GetType().FullName).Append(Environment.NewLine);
					innerEx = innerEx.InnerException;
				}	

				formattedException.Append(Environment.NewLine).Append("Custom Properties")
					.Append(Environment.NewLine);

				Type baseEx = typeof(Exception);

				foreach(PropertyInfo propInfo in _targetEx.GetType().GetProperties())
				{
					if(baseEx.GetProperty(propInfo.Name) == null)
					{
						formattedException.Append("\t").Append(propInfo.Name).Append(": ")
							.Append(propInfo.GetValue(_targetEx, null))
							.Append(Environment.NewLine);
					}
				}
			}

			return formattedException.ToString();
		}
	}
}
