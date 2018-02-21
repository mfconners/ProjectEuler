using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using ProjectEuler.MathExtensions;
using ProjectEuler.Problems;

namespace ProjectEuler
{
	partial class ProjectEulerForm
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

			_manageWorkers.Wait();
			_manageWorkers.Dispose();
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewDefaultColumnHeaderCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewRightAlignedDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewLeftAlignedWrappedDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			this.ProjectEulerTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.TotalRunningTimeLabel = new System.Windows.Forms.Label();
			this.SolveAllProblemsButton = new System.Windows.Forms.Button();
			this.TotalProcessorTimeLabel = new System.Windows.Forms.Label();
			this.TimeLabel = new System.Windows.Forms.Label();
			this.ProblemsDataGridView = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.problemBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			this.ProblemNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AnswerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CorrectAnswerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SolutionTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.problemBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.ProjectEulerTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ProblemsDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.problemBindingSource1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.problemBindingSource)).BeginInit();
			this.SuspendLayout();
			//
			// ProjectEulerTableLayoutPanel
			//
			this.ProjectEulerTableLayoutPanel.AutoSize = true;
			this.ProjectEulerTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ProjectEulerTableLayoutPanel.ColumnCount = 5;
			this.ProjectEulerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.ProjectEulerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
			this.ProjectEulerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
			this.ProjectEulerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.ProjectEulerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
			this.ProjectEulerTableLayoutPanel.Controls.Add(this.TotalRunningTimeLabel, 1, 0);
			this.ProjectEulerTableLayoutPanel.Controls.Add(this.SolveAllProblemsButton, 0, 0);
			this.ProjectEulerTableLayoutPanel.Controls.Add(this.TotalProcessorTimeLabel, 3, 0);
			this.ProjectEulerTableLayoutPanel.Controls.Add(this.TimeLabel, 4, 0);
			this.ProjectEulerTableLayoutPanel.Controls.Add(this.ProblemsDataGridView, 0, 1);
			this.ProjectEulerTableLayoutPanel.Controls.Add(this.label1, 2, 0);
			this.ProjectEulerTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProjectEulerTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
			this.ProjectEulerTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.ProjectEulerTableLayoutPanel.Name = "ProjectEulerTableLayoutPanel";
			this.ProjectEulerTableLayoutPanel.RowCount = 2;
			this.ProjectEulerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.ProjectEulerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.ProjectEulerTableLayoutPanel.Size = new System.Drawing.Size(577, 501);
			this.ProjectEulerTableLayoutPanel.TabIndex = 0;
			//
			// TotalRunningTimeLabel
			//
			this.TotalRunningTimeLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.TotalRunningTimeLabel.AutoSize = true;
			this.TotalRunningTimeLabel.Location = new System.Drawing.Point(203, 8);
			this.TotalRunningTimeLabel.Name = "TotalRunningTimeLabel";
			this.TotalRunningTimeLabel.Size = new System.Drawing.Size(106, 13);
			this.TotalRunningTimeLabel.TabIndex = 5;
			this.TotalRunningTimeLabel.Text = ProjectEuler.Properties.Resources.TotalRunningTime;
			//
			// SolveAllProblemsButton
			//
			this.SolveAllProblemsButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.SolveAllProblemsButton.AutoSize = true;
			this.SolveAllProblemsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.SolveAllProblemsButton.Location = new System.Drawing.Point(3, 3);
			this.SolveAllProblemsButton.Name = "SolveAllProblemsButton";
			this.SolveAllProblemsButton.Size = new System.Drawing.Size(104, 23);
			this.SolveAllProblemsButton.TabIndex = 0;
			this.SolveAllProblemsButton.Text = ProjectEuler.Properties.Resources.SolveAllPrimes;
			this.SolveAllProblemsButton.UseVisualStyleBackColor = true;
			this.SolveAllProblemsButton.Click += new System.EventHandler(this.SolveAllProblemsButton_Click);
			//
			// TotalProcessorTimeLabel
			//
			this.TotalProcessorTimeLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.TotalProcessorTimeLabel.AutoSize = true;
			this.TotalProcessorTimeLabel.Location = new System.Drawing.Point(401, 2);
			this.TotalProcessorTimeLabel.Name = "TotalProcessorTimeLabel";
			this.TotalProcessorTimeLabel.Size = new System.Drawing.Size(84, 26);
			this.TotalProcessorTimeLabel.TabIndex = 1;
			this.TotalProcessorTimeLabel.Text = ProjectEuler.Properties.Resources.TotalProcessorTime;
			//
			// TimeLabel
			//
			this.TimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TimeLabel.AutoSize = true;
			this.TimeLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.problemBindingSource1, "SolutionTime", true, System.Windows.Forms.DataSourceUpdateMode.Never, "0.0", "0.000s"));
			this.TimeLabel.Location = new System.Drawing.Point(491, 8);
			this.TimeLabel.Name = "TimeLabel";
			this.TimeLabel.Size = new System.Drawing.Size(83, 13);
			this.TimeLabel.TabIndex = 2;
			this.TimeLabel.Text = ProjectEuler.Properties.Resources.ZeroPointZero;
			//
			// ProblemsDataGridView
			//
			this.ProblemsDataGridView.AllowUserToAddRows = false;
			this.ProblemsDataGridView.AllowUserToDeleteRows = false;
			this.ProblemsDataGridView.AllowUserToOrderColumns = true;
			this.ProblemsDataGridView.AllowUserToResizeRows = false;
			this.ProblemsDataGridView.AutoGenerateColumns = false;
			this.ProblemsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridViewDefaultColumnHeaderCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
			dataGridViewDefaultColumnHeaderCellStyle.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewDefaultColumnHeaderCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewDefaultColumnHeaderCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewDefaultColumnHeaderCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewDefaultColumnHeaderCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			this.ProblemsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewDefaultColumnHeaderCellStyle;
			this.ProblemsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ProblemsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.ProblemNumberColumn,
			this.NameColumn,
			this.AnswerColumn,
			this.CorrectAnswerColumn,
			this.SolutionTimeColumn});
			this.ProjectEulerTableLayoutPanel.SetColumnSpan(this.ProblemsDataGridView, 5);
			this.ProblemsDataGridView.DataSource = this.problemBindingSource;
			dataGridViewDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
			dataGridViewDefaultCellStyle.BackColor = Color.LightGray;
			dataGridViewDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewDefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewDefaultCellStyle.SelectionBackColor = Color.LightSlateGray;
			dataGridViewDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.ProblemsDataGridView.DefaultCellStyle = dataGridViewDefaultCellStyle;
			this.ProblemsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProblemsDataGridView.Location = new System.Drawing.Point(3, 33);
			this.ProblemsDataGridView.Name = "ProblemsDataGridView";
			this.ProblemsDataGridView.ReadOnly = true;
			this.ProblemsDataGridView.RowHeadersVisible = false;
			this.ProblemsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ProblemsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ProblemsDataGridView.Size = new System.Drawing.Size(571, 465);
			this.ProblemsDataGridView.TabIndex = 3;
			//
			// label1
			//
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(315, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = ProjectEuler.Properties.Resources.ZeroPointZero;
			//
			// problemBindingSource1
			//
			this.problemBindingSource1.DataSource = typeof(Problem);
			//
			// ProblemNumberColumn
			//
			this.ProblemNumberColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ProblemNumberColumn.DataPropertyName = "ProblemNumber";
			dataGridViewRightAlignedDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
			this.ProblemNumberColumn.DefaultCellStyle = dataGridViewRightAlignedDefaultCellStyle;
			this.ProblemNumberColumn.HeaderText = ProjectEuler.Properties.Resources.Hash;
			this.ProblemNumberColumn.Name = "ProblemNumberColumn";
			this.ProblemNumberColumn.ReadOnly = true;
			this.ProblemNumberColumn.Width = 39;
			//
			// NameColumn
			//
			this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.NameColumn.DataPropertyName = "Name";
			dataGridViewLeftAlignedWrappedDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewLeftAlignedWrappedDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.NameColumn.DefaultCellStyle = dataGridViewLeftAlignedWrappedDefaultCellStyle;
			this.NameColumn.HeaderText = ProjectEuler.Properties.Resources.Name;
			this.NameColumn.Name = "NameColumn";
			this.NameColumn.ReadOnly = true;
			this.NameColumn.Width = 60;
			//
			// AnswerColumn
			//
			this.AnswerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.AnswerColumn.DataPropertyName = "Answer";
			this.AnswerColumn.HeaderText = ProjectEuler.Properties.Resources.Answer;
			this.AnswerColumn.Name = "AnswerColumn";
			this.AnswerColumn.ReadOnly = true;
			this.AnswerColumn.Width = 67;
			//
			// CorrectAnswerColumn
			//
			this.CorrectAnswerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.CorrectAnswerColumn.DataPropertyName = "CorrectAnswer";
			this.CorrectAnswerColumn.HeaderText = ProjectEuler.Properties.Resources.CorrectAnswer;
			this.CorrectAnswerColumn.Name = "CorrectAnswerColumn";
			this.CorrectAnswerColumn.ReadOnly = true;
			this.CorrectAnswerColumn.Width = 104;
			//
			// SolutionTimeColumn
			//
			this.SolutionTimeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.SolutionTimeColumn.DataPropertyName = "SolutionTime";
			this.SolutionTimeColumn.DefaultCellStyle = dataGridViewRightAlignedDefaultCellStyle;
			this.SolutionTimeColumn.HeaderText = ProjectEuler.Properties.Resources.SolutionTime;
			this.SolutionTimeColumn.Name = "SolutionTimeColumn";
			this.SolutionTimeColumn.ReadOnly = true;
			this.SolutionTimeColumn.Width = 96;
			//
			// problemBindingSource
			//
			this.problemBindingSource.AllowNew = false;
			this.problemBindingSource.DataSource = typeof(Problem);
			//
			// ProjectEulerForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(577, 501);
			this.Controls.Add(this.ProjectEulerTableLayoutPanel);
			this.MinimumSize = new System.Drawing.Size(500, 325);
			this.Name = "ProjectEulerForm";
			this.Text = ProjectEuler.Properties.Resources.ProjectEulerProblems;
			this.Load += new System.EventHandler(this.ProjectEulerForm_Load);
			this.ProjectEulerTableLayoutPanel.ResumeLayout(false);
			this.ProjectEulerTableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ProblemsDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.problemBindingSource1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.problemBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BindingSource problemBindingSource;
		private TableLayoutPanel ProjectEulerTableLayoutPanel;
		private Button SolveAllProblemsButton;
		private Label TotalProcessorTimeLabel;
		private Label TimeLabel;
		private DataGridView ProblemsDataGridView;
		private DataGridViewTextBoxColumn ProblemNumberColumn;
		private DataGridViewTextBoxColumn NameColumn;
		private DataGridViewTextBoxColumn AnswerColumn;
		private DataGridViewTextBoxColumn CorrectAnswerColumn;
		private DataGridViewTextBoxColumn SolutionTimeColumn;
		private Label TotalRunningTimeLabel;
		private Label label1;
		private BindingSource problemBindingSource1;
	}
}

