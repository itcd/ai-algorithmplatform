using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PositionSetViewer
{
    public partial class LayersOptionDialog : Form
    {
        private Layers layers;

        private Layer selectedLayer;
        public Layer SelectLayer
        {
            get { return selectedLayer; }
        }


        Update update = null;
        bool isDropDown = true;

        public LayersOptionDialog(Layers mPositionPainter, Update update)
        {
            this.update = update;

            InitializeComponent();
            if (mPositionPainter == null)
            {
                throw new Exception("ÎÞÐ§PositionPainter");
            }

            this.layers = mPositionPainter;
            this.layers.LayersCountChanged += this.LoadList;

            this.LoadList();
        }

        private void PainterOptionDialog_Load(object sender, EventArgs e)
        {
            if (isDropDown)
            {
                DeDropDown();
            }
            //this.Deactivate += delegate
            //{
            //    this.Hide();
            //};
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs cea)
        {
            cea.Cancel = true;
            this.Hide();
        }

        delegate void dLoadList();
        private void LoadList()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new dLoadList(LoadList), new object[] { });
                return;
            }

            lock (layers)
            {
                listView.Items.Clear();

                foreach (Layer item in layers)
                {
                    ListViewItem litm = null;
                    litm = listView.Items.Add(item.Name.ToString());
                    litm.Tag = item;

                    (litm.SubItems.Add(item.GetType().Name.ToString().Substring(6))).Name = "Type";
                    (litm.SubItems.Add((item.Visible == true) ? "¡ñ" : "¡ð")).Name = "Visible";
                    (litm.SubItems.Add((item.Active == true) ? "¡ñ" : "¡ð")).Name = "Active";
                }

                //ListViewGroup listViewGroup1 = new ListViewGroup("Group1");
                //listViewGroup1.Name = "Group1";
                //listViewGroup1.Tag = Painter.Layers[0];
                //listView.Groups.Add(listViewGroup1);

                //ListViewGroup listViewGroup2 = new ListViewGroup("Group2");
                //listViewGroup2.Name = "Group2";
                //listViewGroup2.Tag = Painter.Layers[1];
                //listView.Groups.Add(listViewGroup2);

                //listView.Items[0].Group = listViewGroup1;
                //listView.Items[1].Group = listViewGroup2;

                if (listView.Items.Count > 0)
                {
                    listView.Items[0].Selected = true;
                }
            }
        }

        private void UpdateListViewSubItems()
        {
            foreach (ListViewItem listViewItem in listView.Items)
            {
                listViewItem.SubItems["Visible"].Text = (((Layer)listViewItem.Tag).Visible == true) ? "¡ñ" : "¡ð";
                listViewItem.SubItems["Active"].Text = (((Layer)listViewItem.Tag).Active == true) ? "¡ñ" : "¡ð";
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                this.selectedLayer = (Layer)listView.SelectedItems[0].Tag;

                layerPropertyGrid.SelectedObject = this.SelectLayer;

                ActiveCheckBox.Checked = this.SelectLayer.Active;

                VisibleCheckBox.Checked = this.SelectLayer.Visible;

                if (this.SelectLayer.MainColor == null)
                {
                    LayerColorSelect.Enabled = false;
                }
                else
                {
                    LayerColorSelect.BackColor = this.SelectLayer.MainColor;
                }
            }
        }

        private void toolLayerDelete_Click(object sender, EventArgs e)
        {
            if (this.SelectLayer != null)
            {
                layers.Remove(this.SelectLayer);
                this.LoadList();
                update();
            }
        }

        private void toolLayerDown_Click(object sender, EventArgs e)
        {
            if (layers.Down(this.SelectLayer))
            {
                int sequence = listView.SelectedItems[0].Index;
                this.LoadList();
                listView.Items[0].Selected = false;
                listView.Items[sequence + 1].Selected = true;
                listView.Items[sequence + 1].EnsureVisible();
                listView.Select();
                update();
            }
        }

        private void toolLayerUp_Click(object sender, EventArgs e)
        {
            if (layers.Up(this.SelectLayer))
            {
                int sequence = listView.SelectedItems[0].Index;
                this.LoadList();
                listView.Items[0].Selected = false;
                listView.Items[sequence - 1].Selected = true;
                listView.Items[sequence - 1].EnsureVisible();
                listView.Select();
                update();
            }
        }

        private void butRefresh_Click(object sender, EventArgs e)
        {
        }

        private void layerpPropertyGrid_Click(object sender, EventArgs e)
        {
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void layerpPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            UpdateListViewSubItems();
            update();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        public int ScaleTrackBarValue
        {
            get { return ScaleTrackBar.Value; }
            set
            {
                if (value > ScaleTrackBar.Maximum)
                {
                    ScaleTrackBar.Value = ScaleTrackBar.Maximum;
                }
                else if (value < ScaleTrackBar.Minimum)
                {
                    ScaleTrackBar.Value = ScaleTrackBar.Minimum;
                }
                else
                {
                    ScaleTrackBar.Value = value;
                }
                ScaleTrackBarScrolled();
            }
        }

        private void ScaleTrackBar_Scroll(object sender, EventArgs e)
        {
            ScaleTrackBarScrolled();
        }

        private void ScaleTrackBarScrolled()
        {
            float scale = (float)Math.Pow(1.1, (double)ScaleTrackBar.Value);

            layers.MaxRectToDraw = new Rectangle(layers.MaxRectToDraw.X, layers.MaxRectToDraw.Y,
                (int)(layers.InitMaxRectToDraw.Width * scale), (int)(layers.InitMaxRectToDraw.Height * scale));

            update();
        }

        private void PainterOptionDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.layers.LayersCountChanged -= this.LoadList;

            //mPainter.MaxRectInScreen = new Rectangle(mPainter.MaxRectInScreen.x, mPainter.MaxRectInScreen.y,
            //    (int)(originalRect.Width), (int)(originalRect.Height));
            //update();
        }

        private void VisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMouseEntered)
            {
                if (this.SelectLayer != null)
                {
                    foreach (ListViewItem item in listView.SelectedItems)
                    {
                        ((Layer)(item.Tag)).Visible = VisibleCheckBox.Checked;
                        layerPropertyGrid.SelectedObject = this.SelectLayer;
                    }
                    UpdateListViewSubItems();
                    listView.Select();
                    update();
                }
                checkBoxMouseEntered = false;
            }
        }

        private void ActiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMouseEntered)
            {
                if (this.SelectLayer != null)
                {
                    foreach (ListViewItem item in listView.SelectedItems)
                    {
                        ((Layer)(item.Tag)).Active = ActiveCheckBox.Checked;
                        layerPropertyGrid.SelectedObject = this.SelectLayer;
                    }
                    UpdateListViewSubItems();
                    listView.Select();
                    update();
                }
                checkBoxMouseEntered = false;
            }
        }

        private void toolLayer_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void LayerColorSelect_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.SelectLayer != null)
            {
                foreach (ListViewItem item in listView.SelectedItems)
                {
                    ((Layer)(item.Tag)).MainColor = LayerColorSelect.BackColor;
                    layerPropertyGrid.SelectedObject = this.SelectLayer;
                }
                listView.Select();
                update();
            }
        }

        private bool checkBoxMouseEntered = false;
        private void VisibleCheckBox_Enter(object sender, EventArgs e)
        {
            checkBoxMouseEntered = true;
        }

        private void ActiveCheckBox_Enter(object sender, EventArgs e)
        {
            checkBoxMouseEntered = true;
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
            this.Height += 202;
            isDropDown = true;
        }

        private void DeDropDown()
        {
            this.Height -= 202;
            isDropDown = false;
        }
    }
}