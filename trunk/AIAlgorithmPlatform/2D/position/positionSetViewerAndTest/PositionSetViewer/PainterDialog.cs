using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Position_Interface;
using Position_Implement;
using M2M;

namespace PositionSetViewer
{
    public partial class PainterDialog
    {
        #region private member

        private Layers layers = new LayersExOptDlg();

        public Layers Layers
        {
            get { return layers; }
            set { layers = value; }
        }

        private bool fillPolygon;
        public bool FillPolygon
        {
            get { return fillPolygon; }
            set { fillPolygon = value; }
        }

        private Form layersPainterForm;

        private bool isHoldOn = false;

        private bool isShowDialog = true;

        private void ShowDialog()
        {
            layersPainterForm.ShowDialog();
        }

        private void ShowForm()
        {
            ShowForm(true);
        }

        private void ShowForm(bool mFocus)
        {
            if (mFocus)
            {
                layersPainterForm.Focus();
            }

            layersPainterForm.Invalidate();
            layersPainterForm.Show();
        }

        private void IfNotHoldingShowFormInShowMode()
        {
            if (isHoldOn == false)
            {
                ShowFormInShowMode();
            }
        }

        private void ShowFormInShowMode()
        {
            if (isShowDialog)
            {
                ShowDialog();
            }
            else
            {
                ShowForm();
            }
        }

        #endregion

        #region public member

        public PainterDialog()
        {
            layersPainterForm = new LayersPainterForm((LayersExOptDlg)layers);
        }

        public void Reset()
        {
            isHoldOn = false;
            isShowDialog = true;

            Clear();
        }

        public void HoldOnMode()
        {
            isHoldOn = true;
        }

        public void NotHoldOnMode()
        {
            isHoldOn = false;
        }

        public void Show()
        {
            ShowFormInShowMode();
        }

        public void SetShowModeToDialog()
        {
            isShowDialog = true;
        }

        public void SetShowModeToForm()
        {
            isShowDialog = false;
        }

        public void Hide()
        {
            layersPainterForm.Hide();
        }

        public void Clear()
        {
            layers.Clear();
        }

        #endregion

        /// <summary>
        /// add new draw method in region below
        /// </summary>
        /// <param name="pSet">position set being draw</param>
        #region draw method

        public void DrawPositionSet(IPositionSet pSet)
        {
            layers.Add(new Layer_PositionSet_Point(new PositionSet_Cloned(pSet)));

            IfNotHoldingShowFormInShowMode();
        }

        public void DrawPath(IPositionSet pSet)
        {
            layers.Add(new Layer_PositionSet_Path(new PositionSet_Cloned(pSet)));

            IfNotHoldingShowFormInShowMode();
        }

        public void DrawConvexHull(IPositionSet pSet)
        {
            Layer_PositionSet_ConvexHull layer = new Layer_PositionSet_ConvexHull(new PositionSet_Cloned(pSet));
            layer.fillColor = fillPolygon;
            layers.Add(layer);

            IfNotHoldingShowFormInShowMode();
        }

        public void DrawSquareFrame(IPositionSet pSet)
        {
            layers.Add(new Layer_PositionSet_Square(new PositionSet_Cloned(pSet)));

            IfNotHoldingShowFormInShowMode();
        }

        public void DrawM2MLevelGrid(ILevel M2MLevel)
        {
            layers.Add(new Layer_Grid(M2MLevel));

            IfNotHoldingShowFormInShowMode();
        }

        public void DrawM2MLevel(ILevel M2MLevel)
        {
            layers.Add(new Layer_M2MLevel(M2MLevel));

            IfNotHoldingShowFormInShowMode();
        }

        public void DrawM2MStructureGrid(IM2MStructure m2mStructure)
        {
            layers.Add(new Layer_M2MStructureGrid(m2mStructure));

            IfNotHoldingShowFormInShowMode();
        }

        public void DrawM2MStructure(IM2MStructure m2mStructure)
        {
            layers.Add(new Layer_M2MStructure(m2mStructure));

            IfNotHoldingShowFormInShowMode();
        }
        
        #endregion
    }
}