using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlgorithmDemo
{
    public partial class FlowControlerForm : Form
    {
        delegate void dSelectConfiguratoinObject(object o);

        bool isDropDown = true;
        System.Threading.Thread workerThread;
        enum ControlMode { Manual, Auto };
        ControlMode controlMode = ControlMode.Manual;
        int lastTickCount = Environment.TickCount;
        int frameTime = 300;

        public FlowControlerForm()
        {
            InitializeComponent();
            this.Activated += delegate { this.Opacity = 1; };
            this.Deactivate += delegate
            {
                this.Opacity = 0.6;
                if (isDropDown)
                {
                    DeDropDown();
                }
            };
        }

        private void FlowControler_Load(object sender, EventArgs e)
        {
            DemoRateTrackBar.Value = (int)(1000.0 / frameTime);
            if (isDropDown)
            {
                DeDropDown();
            }
            this.FormClosed += delegate
            {
                if (workerThread != null)
                {
                    try
                    {
                        if ((workerThread.ThreadState & System.Threading.ThreadState.Suspended) == System.Threading.ThreadState.Suspended)
                        {
                            workerThread.Resume();
                        }

                        if ((workerThread.ThreadState & System.Threading.ThreadState.Suspended) != System.Threading.ThreadState.Suspended)
                        {
                            workerThread.Abort();
                        }
                    }
                    catch (System.Threading.ThreadStateException exception)
                    { }
                }
            };
        }

        public void SelectConfiguratoinObject(object o)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new dSelectConfiguratoinObject(SelectConfiguratoinObject), new object[] { o });
                return;
            }
            propertyGrid.SelectedObject = o;
        }

        private void CForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //update();
        }

        delegate void dShow();

        public void SuspendAndRecordWorkerThread()
        {
            this.BeginInvoke(new dShow(Show), new object[]{});

            if (controlMode == ControlMode.Manual)
            {
                EnableFlowBotton();
                workerThread = System.Threading.Thread.CurrentThread;
                System.Threading.Thread.CurrentThread.Suspend();
            }
            else if (controlMode == ControlMode.Auto)
            {
                //if (lastTimeMillisecond == default)
                //{
                //    lastTimeMillisecond = System.DateTime.Now.Millisecond;

                //    System.Threading.Thread.CurrentThread.Join(frameTime);
                //}
                //else
                {
                    int elapsed = unchecked(Environment.TickCount - lastTickCount);
                    int sleepTime = frameTime - lastTickCount / 1500;
                    if (sleepTime > 0)
                    {
                        System.Threading.Thread.CurrentThread.Join(sleepTime);
                    }
                    lastTickCount = DateTime.Now.Millisecond;
                }
            }
        }

        delegate void dTemp();

        private void EnableFlowBotton()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new dTemp(EnableFlowBotton));
                return;
            }

            AutoStepBotton.Enabled = true;
            BreakAllBotton.Enabled = true;
            NextStepBotton.Enabled = true;
            this.Focus();
        }

        private void NextStepBotton_Click(object sender, EventArgs e)
        {
            if (workerThread != null)
            {
                AutoStepBotton.Enabled = false;
                BreakAllBotton.Enabled = false;
                NextStepBotton.Enabled = false;
                if (this.ParentForm != null)
                {
                    this.Parent.Focus();
                }
                workerThread.Resume();
            }
        }

        private void AutoStepBotton_Click(object sender, EventArgs e)
        {
            if (controlMode == ControlMode.Manual)
            {
                controlMode = ControlMode.Auto;
                AutoStepBotton.Text = "||";
                NextStepBotton.Enabled = false;
                workerThread.Resume();
            }
            else if (controlMode == ControlMode.Auto)
            {
                controlMode = ControlMode.Manual;
                AutoStepBotton.Text = ">>";
                AutoStepBotton.Enabled = true;
                BreakAllBotton.Enabled = true;
                NextStepBotton.Enabled = true;
            }
        }

        delegate void dReset();
        public void Reset()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new dReset(Reset), new object[] { });
                return;
            }

            controlMode = ControlMode.Manual;
            this.SelectConfiguratoinObject(null);
            AutoStepBotton.Text = ">>";
            AutoStepBotton.Enabled = false;
            BreakAllBotton.Enabled = false;
            NextStepBotton.Enabled = false;
        }

        private void BreakAllBotton_Click(object sender, EventArgs e)
        {
            if ((workerThread.ThreadState & System.Threading.ThreadState.Suspended) == System.Threading.ThreadState.Suspended)
            {
                workerThread.Resume();
            }
            workerThread.Abort();
            Reset();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs cea)
        {
            //cea.Cancel = true;
            //this.Hide();
        }

        private void propertyGrid_Click(object sender, EventArgs e)
        {
        }

        private void DropDownBotton_Click(object sender, EventArgs e)
        {
            if (isDropDown)
            {
                DeDropDown();
            }
            else
            {
                DropDown();
            }
        }

        private void DropDown()
        {
            this.Height += 121;
            isDropDown = true;
        }

        private void DeDropDown()
        {
            this.Height -= 121;
            isDropDown = false;
        }

        private void DemoRateTrackBar_Scroll(object sender, EventArgs e)
        {
            int time = (int)(1000.0 / DemoRateTrackBar.Value);
            System.Threading.Interlocked.Exchange(ref frameTime, time);
        }
    }
}