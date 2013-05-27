using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;

namespace ImageGet
{
    public partial class MainWin2 : Form
    {
        public MainWin2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get the task parameters
        /// </summary>
        public TaskParameters2 Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Initialize the task parameters, call before the form is loaded
        /// </summary>
        public void InitializeParameters(string[] sources, string destination)
        {
            Parameters = new TaskParameters2();
            
            int it = 0;
            foreach (var item in sources)
            {
                if (IsAvailableSource(item))
                {
                    TaskNode node = new TaskNode()
                    {
                        Index = it,
                        Source = item,
                        State = TaskNodeState.Ready
                    };
                    Parameters.TaskNodes.Add(node);
                    it++;
                }
            }

            Parameters.Destination = destination;
            Parameters.TotalCount = Parameters.TaskNodes.Count;
        }

        /// <summary>
        /// Initialize the download threads
        /// </summary>
        private void InitializeDownloader()
        {
            downloader = new BackgroundWorker();
            downloader.WorkerReportsProgress = true;
            downloader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnDownloaderCompleted);
            downloader.ProgressChanged += new ProgressChangedEventHandler(OnDownloaderProgressChanged);
            downloader.DoWork += new DoWorkEventHandler(OnDownloaderDoWork);
        }

        /// <summary>
        /// check if the source is available for download
        /// </summary>
        private bool IsAvailableSource(string source)
        {
            if (source.StartsWith("http://"))
            {
                string ext = source.Substring(source.LastIndexOf('.')).ToLower();
                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".bmp":
                    case ".png":
                        return true;
                }
            }
            return false;
        }

        #region Interaction Events

        private void OnLoad(object sender, EventArgs e)
        {
            DestinationLabel.Text = Parameters.Destination;
            TaskListView.Items.Clear();
            foreach (var item in Parameters.TaskNodes)
            {
                ListViewItem vi = new ListViewItem(new string[] { item.Source, item.State.ToString() });
                TaskListView.Items.Add(vi);
            }
            ProgressBar.Maximum = Parameters.TotalCount;
            InitializeDownloader();
            downloader.RunWorkerAsync(Parameters);
        }

        private void OnShowButtonClicked(object sender, EventArgs e)
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

        #region Downloader Events

        private void OnDownloaderDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            TaskParameters2 parameters = e.Argument as TaskParameters2;

            using (WebClient client = new WebClient())
            {
                for (int i = 0; i < parameters.TotalCount; i++)
                {
                    TaskNode node = parameters.TaskNodes[i];
                    node.State = TaskNodeState.Downloading;
                    worker.ReportProgress(0, node);
                    string path = Program.GetLocalPath(node.Source, parameters.Destination);

                    if (Program.Download(client, node.Source, path))
                    {
                        node.State = TaskNodeState.Completed;
                    }
                    else
                    {
                        node.State = TaskNodeState.Failed;
                    }
                    worker.ReportProgress(0, node);
                }
            }
        }

        private void OnDownloaderProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TaskNode node = e.UserState as TaskNode;
            if (node != null)
            {
                TaskListView.Items[node.Index].SubItems[1].Text = node.State.ToString();
                TaskListView.TopItem = TaskListView.Items[node.Index];
                if (node.State == TaskNodeState.Completed || node.State == TaskNodeState.Failed)
                {
                    ProgressBar.PerformStep();
                }
            }
        }

        private void OnDownloaderCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Value = Parameters.TotalCount; 
            ShowButton.Enabled = true;
            ActionButton.Text = "Close";
            if (CloseCheckBox.Checked)
            {
                this.Close();
            }
        }

        #endregion

        #region Fields

        private BackgroundWorker downloader;

        #endregion
    }

    public class TaskParameters2
    {
        public List<TaskNode> TaskNodes { get; private set; }
        public string Destination { get; set; }
        public int TotalCount { get; set; }
        public AutoResetEvent DoneEvent { get; set; }

        public TaskParameters2()
        {
            TaskNodes = new List<TaskNode>();
        }
    }

    public class TaskNode
    {
        public int Index { get; set; }
        public string Source { get; set; }
        public TaskNodeState State { get; set; }
    }

    public enum TaskNodeState
    {
        Ready,
        Downloading,
        Completed,
        Failed
    }
}
