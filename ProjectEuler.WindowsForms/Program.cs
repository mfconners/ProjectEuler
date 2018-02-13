using System;
using System.Resources;
using System.Windows.Forms;

[assembly: NeutralResourcesLanguageAttribute("en")]
[assembly: CLSCompliant(true)]

namespace ProjectEuler
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ProjectEulerForm());
		}
	}
}
