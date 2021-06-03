﻿using RPGCore.Documentation.Internal;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace RPGCore.Documentation
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			string[] loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
			string[] referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
			var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
			toLoad.ForEach(path =>
			{
				try
				{
					loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path)));
				}
				catch
				{

				}
			});

			var directory = FindSourceDirectory();
			string basePath = Path.Combine(directory.FullName, "src/libs/RPGCore.Documentation/Samples");
			foreach (string file in Directory.GetFiles(basePath, "*", SearchOption.AllDirectories))
			{
				ExportSvgSample(basePath, file);
			}
		}

		private static void ExportHtmlSample(string filename)
		{
			var sampleFile = GetSampleFile($"{filename}.cs");

			string sampleContent = File.ReadAllText(sampleFile.FullName);

			foreach (var builder in SampleParser.HtmlHighlight(sampleContent))
			{
				var destination = builder.Name == ""
					? GetDestinationFile($"{filename}.html")
					: GetDestinationFile($"{filename}.{builder.Name}.html");

				destination.Directory?.Create();
				destination.Delete();

				using var fs = destination.OpenWrite();
				using var sw = new StreamWriter(fs);

				sw.Write($@"<!DOCTYPE html>
<html lang=""en"">
<head>
	<meta charset=""UTF-8"">
	<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
	<title>{filename} {builder.Name }</title>
	<link rel=""stylesheet"" asp-append-version=""true"" href=""../patio.min.css"" />
</head>
<body>
	<div class=""container"">");

				RenderToTable(sw, builder);

				sw.Write(@"</div></body></html>");
			}
		}

		public static void RenderToTable(StreamWriter output, SampleRegion region)
		{
			output.WriteLine("<table class=\"code-table\">");

			for (int i = 0; i < region.Lines.Length; i++)
			{
				var line = region.Lines[i];
				output.Write("\t<tr>\n\t\t<th>");
				output.Write(i + 1);
				output.Write("</th>\n\t\t<td>");

				foreach (var span in line)
				{
					if (span.Style == null)
					{
						output.Write(XmlEscape(span.Content));
					}
					else
					{
						output.Write("<span class=\"");
						output.Write(span.Style);
						output.Write("\">");
						output.Write(XmlEscape(span.Content));
						output.Write("</span>");
					}
				}
				output.Write("</td>\n\t</tr>\n");
			}
			output.WriteLine("</table>");
		}

		private static void ExportSvgSample(string basePath, string filePath)
		{
			var sampleFile = new FileInfo(filePath);

			string localPath = filePath[(basePath.Length + 1)..];
			string localName = localPath.Replace(sampleFile.Extension, "");

			string sampleContent = File.ReadAllText(sampleFile.FullName);

			foreach (var builder in SampleParser.HtmlHighlight(sampleContent))
			{
				if (builder.Name == "")
				{
					continue;
				}

				var destination = builder.Name == ""
					? GetDestinationFile($"{localName}.svg")
					: GetDestinationFile($"{localName}.{builder.Name}.svg");

				destination.Directory?.Create();
				destination.Delete();

				using var fs = destination.OpenWrite();
				using var sw = new StreamWriter(fs);

				RenderToSvgGraphic(sw, builder);
			}
		}

		public static void RenderToSvgGraphic(StreamWriter output, SampleRegion region)
		{
			double verticalPadding = 10.0;
			double characterWidth = 9.36;
			double lineHeight = 21.0;

			double totalHeight = (region.Lines.Length * lineHeight) + (verticalPadding * 1.5);

			output.Write(@"<svg viewBox=""0 0 1200 ");
			output.Write(totalHeight);
			output.Write(@""" width=""1200"" height=""");
			output.Write(totalHeight);
			output.Write(@""" xmlns =""http://www.w3.org/2000/svg"">

	<defs>
		<clipPath id=""round-left-corners"">
			<rect x=""0"" y=""0"" width=""100"" height=""100%"" rx=""8"" ry=""8""/>
		</clipPath>
	</defs>
	<style>
		.code { font: 17px Consolas; fill: rgba(220, 220, 220, 255); dominant-baseline: hanging; }
		.code-background { fill: #161b22; }
		.code-background-linenumber { fill: #111418; }
		.c-ln { font: 17px Consolas; fill: #2b90af; text-anchor: end; pointer-events: none; user-select: none; dominant-baseline: hanging; }
		.c-keyword { fill: rgba(86, 156, 214, 255); }
		.c-control { fill: rgba(216, 160, 223, 255); }
		.c-string { fill: rgba(214, 157, 133, 255); }
		.c-numeric { fill: rgba(181, 207, 168, 255); }
		.c-comment { fill: rgba(87, 166, 74, 255); }
		.c-class { fill: rgba(78, 201, 176, 255); }
		.c-interface { fill: rgba(184, 215, 163, 255); }
		.c-enum { fill: rgba(184, 215, 163, 255); }
		.c-structure { fill: rgba(134, 198, 145, 255); }
		.c-method { fill: rgba(220, 220, 170, 255); }
		.c-parameter { fill: rgba(156, 220, 254, 255); }
		.c-local { fill: rgba(156, 220, 254, 255); }
	</style>
	<rect x=""0"" y=""0"" width=""100%"" height=""100%"" rx=""8"" ry=""8"" class=""code-background"" />
	<rect x=""0"" y=""0"" width=""46"" height=""100%"" class=""code-background-linenumber"" clip-path=""url(#round-left-corners)"" />");

			for (int i = 0; i < region.Lines.Length; i++)
			{
				double yOffset = (i * lineHeight) + verticalPadding;
				output.Write($"\t<text x=\"38\" y=\"{yOffset}\" class=\"c-ln\">{i + 1}</text>\n");
			}

			for (int i = 0; i < region.Lines.Length; i++)
			{
				var line = region.Lines[i];

				double yOffset = (i * lineHeight) + verticalPadding;
				double indent = 64.0;
				bool wroteStart = false;
				for (int y = 0; y < line.Length; y++)
				{
					var span = line[y];

					if (string.IsNullOrWhiteSpace(span.Content) && y == 0)
					{
						indent += span.Content.Length * characterWidth;
						continue;
					}

					if (!wroteStart)
					{
						output.Write($"\t<text x=\"{indent}\" y=\"{yOffset}\" class=\"code\">");
						wroteStart = true;
					}

					if (span.Style == null)
					{
						output.Write(XmlEscape(span.Content));
					}
					else
					{
						output.Write("<tspan class=\"");
						output.Write(span.Style);
						output.Write("\">");
						output.Write(XmlEscape(span.Content));
						output.Write("</tspan>");
					}
				}
				if (wroteStart)
				{
					output.Write("</text>\n");
				}
			}
			output.WriteLine("</svg>");
		}


		public static string XmlEscape(string unescaped)
		{
			var doc = new XmlDocument();
			var node = doc.CreateElement("root");
			node.InnerText = unescaped;
			return node.InnerXml;
		}

		public static FileInfo GetSampleFile(string file)
		{
			var directory = FindSourceDirectory();
			string sampleFile = Path.Combine(directory.FullName, "src/libs/RPGCore.Documentation/Samples", file);
			return new FileInfo(sampleFile);
		}

		public static FileInfo GetDestinationFile(string file)
		{
			var directory = FindSourceDirectory();
			string sampleFile = Path.Combine(directory.FullName, "docs/samples", file);
			return new FileInfo(sampleFile);
		}

		private static DirectoryInfo FindSourceDirectory()
		{
			var directory = new DirectoryInfo(Environment.CurrentDirectory);
			while (directory != null)
			{
				if (directory.Name.Equals("RPGCore", StringComparison.OrdinalIgnoreCase))
				{
					return directory;
				}

				directory = directory.Parent;
			}
			return directory;
		}
	}
}
