using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Diagnostics;

public class OnionLogger {

	public static OnionLogger globalLog;

	/// The StreamWriter used to write to the logfile.
	private readonly StreamWriter writer;

	private const char indentChar = ' ';

	/// The number of indentChar to use for each indentation. Default value is 2.
	private readonly uint indentSize;
	
	/// The string to use for each indentation.
	private readonly string indentationString;
	
	private readonly Stack<LogLayer> stack;

	public OnionLogger()
	{
		string filepath = "./"+DefaultLogFilename();
		indentSize = 2;
		StringBuilder sb = new StringBuilder();
		for (uint i = 0; i < indentSize; ++i) {
			sb.Append(indentChar);
		}
		indentationString = sb.ToString();
		
		stack = new Stack<LogLayer>();
		writer = new StreamWriter(filepath);
	}

	/** Initializes a new instance of the OnionLogger class.
	 *	@param filepath A path to log the file to.
	 */
	public OnionLogger(string filepath)
	{
		indentSize = 2;
		StringBuilder sb = new StringBuilder();
		for (uint i = 0; i < indentSize; ++i) {
			sb.Append(indentChar);
		}
		indentationString = sb.ToString();

		stack = new Stack<LogLayer>();
		writer = new StreamWriter(filepath);
	}

	public void Info(string message) {
		string indentedMessage = GetCurrentIndentation() + message;
		WriteLine(indentedMessage);
	}

	public void PushLayer(string name) {
		LogLayer layer = new LogLayer(name);
		string startMessage = GetCurrentIndentation() + layer.StartLayer();
		WriteLine(startMessage);
		stack.Push(layer);
	}

	public void PopLayer() {
		if (stack.Count == 0) {
			return;
		}
		LogLayer popped = stack.Pop();
		string endMessage = GetCurrentIndentation() + popped.EndLayer();
		WriteLine(endMessage);
	}

	private string GetCurrentIndentation()
	{
		StringBuilder sb = new StringBuilder();
		for (uint i = 0; i < stack.Count; ++i) {
			sb.Append(indentationString);
		}
		return sb.ToString();
	}

	private void WriteLine(string s)
	{
		writer.WriteLine(s);
		writer.Flush();
	}

	public static string DefaultLogFilename() {
		string timestamp = 
			string.Format("{0}-{1}-{2}-{3}.{4}.{5}",
			              DateTime.Now.Year,
			              DateTime.Now.Month,
			              DateTime.Now.Day,
			              DateTime.Now.Hour,
			              DateTime.Now.Minute,
			              DateTime.Now.Second);
		return (timestamp + ".txt");
	}

	private class LogLayer {

		private readonly string name;

		/// The stopwatch used to keep track of events in this layer.
		private readonly Stopwatch stopwatch;
		
		public LogLayer(string name)
		{
			this.name = name;
			this.stopwatch = new Stopwatch();
		}

		public string StartLayer()
		{
			stopwatch.Start();
			return string.Format("{0} started at {1}", this.name, DateTime.Now);
		}

		public string EndLayer()
		{
			stopwatch.Stop();
			return string.Format("{0} ended after {1}ms", this.name, stopwatch.ElapsedMilliseconds);
		}
	}
}


