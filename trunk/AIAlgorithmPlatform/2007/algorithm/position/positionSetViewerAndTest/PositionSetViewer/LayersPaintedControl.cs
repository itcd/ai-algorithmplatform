using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Position_Interface;

namespace PositionSetViewer
{
    public partial class LayersPaintedControl : UserControl
    {
        private LayersExOptDlg layers;
        public LayersExOptDlg Layers
        {
            get { return layers; }
            set { layers = value; }
        }

        Browser browser;

        private float screenUpperLeftPositionX;
        public float ScreenUpperLeftPositionX
        {
            get { return screenUpperLeftPositionX; }
        }

        private float screenUpperLeftPositionY;
        public float ScreenUpperLeftPositionY
        {
            get { return screenUpperLeftPositionY; }
        }

        private Bitmap entireImage = null;
        public Bitmap EntireImage
        {
            get { return entireImage; }
        }

        public LayersPaintedControl()
        {
            InitializeComponent();
                        
            this.DoubleBuffered = true;
        }

        private void LayersPaintedControl_Load(object sender, EventArgs e)
        {
            this.Resize += LayersPaintedControl_Resize;
            this.MouseUp += LayersPaintedControl_MouseUp;
            this.MouseLeave += LayersPaintedControl_MouseLeave;
            this.MouseMove += LayersPaintedControl_MouseMove;
            this.MouseDown += LayersPaintedControl_MouseDown;
            this.MouseWheel += LayersPaintedControl_MouseWheel;

            layers.Clearing += Reset;

            browser = new Browser(this);
            browser.Show(this);
            browser.Hide();

            this.ParentForm.Activated += delegate { browser.Show(); };
            this.ParentForm.Deactivate += delegate { browser.Hide(); };
            this.ParentForm.FormClosing += delegate { layers.CloseOptionDialog(); };
            this.ParentForm.Invalidated += delegate { this.Invalidate(); };
            
            layers.UnActiveBitmapChanged += delegate
            {
                entireImage = layers.UnActiveBitmap;
                browser.ParentImageIsChanged();
            };

            layers.ShowOptionDialog(this.Invalidate);
            layers.HideOptionDialog();
        }

        private void Reset()
        {
            screenUpperLeftPositionX = 0;
            screenUpperLeftPositionY = 0;
            layers.MaxRectToDraw = layers.InitMaxRectToDraw;
        }

        public float ConvertMouseXToRealX(int x)
        {
            return layers.ConvertScreenXToRealX(x + ScreenUpperLeftPositionX);
        }

        public float ConvertMouseYToRealY(int y)
        {
            return layers.ConvertScreenYToRealY(y + ScreenUpperLeftPositionY);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            layers.Draw(e.Graphics, -(int)ScreenUpperLeftPositionX, -(int)ScreenUpperLeftPositionY, this.ClientSize.Width, this.ClientSize.Height);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            browser.Invalidate();
        }

        private void LayersPaintedControl_Resize(object sender, EventArgs e)
        {
            ValidateScreenUpperLeftPosition();
            this.Invalidate();
        }

        public enum State_PainterForm_MouseDown { onDaggle, Other };

        public State_PainterForm_MouseDown state_PainterForm_MouseDown = State_PainterForm_MouseDown.onDaggle;

        bool onDaggle = false;

        Point beginDagglePoint;

        private void LayersPaintedControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                layers.ShowOptionDialog(this.Invalidate);
            }
            if (e.Button == MouseButtons.Left)
            {
                layers.HideOptionDialog();

                if (state_PainterForm_MouseDown == State_PainterForm_MouseDown.onDaggle)
                {
                    browser.Location = new Point(Location.X + ParentForm.Location.X, ParentForm.Location.Y + ParentForm.Size.Height - browser.Size.Height);
                    browser.Show();
                    this.Focus();

                    this.Cursor = Cursors.SizeAll;
                    onDaggle = true;
                    beginDagglePoint = new Point(e.Location.X, e.Location.Y);
                }
            }
        }

        private void LayersPaintedControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (state_PainterForm_MouseDown == State_PainterForm_MouseDown.onDaggle)
            {
                if (onDaggle == true)
                {                    
                    browser.Hide();

                    this.Cursor = Cursors.Default;
                    onDaggle = false;
                }
            }
        }

        private void LayersPaintedControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (state_PainterForm_MouseDown == State_PainterForm_MouseDown.onDaggle)
            {
                if (onDaggle == true)
                {
                    screenUpperLeftPositionX = ScreenUpperLeftPositionX + beginDagglePoint.X - e.Location.X;
                    screenUpperLeftPositionY = ScreenUpperLeftPositionY + beginDagglePoint.Y - e.Location.Y;

                    ValidateScreenUpperLeftPosition();

                    beginDagglePoint = e.Location;

                    this.Invalidate();
                }
            }
        }

        private void ValidateScreenUpperLeftPosition()
        {
            if (screenUpperLeftPositionX > layers.RealRectToDraw.Width - this.ClientSize.Width)
            {
                screenUpperLeftPositionX = layers.RealRectToDraw.Width - this.ClientSize.Width;
            }

            if (screenUpperLeftPositionX < 0)
            {
                screenUpperLeftPositionX = 0;
            }

            if (screenUpperLeftPositionY > layers.RealRectToDraw.Height - this.ClientSize.Height)
            {
                screenUpperLeftPositionY = layers.RealRectToDraw.Height - this.ClientSize.Height;
            }

            if (screenUpperLeftPositionY < 0)
            {
                screenUpperLeftPositionY = 0;
            }
        }

        private void LayersPaintedControl_MouseLeave(object sender, EventArgs e)
        {
            if (state_PainterForm_MouseDown == State_PainterForm_MouseDown.onDaggle)
            {
                if (onDaggle == true)
                {
                    this.Cursor = Cursors.Default;
                    onDaggle = false;
                }
            }
        }

        private void LayersPaintedControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (layers.painterOptionDialog != null)
            {
                if (e.Delta > 0)
                {
                    layers.painterOptionDialog.ScaleTrackBarValue += 1;
                }
                else
                {
                    layers.painterOptionDialog.ScaleTrackBarValue -= 1;
                }
            }
        }
        
        delegate DialogResult dShow(string text);

        public IPosition GetMouseDoubleChickedRealPosition()
        {
            System.Threading.Thread currentThread = System.Threading.Thread.CurrentThread;
            Position_Point mouseChickedRealPosition = null;

            this.BeginInvoke(new dShow(MessageBox.Show), new object[] { "please double chick on the screen." });

            MouseDoubleClick += delegate(object sender, MouseEventArgs e)
            {
                mouseChickedRealPosition = new Position_Point();
                mouseChickedRealPosition.SetX(ConvertMouseXToRealX(e.X));
                mouseChickedRealPosition.SetY(ConvertMouseYToRealY(e.Y));

                if ((currentThread.ThreadState & System.Threading.ThreadState.Suspended) == System.Threading.ThreadState.Suspended)
                {
                    currentThread.Resume();
                }
            };

            currentThread.Suspend();
            return mouseChickedRealPosition;
        }
    }
}
