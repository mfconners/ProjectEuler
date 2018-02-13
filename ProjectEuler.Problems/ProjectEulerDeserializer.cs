using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace ProjectEuler.Problems
{
	internal class ProjectEulerDeserializer
	{
		private Dictionary<int, ProblemMetaData> problem_data;
		private static ProjectEulerDeserializer _resource_data = new ProjectEulerDeserializer(Properties.Resources.ProjectEulerAllProblems);
		public static ProjectEulerDeserializer ResourceProblemData { get { return _resource_data; } }

		public struct ProblemMetaData
		{
			public int ProblemNumber;
			public string ProblemName;
			public DateTime PublishedDate;
			public int SolutionCount;
			public int DifficultyRating;
			public string ProblemContents;
		}

		public ProjectEulerDeserializer(string allproblems)
		{
			problem_data = new Dictionary<int, ProblemMetaData>();

			XmlDocument allproblems_xml = new XmlDocument();

			{
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.DtdProcessing = DtdProcessing.Parse;
				settings.IgnoreWhitespace = true;
				XmlReader xml_reader = XmlReader.Create(new StringReader(allproblems.Replace(" async ", " ")), settings);

				allproblems_xml.Load(xml_reader);
				xml_reader.Close();
			}

			XmlNode toplevel = allproblems_xml;
			XmlNode content_node = null;
			for (int i = 0; i < toplevel.ChildNodes.Count && content_node == null; ++i)
			{
				if (toplevel.ChildNodes[i].NodeType == XmlNodeType.Element)
				{
					if (toplevel.ChildNodes[i].Name == "html" || toplevel.ChildNodes[i].Name == "body")
					{
						toplevel = toplevel.ChildNodes[i];
						i = -1;
					}
					else if (toplevel.ChildNodes[i].Name == "div")
					{
						for (int j = 0; j < toplevel.ChildNodes[i].Attributes.Count && content_node == null; ++j)
						{
							if (toplevel.ChildNodes[i].Attributes[j].Name == "id")
							{
								if (toplevel.ChildNodes[i].Attributes[j].Value == "container")
								{
									toplevel = toplevel.ChildNodes[i];
									i = -1;
									break;
								}
								else if (toplevel.ChildNodes[i].Attributes[j].Value == "content")
								{
									content_node = toplevel.ChildNodes[i];
									break;
								}
							}
						}
					}
				}
			}

			for (int i = 0; i < content_node.ChildNodes.Count; ++i)
			{
				XmlNode problemNode = content_node.ChildNodes[i];
				if (problemNode.NodeType == XmlNodeType.Element && problemNode.Name == "div" && problemNode.FirstChild != problemNode.LastChild)
				{
					if (problemNode.FirstChild.Attributes["class"].Value == "info" && problemNode.LastChild.Attributes["class"].Value == "problem_content")
					{
						XmlNode problemData = problemNode.FirstChild;
						while (problemData.FirstChild != null && problemData.Name != "a")
						{
							problemData = problemData.FirstChild;
						}

						XmlNode problemDescription = problemNode.LastChild;

						if (problemData.Name == "a" && problemData.Attributes["href"] != null)
						{
							ProblemMetaData problem = new ProblemMetaData();

							problem.ProblemNumber = Convert.ToInt32(problemData.Attributes["href"].Value.Replace(Properties.Resources.ProblemNumberToken, String.Empty));

							if (problemData.FirstChild != null && problemData.FirstChild.NodeType == XmlNodeType.Text)
							{
								int name_start = problemData.FirstChild.Value.IndexOf(':');
								if (name_start > 0)
								{
									problem.ProblemName = problemData.FirstChild.Value.Substring(name_start + 2);
								}
							}
							else
							{
								problem.ProblemName = String.Empty;
							}

							problem.PublishedDate = new DateTime();
							problem.SolutionCount = -1;
							problem.DifficultyRating = -1;

							if (problemData.LastChild != null && problemData.LastChild.NodeType == XmlNodeType.Element)
							{
								if (problemData.LastChild.FirstChild != null && problemData.LastChild.FirstChild.NodeType == XmlNodeType.Text)
								{
									string[] metadata = problemData.LastChild.FirstChild.Value.Split(';');

									for (int j = 0; j < metadata.Length; ++j)
									{
										if (metadata[j].Replace(Properties.Resources.SolutionCountToken, "") != metadata[j])
										{
											problem.SolutionCount = Convert.ToInt32(metadata[1].Replace(Properties.Resources.SolutionCountToken, String.Empty));
										}
										else if (metadata[j].Replace(Properties.Resources.DifficultyRatingToken, "") != metadata[j])
										{
											string rating = metadata[2].Replace(Properties.Resources.DifficultyRatingToken, String.Empty);
											int percent_pos = rating.IndexOf('%');
											if (percent_pos > 0)
											{
												rating = rating.Substring(0, percent_pos);
											}
											problem.DifficultyRating = Convert.ToInt32(rating);
										}
									}
								}
							}
							problem.ProblemContents = problemDescription.InnerXml;

							problem_data.Add(problem.ProblemNumber, problem);
						}
					}
				}
			}
		}

		public ProblemMetaData GetProblemMetaData(int problem_number)
		{
			if (problem_data.ContainsKey(problem_number))
			{
				return problem_data[problem_number];
			}
			ProblemMetaData meta = new ProblemMetaData();
			meta.ProblemNumber = -1;
			meta.ProblemName = String.Empty;
			meta.PublishedDate = new DateTime();
			meta.SolutionCount = -1;
			meta.DifficultyRating = -1;
			meta.ProblemContents = String.Empty;

			return meta;
		}
	}
}
