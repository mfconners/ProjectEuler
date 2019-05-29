using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ProjectEuler.Problems;

namespace ProjectEuler
{
	public partial class ProjectEulerForm : Form
	{
		// TODO this.ResizeEnd += new System.EventHandler(this.ProjectEulerForm_Resize);
		private HashSet<Thread> threads = new HashSet<Thread>();
		private HashSet<int> RowsBeingWorked = new HashSet<int>();
		private Queue<int> RowsQueue = new Queue<int>();
		private int TotalQueueCount = 0;
		private Dictionary<int, Problem> RowsProblems = new Dictionary<int, Problem>();
		private Dictionary<int, int> ProblemsRows = new Dictionary<int, int>();
		private SemaphoreSlim _manageWorkers = new SemaphoreSlim(1);
		private DateTime _start;
		private TimeSpan _runningTime = TimeSpan.Zero;
		public TimeSpan RunningTime
		{
			get { return _runningTime; }
			private set { _runningTime = value; }
		}

		System.Windows.Forms.DataGridViewCellStyle untestedAnswerStyle;
		System.Windows.Forms.DataGridViewCellStyle wrongAnswerStyle;
		System.Windows.Forms.DataGridViewCellStyle correctAnswerStyle;
		System.Windows.Forms.DataGridViewCellStyle newAnswerStyle;

		public ProjectEulerForm()
		{
			InitializeComponent();
			TimeLabel.Text = Problem.TotalSolutionTime;

			untestedAnswerStyle = new DataGridViewCellStyle();
			untestedAnswerStyle.BackColor = Color.Gold;
			untestedAnswerStyle.SelectionBackColor = Color.DeepSkyBlue;

			wrongAnswerStyle = new DataGridViewCellStyle();
			wrongAnswerStyle.BackColor = Color.Red;
			wrongAnswerStyle.SelectionBackColor = Color.SlateBlue;

			correctAnswerStyle = new DataGridViewCellStyle();
			correctAnswerStyle.BackColor = Color.White;
			correctAnswerStyle.SelectionBackColor = Color.Blue;

			newAnswerStyle = new DataGridViewCellStyle();
			newAnswerStyle.BackColor = Color.LawnGreen;
			newAnswerStyle.SelectionBackColor = Color.LightSeaGreen;
		}

		private void ProjectEulerForm_Load(object sender, EventArgs e)
		{
			int row = 0;
			foreach (Problem problem in Problem.NumberedProblems.Values)
			{
				this.problemBindingSource.Add(problem);
				ProblemsRows.Add(problem.ProblemNumber, row);
				if (problem.HasSolutionAttempt)
				{
					ProblemsDataGridView.Rows[row].DefaultCellStyle = untestedAnswerStyle;
				}
				RowsProblems.Add(row++, problem);
			}
		}

		private void RestartWorkersIfNecessary()
		{
			_manageWorkers.Wait();

			while (threads.Count < Environment.ProcessorCount || threads.Count < 1)
			{
				Thread problem_solver = new Thread(problemSolver_DoWork);
				problem_solver.IsBackground = true;
				problem_solver.Priority = ThreadPriority.BelowNormal;
				threads.Add(problem_solver);

				_start = DateTime.Now;
				problem_solver.Start();
			}

			_manageWorkers.Release();
		}

		private void SolveAllProblemsButton_Click(object sender, EventArgs e)
		{
			_manageWorkers.Wait();

			for (int row = 0; row < ProblemsDataGridView.Rows.Count; ++row)
			{
				string correctAnswer = ProblemsDataGridView["CorrectAnswerColumn", row].Value.ToString();
				if (string.IsNullOrEmpty(correctAnswer) || correctAnswer == Problem.SolutionUnknown)
				{
					RowsQueue.Enqueue(row);
				}
			}

			TotalQueueCount -= RowsQueue.Count;
			//for (int row = ProblemsDataGridView.Rows.Count - 1; row >= 0; --row)
			for (int row = 0; row < ProblemsDataGridView.Rows.Count; ++row)
			{
				RowsQueue.Enqueue(row);
			}
			TotalQueueCount += RowsQueue.Count;

			_manageWorkers.Release();

			RestartWorkersIfNecessary();
		}

		private void UpdateUIAfterSolution(int row)
		{
			BeginInvoke((MethodInvoker)delegate
								 {
									 this.UpdateRowFormatting(row);
								 });
		}

		void UpdateRowFormatting(int row)
		{
			for (int i = 0; i < this.ProblemsDataGridView.Columns.Count; ++i)
			{
				this.ProblemsDataGridView.UpdateCellValue(i, row);
			}

			int problemNumber = Convert.ToInt32(ProblemsDataGridView["ProblemNumberColumn", row].Value);

			if (Problem.NumberedProblems.ContainsKey(problemNumber))
			{
				Problem problem = (Problem)Problem.NumberedProblems[problemNumber];

				if (string.IsNullOrEmpty(problem.Answer))
				{
				}
				else if (problem.CorrectAnswer == String.Empty || problem.CorrectAnswer == Problem.SolutionUnknown)
				{
					if (problem.Answer == String.Empty || problem.Answer == Problem.SolutionUnknown)
					{
						this.ProblemsDataGridView.Rows[row].DefaultCellStyle = correctAnswerStyle;
					}
					else
					{
						this.ProblemsDataGridView.Rows[row].DefaultCellStyle = newAnswerStyle;
					}
				}
				else if (problem.Answer != problem.CorrectAnswer)
				{
					this.ProblemsDataGridView.Rows[row].DefaultCellStyle = wrongAnswerStyle;
				}
				else
				{
					this.ProblemsDataGridView.Rows[row].DefaultCellStyle = correctAnswerStyle;
				}
			}

			this.ProblemsDataGridView.Update();
			this.TimeLabel.Text = Problem.TotalSolutionTime;
		}

		private void problemSolver_DoWork()
		{
			int row = -1;
			Problem problem = null;

			_manageWorkers.Wait();

			#region Select a problem
			while (problem == null && RowsQueue.Count > 0)
			{
				row = RowsQueue.Dequeue();
				problem = RowsProblems[row];

				if (RowsBeingWorked.Contains(row) || !string.IsNullOrEmpty(problem.Answer) || !problem.HasSolutionAttempt)
				{
					row = ProblemsDataGridView.RowCount;
					problem = null;
				}
			}
			#endregion

			if (problem != null)
			{
				RowsBeingWorked.Add(row);

				_manageWorkers.Release();


				Thread.CurrentThread.Name = problem.GetType().Name;

				problem.Solve();

				if (problem != null)
					RunningTime = DateTime.Now - _start;

				if (row >= 0 && row < ProblemsDataGridView.RowCount)
					UpdateUIAfterSolution(row);


				_manageWorkers.Wait();

				RowsBeingWorked.Remove(row);
			}
			//else if (RowsBeingWorked.Count > 0)
			//{
			//	{
			//		HashSet<int> rowsBeingWorked = new HashSet<int>(RowsBeingWorked);
			//		_manageWorkers.Release();
			//		foreach (int assistRow in rowsBeingWorked)
			//		{
			//			Problem assistProblem = RowsProblems[assistRow];
			//			assistProblem.AssistCalculateSolution();
			//		}
			//		_manageWorkers.Wait();
			//	}
			//}


			if (RowsQueue.Count > 0)
			{
				Thread problem_solver = new Thread(problemSolver_DoWork);
				problem_solver.IsBackground = true;
				problem_solver.Priority = ThreadPriority.BelowNormal;
				threads.Add(problem_solver);
				problem_solver.Start();
			}

			threads.Remove(Thread.CurrentThread);

			_manageWorkers.Release();
		}
	}
}
