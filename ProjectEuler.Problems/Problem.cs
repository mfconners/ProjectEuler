using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ProjectEuler.Problems
{
	public class Problem
	{
		#region Properties
		private static SortedList _numberedProblems = InitNumberedProblems();
		public static SortedList NumberedProblems
		{
			get { return _numberedProblems; }
		}

		private int _problemNumber = 0;
		public int ProblemNumber
		{
			get { return _problemNumber; }
			private set
			{
				if (_problemNumber != 0)
					throw new InvalidOperationException("This problem already has a number: " + _problemNumber + ", " + value);

				_problemNumber = value;
			}
		}

		private string _name = String.Empty;
		public virtual string Name { get { return _name; } }

		private string _description = String.Empty;
		public virtual string Description { get { return _description; } }

		public bool HasSolutionAttempt { get { return this.GetType() != typeof(Problem); } }

		static private SortedList InitNumberedProblems()
		{
			SortedList probs = new SortedList();

			AppDomain domain = AppDomain.CurrentDomain;
			foreach (Assembly assembly in domain.GetAssemblies())
			{
				foreach (Type possibleProblemType in assembly.GetTypes())
				{
					if (possibleProblemType.IsSubclassOf(typeof(Problem)) &&
							!possibleProblemType.IsAbstract &&
							possibleProblemType.GetConstructor(Type.EmptyTypes) != null)
					{
						Problem prob = (Problem)Activator.CreateInstance(possibleProblemType);
						prob.ProblemNumber = Convert.ToInt32(prob.GetType().Name.Replace("Problem", ""));
						if (probs.ContainsKey(prob.ProblemNumber))
							throw new InvalidOperationException("Multiple problems with the same number: " + prob.ProblemNumber);

						ProjectEulerDeserializer.ProblemMetaData meta = ProjectEulerDeserializer.ResourceProblemData.GetProblemMetaData(prob.ProblemNumber);
						prob._name = meta.ProblemName;
						prob._description = meta.ProblemContents;

						probs[prob.ProblemNumber] = prob;
					}
				}
			}

			//  ##TODO## This seems to have a negative impact on the Windows Forms performance... Need to fix it.
			for (int i = 1; 0 == 1 && i < probs.Count + 5; ++i)
			{
				if (!probs.ContainsKey(i))
				{
					ProjectEulerDeserializer.ProblemMetaData meta = ProjectEulerDeserializer.ResourceProblemData.GetProblemMetaData(i);
					if (meta.ProblemNumber > 0)
					{
						Problem prob = new Problem();
						prob.ProblemNumber = meta.ProblemNumber;
						prob._name = meta.ProblemName;
						prob._description = meta.ProblemContents;

						probs[prob.ProblemNumber] = prob;
					}
				}
			}

			return probs;
		}

		public string Answer { get; private set; }
		public virtual string CorrectAnswer { get { return String.Empty; } }

		private TimeSpan _solutionTime = TimeSpan.Zero;
		public string SolutionTime
		{
			get
			{
				string formatted = string.Format("{0}{1}{2}{3}",
						_solutionTime.TotalDays > 1.0 ? string.Format("{0:0} d, ", _solutionTime.Days) : string.Empty,
						_solutionTime.TotalHours > 1.0 ? string.Format("{0:0} h, ", _solutionTime.Hours) : string.Empty,
						_solutionTime.TotalMinutes > 1.0 ? string.Format("{0:0} m, ", _solutionTime.Minutes) : string.Empty,
						string.Format("{0:0.0000} s", (_solutionTime.Ticks * 1.0 / TimeSpan.TicksPerSecond) % 60));

				return formatted;
			}
		}

		public static string TotalSolutionTime
		{
			get
			{
				TimeSpan _totalSolutionTime = TimeSpan.Zero;
				foreach (Problem problem in _numberedProblems.Values)
					_totalSolutionTime += problem._solutionTime;

				string formatted = string.Format("{0}{1}{2}{3}",
						_totalSolutionTime.TotalDays > 1.0 ? string.Format("{0:0} d, ", _totalSolutionTime.Days) : string.Empty,
						_totalSolutionTime.TotalHours > 1.0 ? string.Format("{0:0} h, ", _totalSolutionTime.Hours) : string.Empty,
						_totalSolutionTime.TotalMinutes > 1.0 ? string.Format("{0:0} m, ", _totalSolutionTime.Minutes) : string.Empty,
						string.Format("{0:0.0000} s", (_totalSolutionTime.Ticks * 1.0 / TimeSpan.TicksPerSecond) % 60));

				return formatted;
			}
		}
		#endregion


		protected Problem()
		{
			Answer = string.Empty;
		}

		public const string SolutionUnknown = "UNKNOWN";
		protected virtual string CalculateSolution() { return SolutionUnknown; }
		virtual public void AssistCalculateSolution() { }

		public void Solve()
		{
			if (string.IsNullOrEmpty(Answer))
			{
				/* TODO Update the measure of solution time to the use of processor time.
				Process myProcess = new Process();
				Thread myThread = Thread.CurrentThread;
				ProcessThread myProcessThread = null;
				foreach (ProcessThread pt in myProcess.Threads)
						if (pt.Id == myThread.ManagedThreadId)
						{
								myProcessThread = pt;
								break;
						}
				*/
				Stopwatch watch = new Stopwatch();
				watch.Start();
				//TimeSpan procTime = TimeSpan.Zero;

				//if (myProcessThread != null)
				//    procTime = myProcessThread.TotalProcessorTime;

				Answer = CalculateSolution();

				//if (myProcessThread == null)
				watch.Stop();
				_solutionTime = watch.Elapsed;
				//else
				//    _solutionTime = myProcessThread.TotalProcessorTime - procTime;
			}
		}
	}
}
