using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PositionSetViewer;
using M2M;
using NearestNeighbor;
using Position_Interface;
using Position_Implement;
using Position_Connected_Interface;
using Position_Connected_Implement;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace PositionSetEditer
{
    public partial class LayersEditedControl : UserControl
    {
        const string error_position = "Can not modify the position!";
        const string error_connection = "Can not modify the connection!";

        M2M_NN m2m_NN;

        LayersPaintedControl layersPaintedControl;
        public LayersPaintedControl LayersPaintedControl
        {
            get { return layersPaintedControl; }
            set
            {
                layersPaintedControl = value;
                if (value != null)
                {
                    this.layers = layersPaintedControl.Layers;
                    layersPaintedControl.MouseMove += OnLayersPainterFormMouseMove;
                    layersPaintedControl.MouseDown += OnLayersPainterFormMouseDown;
                }
            }
        }

        LayersExOptDlg layers;
        Layer currentLayer = null;

        delegate void dGetPosition(IPosition position);
        event dGetPosition MouseMovedRealPosition;
        event dGetPosition MouseLBottonCheckRealPosition;

        delegate void dMouseDoubleChick(object sender, MouseEventArgs e);
        event dMouseDoubleChick MouseDoubleChickOnLayersPaintedControl;

        public LayersEditedControl()
        {
            InitializeComponent();
        }

        private void PositionSetEditedControl_Load(object sender, EventArgs e)
        {
            selectBotton.Enabled = false;
            addNodeButton.Enabled = false;
            removeNodeButton.Enabled = false;
            daggleButton.Enabled = false;
            addConnectionButton.Enabled = false;
            addDoubleConnectionButton.Enabled = false;
            newButton.Enabled = false;
            openButton.Enabled = false;
            saveButton.Enabled = false;

            layersPaintedControl.MouseMove += PositionCoordinateLabelShow;
            layersPaintedControl.MouseDoubleClick += delegate(object sender2, MouseEventArgs e2)
            {
                if (MouseDoubleChickOnLayersPaintedControl != null)
                {
                    MouseDoubleChickOnLayersPaintedControl(sender2, e2);
                }
            };
        }

        public void OnLayersPainterFormMouseMove(object sender, MouseEventArgs e)
        {
            if (MouseMovedRealPosition != null)
            {
                Position_Connected_Edit mouseChickedRealPosition = new Position_Connected_Edit();
                mouseChickedRealPosition.SetX(layersPaintedControl.ConvertMouseXToRealX(e.X));
                mouseChickedRealPosition.SetY(layersPaintedControl.ConvertMouseYToRealY(e.Y));
                if (MouseMovedRealPosition != null)
                {
                    MouseMovedRealPosition(mouseChickedRealPosition);
                }
            }
        }

        public void OnLayersPainterFormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MouseLBottonCheckRealPosition != null)
                {
                    Position_Connected_Edit mouseChickedRealPosition = new Position_Connected_Edit();
                    mouseChickedRealPosition.SetX(layersPaintedControl.ConvertMouseXToRealX(e.X));
                    mouseChickedRealPosition.SetY(layersPaintedControl.ConvertMouseYToRealY(e.Y));
                    if (MouseLBottonCheckRealPosition != null)
                    {
                        MouseLBottonCheckRealPosition(mouseChickedRealPosition);
                    }
                }
            }
        }

        private void ResetEventHandler()
        {
            MouseMovedRealPosition = null;
            MouseLBottonCheckRealPosition = null;
        }

        public void PositionCoordinateLabelShow(object sender, MouseEventArgs e)
        {
            this.positionCoordinateLabel.Text = "(" + layersPaintedControl.ConvertMouseXToRealX(e.X).ToString()
                + ", " + layersPaintedControl.ConvertMouseYToRealY(e.Y).ToString() + ")";
        }

        public enum State_PainterForm_MouseDown { OnDaggle, OnSelect, Other };
        IPositionSet selectedPositionSetEdit;
        Layer_PositionSet selectedPositionSetLayer;
        private void EditedBotton_Click(object sender, EventArgs e)
        {
            Layer_PositionSet layer = layers.painterOptionDialog.SelectLayer as Layer_PositionSet;
            if (layer != null)
            {
                if (layer.EditAble)
                {
                    selectedPositionSetLayer = layer;
                    selectedPositionSetEdit = layer.PositionSet;
                    m2m_NN = new M2M_NN();
                    m2m_NN.PreProcess(selectedPositionSetEdit);

                    selectBotton.Enabled = true;
                    addNodeButton.Enabled = true;
                    removeNodeButton.Enabled = true;
                    daggleButton.Enabled = true;
                    addConnectionButton.Enabled = true;
                    addDoubleConnectionButton.Enabled = true;
                    newButton.Enabled = true;
                    openButton.Enabled = true;
                    saveButton.Enabled = true;
                }
            }
        }

        private void DaggleButton_Click(object sender, EventArgs e)
        {
            if (currentLayer != null)
            {
                layers.Remove(currentLayer);
            }
            currentLayer = null;
            ResetEventHandler();
            layersPaintedControl.state_PainterForm_MouseDown = LayersPaintedControl.State_PainterForm_MouseDown.onDaggle;
        }

        private void SelectBotton_Click(object sender, EventArgs e)
        {
            layersPaintedControl.state_PainterForm_MouseDown = LayersPaintedControl.State_PainterForm_MouseDown.onDaggle;
            ResetEventHandler();
            //layersPainterForm.state_PainterForm_MouseDown = LayersPainterForm.State_PainterForm_MouseDown.Other;
            IPosition nearestPoint = new Position_Point(0, 0);
            PositionSetEdit_ImplementByICollectionTemplate positionSetEdit_ImplementByICollectionTemplate = new PositionSetEdit_ImplementByICollectionTemplate();
            positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
            Layer_PositionSet_Square layer = new Layer_PositionSet_Square(positionSetEdit_ImplementByICollectionTemplate);

            MouseMovedRealPosition = delegate(IPosition position)
            {
                positionSetEdit_ImplementByICollectionTemplate.RemovePosition(nearestPoint);
                nearestPoint = m2m_NN.NearestNeighbor(position);
                positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
                layer.SpringLayerRepresentationChangedEvent(layer);
                layersPaintedControl.Invalidate();
            };

            layer.VisibleInOptDlg = false;
            layer.Active = true;
            layer.Name = "Edited Figure";
            layer.SquareFrameDrawer.RectangleRadiusWidth = 8;
            layer.SquareFrameDrawer.LineWidth = 1;
            layer.SquareFrameDrawer.LineColor = Color.YellowGreen;
            layer.CenterPointCoordinate.Visible = true;
            layer.CenterPointCoordinate.CoordinateFont = new Font(layer.CenterPointCoordinate.CoordinateFont, FontStyle.Bold);

            //layer.Point.IsDrawPointBorder = true;
            //layer.Point.PointColor = Color.Red;
            //layer.Point.PointRadius = 3;
            //layer.PointCoordinate.CoordinateFont = new Font(layer.PointCoordinate.CoordinateFont, FontStyle.Bold);
            //layer.PointCoordinate.Visible = true;

            if (currentLayer != null)
            {
                layers.Remove(currentLayer);
            }
            currentLayer = layer;
            layers.Add(layer);
        }

        private void AddNodeButton_Click(object sender, EventArgs e)
        {
            layersPaintedControl.state_PainterForm_MouseDown = LayersPaintedControl.State_PainterForm_MouseDown.Other;
            IPosition currentPoint = new Position_Connected_Edit(0, 0);
            PositionSetEdit_ImplementByICollectionTemplate positionSetEdit_ImplementByICollectionTemplate = new PositionSetEdit_ImplementByICollectionTemplate();
            positionSetEdit_ImplementByICollectionTemplate.AddPosition(currentPoint);
            Layer_PositionSet_Point layer = new Layer_PositionSet_Point(positionSetEdit_ImplementByICollectionTemplate);

            MouseMovedRealPosition = delegate(IPosition position)
            {
                positionSetEdit_ImplementByICollectionTemplate.RemovePosition(currentPoint);
                currentPoint = position;
                positionSetEdit_ImplementByICollectionTemplate.AddPosition(currentPoint);
                layer.SpringLayerRepresentationChangedEvent(layer);
                layersPaintedControl.Invalidate();
            };

            layer.VisibleInOptDlg = false;
            layer.Active = true;
            layer.Name = "Edited Figure";
            layer.Point.IsDrawPointBorder = true;
            layer.Point.PointColor = Color.Red;
            layer.Point.PointRadius = 3;
            layer.PointCoordinate.CoordinateFont = new Font(layer.PointCoordinate.CoordinateFont, FontStyle.Bold);
            layer.PointCoordinate.Visible = true;

            if (currentLayer != null)
            {
                layers.Remove(currentLayer);
            }
            currentLayer = layer;
            layers.Add(layer);

            MouseLBottonCheckRealPosition = delegate(IPosition position)
            {
                //((IPositionSetEdit)selectedPositionSetEdit).AddPosition(position);
                if (selectedPositionSetEdit is IPositionSet_ConnectedEdit && position is IPosition_Connected)
                    ((IPositionSet_ConnectedEdit)selectedPositionSetEdit).AddPosition_Connected((IPosition_Connected)position);
                else
                    if (selectedPositionSetEdit is IPositionSetEdit)
                        ((IPositionSetEdit)selectedPositionSetEdit).AddPosition(position);
                    else
                        throw new Exception(error_position);

                selectedPositionSetLayer.SpringLayerRepresentationChangedEvent(selectedPositionSetLayer);
                if (m2m_NN.CanInsert(position))
                {
                    m2m_NN.Insert(position);
                }
                else
                {
                    m2m_NN.PreProcess(selectedPositionSetEdit);
                    selectedPositionSetLayer.SpringLayerMaxRectChangedEvent();
                }
                layersPaintedControl.Invalidate();
            };
        }

        private void RemoveNodeButton_Click(object sender, EventArgs e)
        {
            ResetEventHandler();
            layersPaintedControl.state_PainterForm_MouseDown = LayersPaintedControl.State_PainterForm_MouseDown.Other;
            IPosition nearestPoint = new Position_Connected_Edit(0, 0);
            PositionSetEdit_ImplementByICollectionTemplate positionSetEdit_ImplementByICollectionTemplate = new PositionSetEdit_ImplementByICollectionTemplate();
            positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
            Layer_PositionSet_Square layer = new Layer_PositionSet_Square(positionSetEdit_ImplementByICollectionTemplate);

            MouseMovedRealPosition = delegate(IPosition position)
            {
                positionSetEdit_ImplementByICollectionTemplate.RemovePosition(nearestPoint);
                nearestPoint = m2m_NN.NearestNeighbor(position);
                positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
                layer.SpringLayerRepresentationChangedEvent(layer);
                layersPaintedControl.Invalidate();
            };

            layer.VisibleInOptDlg = false;
            layer.Active = true;
            layer.Name = "Edited Figure";
            layer.SquareFrameDrawer.RectangleRadiusWidth = 8;
            layer.SquareFrameDrawer.LineWidth = 1;
            layer.SquareFrameDrawer.LineColor = Color.Red;
            layer.CenterPointCoordinate.Visible = true;
            layer.CenterPointCoordinate.CoordinateFont = new Font(layer.CenterPointCoordinate.CoordinateFont, FontStyle.Bold);

            if (currentLayer != null)
            {
                layers.Remove(currentLayer);
            }
            currentLayer = layer;
            layers.Add(layer);

            MouseLBottonCheckRealPosition = delegate(IPosition position)
            {
                //((IPositionSetEdit)selectedPositionSetEdit).RemovePosition(nearestPoint);
                if (selectedPositionSetEdit is IPositionSet_ConnectedEdit && nearestPoint is IPosition_Connected)
                    ((IPositionSet_ConnectedEdit)selectedPositionSetEdit).RemovePosition_Connected((IPosition_Connected)nearestPoint);
                else
                    if (selectedPositionSetEdit is IPositionSetEdit)
                        ((IPositionSetEdit)selectedPositionSetEdit).RemovePosition(nearestPoint);
                    else
                        throw new Exception(error_position);
                selectedPositionSetLayer.SpringLayerRepresentationChangedEvent(selectedPositionSetLayer);
                m2m_NN.Remove(nearestPoint);
                layersPaintedControl.Invalidate();
            };
        }

        delegate DialogResult dShow(string text);
        public IPosition GetMouseDoubleChickedNearestPositionInCurrentPositionSet(IPositionSet currentPositionSet)
        {
            M2M_NN m2m_NN = new M2M_NN();
            m2m_NN.PreProcess(currentPositionSet);

            layersPaintedControl.state_PainterForm_MouseDown = LayersPaintedControl.State_PainterForm_MouseDown.onDaggle;
            ResetEventHandler();
            //layersPainterForm.state_PainterForm_MouseDown = LayersPainterForm.State_PainterForm_MouseDown.Other;
            IPosition nearestPoint = new Position_Connected_Edit(0, 0);
            PositionSetEdit_ImplementByICollectionTemplate positionSetEdit_ImplementByICollectionTemplate = new PositionSetEdit_ImplementByICollectionTemplate();
            positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
            Layer_PositionSet_Square layer = new Layer_PositionSet_Square(positionSetEdit_ImplementByICollectionTemplate);

            MouseMovedRealPosition = delegate(IPosition position)
            {
                positionSetEdit_ImplementByICollectionTemplate.RemovePosition(nearestPoint);
                nearestPoint = m2m_NN.NearestNeighbor(position);
                positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
                layer.SpringLayerRepresentationChangedEvent(layer);
                layersPaintedControl.Invalidate();
            };

            layer.VisibleInOptDlg = false;
            layer.Active = true;
            layer.Name = "Edited Figure";
            layer.SquareFrameDrawer.RectangleRadiusWidth = 8;
            layer.SquareFrameDrawer.LineWidth = 1;
            layer.SquareFrameDrawer.LineColor = Color.Red;
            layer.CenterPointCoordinate.Visible = true;
            layer.CenterPointCoordinate.CoordinateFont = new Font(layer.CenterPointCoordinate.CoordinateFont, FontStyle.Bold);


            System.Threading.Thread currentThread = System.Threading.Thread.CurrentThread;
            Position_Connected_Edit mouseChickedRealPosition = null;
            IPosition nearestPositionInCurrentPositionSet = null;

            this.BeginInvoke(new dShow(MessageBox.Show), new object[] { "please double chick on the screen." });

            MouseDoubleChickOnLayersPaintedControl += delegate(object sender, MouseEventArgs e)
            {
                mouseChickedRealPosition = new Position_Connected_Edit();
                mouseChickedRealPosition.SetX(layersPaintedControl.ConvertMouseXToRealX(e.X));
                mouseChickedRealPosition.SetY(layersPaintedControl.ConvertMouseYToRealY(e.Y));

                nearestPositionInCurrentPositionSet = m2m_NN.NearestNeighbor(mouseChickedRealPosition);

                if ((currentThread.ThreadState & System.Threading.ThreadState.Suspended) == System.Threading.ThreadState.Suspended)
                {
                    currentThread.Resume();
                }
            };

            if (currentLayer != null)
            {
                layers.Remove(currentLayer);
            }
            currentLayer = layer;
            layers.Add(layer);

            currentThread.Suspend();
            MouseDoubleChickOnLayersPaintedControl = null;
            return nearestPositionInCurrentPositionSet;
        }

        //The first node of the connection to add
        private IPosition selectedNode = null;

        private void AddConnectionButton_Click(object sender, EventArgs e)
        {
            selectedNode = null;
            layersPaintedControl.state_PainterForm_MouseDown = LayersPaintedControl.State_PainterForm_MouseDown.onDaggle;
            ResetEventHandler();
            IPosition nearestPoint = new Position_Connected_Edit(0, 0);
            PositionSetEdit_ImplementByICollectionTemplate positionSetEdit_ImplementByICollectionTemplate = new PositionSetEdit_ImplementByICollectionTemplate();
            positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
            Layer_PositionSet_Square layer = new Layer_PositionSet_Square(positionSetEdit_ImplementByICollectionTemplate);

            MouseMovedRealPosition = delegate(IPosition position)
            {
                positionSetEdit_ImplementByICollectionTemplate.RemovePosition(nearestPoint);
                nearestPoint = m2m_NN.NearestNeighbor(position);
                positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
                layer.SpringLayerRepresentationChangedEvent(layer);
                layersPaintedControl.Invalidate();
            };

            layer.VisibleInOptDlg = false;
            layer.Active = true;
            layer.Name = "Edited Figure";
            layer.SquareFrameDrawer.RectangleRadiusWidth = 8;
            layer.SquareFrameDrawer.LineWidth = 1;
            layer.SquareFrameDrawer.LineColor = Color.YellowGreen;
            layer.CenterPointCoordinate.Visible = true;
            layer.CenterPointCoordinate.CoordinateFont = new Font(layer.CenterPointCoordinate.CoordinateFont, FontStyle.Bold);

            if (currentLayer != null)
            {
                layers.Remove(currentLayer);
            }
            currentLayer = layer;
            layers.Add(layer);

            MouseLBottonCheckRealPosition = delegate(IPosition position)
            {
                if (selectedNode == null)
                {
                    selectedNode = nearestPoint;
                    positionSetEdit_ImplementByICollectionTemplate.AddPosition(selectedNode);
                }
                else
                {
                    if (selectedNode != nearestPoint)
                    {
                        float x1 = selectedNode.GetX(), y1 = selectedNode.GetY(), x2 = nearestPoint.GetX(), y2 = nearestPoint.GetY();
                        float distance = (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                        if (selectedNode is IPosition_Connected_Edit)
                            ((IPosition_Connected_Edit)selectedNode).GetAdjacencyPositionSetEdit().AddAdjacency((IPosition_Connected)nearestPoint, distance);
                        else
                            throw new Exception(error_connection);
                        positionSetEdit_ImplementByICollectionTemplate.RemovePosition(selectedNode);
                        selectedNode = null;
                    }
                }

                selectedPositionSetLayer.SpringLayerRepresentationChangedEvent(selectedPositionSetLayer);
                layersPaintedControl.Invalidate();
            };
        }

        private void AddDoubleConnectionButton_Click(object sender, EventArgs e)
        {
            selectedNode = null;
            layersPaintedControl.state_PainterForm_MouseDown = LayersPaintedControl.State_PainterForm_MouseDown.onDaggle;
            ResetEventHandler();
            IPosition nearestPoint = new Position_Connected_Edit(0, 0);
            PositionSetEdit_ImplementByICollectionTemplate positionSetEdit_ImplementByICollectionTemplate = new PositionSetEdit_ImplementByICollectionTemplate();
            positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
            Layer_PositionSet_Square layer = new Layer_PositionSet_Square(positionSetEdit_ImplementByICollectionTemplate);

            MouseMovedRealPosition = delegate(IPosition position)
            {
                positionSetEdit_ImplementByICollectionTemplate.RemovePosition(nearestPoint);
                nearestPoint = m2m_NN.NearestNeighbor(position);
                positionSetEdit_ImplementByICollectionTemplate.AddPosition(nearestPoint);
                layer.SpringLayerRepresentationChangedEvent(layer);
                layersPaintedControl.Invalidate();
            };

            layer.VisibleInOptDlg = false;
            layer.Active = true;
            layer.Name = "Edited Figure";
            layer.SquareFrameDrawer.RectangleRadiusWidth = 8;
            layer.SquareFrameDrawer.LineWidth = 1;
            layer.SquareFrameDrawer.LineColor = Color.YellowGreen;
            layer.CenterPointCoordinate.Visible = true;
            layer.CenterPointCoordinate.CoordinateFont = new Font(layer.CenterPointCoordinate.CoordinateFont, FontStyle.Bold);

            if (currentLayer != null)
            {
                layers.Remove(currentLayer);
            }
            currentLayer = layer;
            layers.Add(layer);

            MouseLBottonCheckRealPosition = delegate(IPosition position)
            {
                if (selectedNode == null)
                {
                    selectedNode = nearestPoint;
                    positionSetEdit_ImplementByICollectionTemplate.AddPosition(selectedNode);
                }
                else
                {
                    if (selectedNode != nearestPoint)
                    {
                        float x1 = selectedNode.GetX(), y1 = selectedNode.GetY(), x2 = nearestPoint.GetX(), y2 = nearestPoint.GetY();
                        float distance = (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                        if (selectedNode is IPosition_Connected_Edit)
                            ((IPosition_Connected_Edit)selectedNode).GetAdjacencyPositionSetEdit().AddAdjacency((IPosition_Connected)nearestPoint, distance);
                        else
                            throw new Exception(error_connection);
                        if (nearestPoint is IPosition_Connected_Edit)
                            ((IPosition_Connected_Edit)nearestPoint).GetAdjacencyPositionSetEdit().AddAdjacency((IPosition_Connected)selectedNode, distance);
                        else
                            throw new Exception(error_connection);
                        positionSetEdit_ImplementByICollectionTemplate.RemovePosition(selectedNode);
                        selectedNode = null;
                    }
                }

                selectedPositionSetLayer.SpringLayerRepresentationChangedEvent(selectedPositionSetLayer);
                layersPaintedControl.Invalidate();
            };
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveMapDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveMapDialog.FileName.CompareTo("") != 0)
                {
                    SaveMapToFile(saveMapDialog.FileName);
                    selectedPositionSetLayer.SpringLayerRepresentationChangedEvent(selectedPositionSetLayer);
                    layersPaintedControl.Invalidate();
                }
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (loadMapDialog.ShowDialog() == DialogResult.OK)
            {
                if (loadMapDialog.FileName.CompareTo("") != 0)
                {
                    LoadMapFromFile(loadMapDialog.FileName);
                    EditedBotton_Click(sender, e);
                    selectedPositionSetLayer.SpringLayerRepresentationChangedEvent(selectedPositionSetLayer);
                    layersPaintedControl.Invalidate();
                }
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            ClearMap();
            selectedPositionSetLayer.SpringLayerRepresentationChangedEvent(selectedPositionSetLayer);
            layersPaintedControl.Invalidate();
        }

        public void SaveMapToFile(string filename)
        {
            //Create the file.
            using (FileStream fs = File.Create(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, selectedPositionSetEdit);
                fs.Close();
            }
        }

        public void LoadMapFromFile(string filename)
        {
            //Open the stream and read it back.
            using (FileStream fs = File.OpenRead(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                IPositionSet set = (IPositionSet)formatter.Deserialize(fs);
                if (set != null)
                {
                    Layer_PositionSet layer = layers.painterOptionDialog.SelectLayer as Layer_PositionSet;
                    layer.PositionSet = set;
                }
                fs.Close();
            }
        }

        public void ClearMap()
        {
            //selectedPositionSetEdit.Clear();
            if (selectedPositionSetEdit is IPositionSet_ConnectedEdit)
                ((IPositionSet_ConnectedEdit)selectedPositionSetEdit).Clear();
            else
                if (selectedPositionSetEdit is IPositionSetEdit)
                    ((IPositionSetEdit)selectedPositionSetEdit).Clear();
                else
                    throw new Exception(error_position);
        }
    }
}