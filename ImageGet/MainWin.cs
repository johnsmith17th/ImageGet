using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ImageGet
{
    public partial class MainWin : Form
    {
        public MainWin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get the task parameters
        /// </summary>
        public TaskParameters Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Initialize the dispatcher of work threads
        /// </summary>
        private void InitializeDispatcher()
        {
            dispatcher = new BackgroundWorker();
            dispatcher.WorkerReportsProgress = true;
            dispatcher.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnDispatcherRunWorkerCompleted);
            dispatcher.ProgressChanged += new ProgressChangedEventHandler(OnDispatcherProgressChanged);
            dispatcher.DoWork += new DoWorkEventHandler(OnDispatcherDoWork);
        }

        /// <summary>
        /// Initialize the task parameters, call before the form is loaded
        /// </summary>
        public void InitializeParameters(string expression, string destination)
        {
            Parameters = new TaskParameters();
            Parameters.Source = expression;
            Parameters.Destination = destination;
            if (!string.IsNullOrEmpty(expression))
            {
                int lastDot = expression.LastIndexOf('.');
                int lastSlash = expression.LastIndexOf('/');
                string f1 = expression;
                string f2 = f1.Substring(0, lastSlash + 1);
                string f3 = f1.Substring(lastDot);
                string f4 = f1.Substring(lastSlash + 1, lastDot - lastSlash - 1);
                int d1;
                if (int.TryParse(f4, out d1))
                {
                    Parameters.Parameter = lastDot - lastSlash - 1;
                    Parameters.Expression = string.Format("{0}{1}{2}", f2, "{0}", f3);
                    Parameters.BeginIndex = 1;
                    Parameters.EndIndex = d1;
                    Parameters.TotalCount = Parameters.EndIndex - Parameters.BeginIndex + 1;
                    Parameters.ThreadCount = Convert.ToInt32(Math.Floor((double)(Parameters.TotalCount) / 5.0) + 1);
                    if (Parameters.ThreadCount > 5) Parameters.ThreadCount = 5;
                }
                else
                {
                    Parameters.IsSingle = true;
                    Parameters.Expression = expression;
                    Parameters.TotalCount = 1;
                }
            }
        }        

        /// <summary>
        /// add item to task list box and scroll to the end
        /// </summary>
        private void AddToTaskList(object item)
        {
            TaskList.Items.Add(item);
            TaskList.TopIndex = TaskList.Items.Count - (int)(TaskList.Height / TaskList.ItemHeight);
        }

        #region Interaction Events

        private void OnLoad(object sender, EventArgs e)
        {
            if (Parameters == null) Parameters = new TaskParameters();
            SourceLabel.Text = Parameters.Expression;
            RangeLabel.Text = string.Format("{0} - {1}, d{2}, t{3}, {4}",
                Parameters.BeginIndex, Parameters.EndIndex, Parameters.Parameter, Parameters.ThreadCount,
                Parameters.IsSingle ? "single" : "multi");
            DestinationLabel.Text = Parameters.Destination;
            ProgressBar.Maximum = Parameters.TotalCount;

            InitializeDispatcher();
            dispatcher.RunWorkerAsync(Parameters);
        }

        private void OnShowDirectoryClicked(object sender, EventArgs e)
        {
            Program.OpenFolder(Parameters.Destination);
        }

        private void OnActionButtonClicked(object sender, EventArgs e)
        {
            switch (ActionButton.Text)
            {
                case "Cancel":
                    Application.Exit();
                    break;
                case "Close":
                    this.Close();
                    break;
            }
        } 

        #endregion

        #region Dispatcher Events

        private void OnDispatcherDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            TaskParameters parameters = e.Argument as TaskParameters;
            parameters.DoneEvent = new AutoResetEvent(false);

            if (parameters.IsSingle)
            {
                string locp = Program.GetLocalPath(parameters.Source, parameters.Destination);
                if (Program.Download(parameters.Source, locp))
                {
                    worker.ReportProgress(100, new ProgressMessage(parameters.Source));
                }
            }
            else
            {
                workerList = new List<BackgroundWorker>();
                parameterList = new List<TaskRunParameters>();

                for (int i = 0; i < parameters.ThreadCount; i++)
                {
                    BackgroundWorker wk = new BackgroundWorker();
                    wk.WorkerReportsProgress = true;
                    wk.ProgressChanged += new ProgressChangedEventHandler(OnWorkerProgressChanged);
                    wk.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnWorkerRunWorkerCompleted);
                    wk.DoWork += new DoWorkEventHandler(OnWorkerDoWork);
                    workerList.Add(wk);

                    TaskRunParameters tp = new TaskRunParameters();
                    tp.Destination = parameters.Destination;
                    tp.Expression = parameters.Expression;
                    tp.Parameter = parameters.Parameter;
                    parameterList.Add(tp);
                }

                int index = 0;
                for (int i = parameters.BeginIndex; i <= parameters.EndIndex; i++)
                {
                    if (index >= parameters.ThreadCount) index = 0;
                    parameterList[index].Indices.Add(i);
                }

                for (int i = 0; i < parameters.ThreadCount; i++)
                {
                    workerList[i].RunWorkerAsync(parameterList[i]);
                }

                parameters.DoneEvent.WaitOne();
                Thread.Sleep(1000);
            }
        }

        private void OnDispatcherProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressMessage m = e.UserState as ProgressMessage;
            if (m != null)
            {
                AddToTaskList(m);
            }
            if (e.ProgressPercentage == 100)
            {
                ProgressBar.Value = Parameters.TotalCount;
            }
        }

        private void OnDispatcherRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowButton.Enabled = true;
            ActionButton.Text = "Close";
            if (CloseCheckBox.Checked)
            {
                this.Close();
            }
        } 

        #endregion

        #region Worker Events

        private void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            TaskRunParameters parameters = e.Argument as TaskRunParameters;

            foreach (var item in parameters.Indices)
            {
                string fmt = "D" + parameters.Parameter;
                string addr = string.Format(parameters.Expression, item.ToString(fmt));
                string locp = Program.GetLocalPath(addr, parameters.Destination);
                if (Program.Download(parameters.Client, addr, locp))
                {
                    ProgressMessage msg = new ProgressMessage(addr);
                    worker.ReportProgress(0, msg);
                }
                else
                {
                    addr = string.Format(parameters.Expression, item);
                    if (Program.Download(parameters.Client, addr, locp))
                    {
                        ProgressMessage msg = new ProgressMessage(addr);
                        worker.ReportProgress(0, msg);
                    }
                }
            }
        }

        private void OnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(100);
            lock (increaseCountLock)
            {
                Parameters.FinishCount++;
                if (Parameters.FinishCount == Parameters.ThreadCount)
                {
                    Parameters.DoneEvent.Set();
                }
            }
        }

        private void OnWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressMessage m = e.UserState as ProgressMessage;
            if (m != null)
            {
                BeginInvoke(new Action(() =>
                {
                    AddToTaskList(m);
                    ProgressBar.PerformStep();
                }));
            }
        }

        #endregion

        #region Fields

        // the subthread dispatch thread
        private BackgroundWorker dispatcher;

        // work thread list
        private List<BackgroundWorker> workerList;

        // work parameter list, which the count property should equal to work thread list
        private List<TaskRunParameters> parameterList;

        // lock for increasing the number of completed threads
        private object increaseCountLock = new object();

        #endregion
    }

    public class TaskParameters
    {
        public bool IsSingle { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Expression { get; set; }
        public int Parameter { get; set; }
        public int BeginIndex { get; set; }
        public int EndIndex { get; set; }
        public int FinishCount { get; set; }
        public int ThreadCount { get; set; }
        public int TotalCount { get; set; }
        public AutoResetEvent DoneEvent { get; set; }
    }

    public class TaskRunParameters
    {
        public TaskRunParameters()
        {
            Client = new WebClient();
            Indices = new List<int>();
        }

        public string Expression { get; set; }
        public string Destination { get; set; }
        public int Parameter { get; set; }
        public WebClient Client { get; private set; }
        public List<int> Indices { get; private set; }
    }

    public class ProgressMessage
    {
        public ProgressMessage(string message)
        {
            this.Time = DateTime.Now;
            this.Message = message;
        }

        public DateTime Time
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Time.ToString("HH:mm:ss"), Message);
        }
    }
}
