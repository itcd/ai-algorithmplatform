using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.ComponentModel;

using Position_Interface;
using PositionSetDrawer;
using DataStructure;
using Position_Connected_Interface;
using M2M;
using PositionSetUtils;
using Position_Connected_Implement;
using QuickHullAlgorithm;
using DrawingLib;

namespace PositionSetViewer
{
    public class Layer_PositionSet_Connected : Layer_PositionSet
    {
        PositionSetDrawerPump positionSetDrawerPump;

        public Layer_PositionSet_Connected(IPositionSet_Connected positionSet)
            : base(positionSet)
        {
            positionSetDrawerPump = new PositionSetDrawerPump(this);

            pointCoordinateDrawer.Visible = false;
            
            connectionDrawer.LineWidth = 1;
            pointDrawer.IsDrawPointBorder = true;
            pointDrawer.PointRadius = 2;
        }

        public override Color MainColor
        {
            get { return connectionDrawer.LineColor; }
            set
            {
                connectionDrawer.LineColor = value;
                SpringLayerRepresentationChangedEvent(this);
            }
        }

        PointDrawer pointDrawer = new PointDrawer();
        public PointDrawer Point
        {
            get { return pointDrawer; }
            set
            {
                pointDrawer = value;
                SpringLayerRepresentationChangedEvent(this);
            }
        }

        CoordinateDrawer pointCoordinateDrawer = new CoordinateDrawer();
        public CoordinateDrawer PointCoordinate
        {
            get { return pointCoordinateDrawer; }
            set
            {
                pointCoordinateDrawer = value;
                SpringLayerRepresentationChangedEvent(this);
            }
        }

        ConnectionDrawer connectionDrawer = new ConnectionDrawer();
        public ConnectionDrawer Connection
        {
            get { return connectionDrawer; }
            set
            {
                connectionDrawer = value;
                SpringLayerRepresentationChangedEvent(this);
            }
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                SetDrawerGraphic(graphics);

                positionSetDrawerPump.ResetPump();
                positionSetDrawerPump.DrawCoordinate(pointCoordinateDrawer);
                positionSetDrawerPump.DrawPoint(pointDrawer);
                positionSetDrawerPump.DrawConnection(connectionDrawer);
                positionSetDrawerPump.Run();
            }
        }
    }
    
    public class Layer_PartSet_Connected : Layer_PositionSet
    {
        PositionSetDrawerPump positionSetDrawerPump;

        IM2MStructure m2mStructure;

        int levelSequence = 0;

        public Layer_PartSet_Connected(IM2MStructure m2mStructure, int levelSequence)
        {
            this.m2mStructure = m2mStructure;
            this.levelSequence = levelSequence;

            positionSetDrawerPump = new PositionSetDrawerPump(this);

            pointCoordinateDrawer.Visible = false;

            connectionDrawer.LineWidth = 1;
            pointDrawer.IsDrawPointBorder = true;
            pointDrawer.PointRadius = 1;

            PartColor = GenerateColor.GetSimilarColor(MainColor);
        }

        [BrowsableAttribute(false)]
        public override RectangleF MaxRect
        {
            get
            {
                return new RectangleF();
            }
        }

        public override Color MainColor
        {
            get { return connectionDrawer.LineColor; }
            set
            {
                PartColor = value;
                connectionDrawer.LineColor = value;
                SpringLayerRepresentationChangedEvent(this);
            }
        }

        PointDrawer pointDrawer = new PointDrawer();
        public PointDrawer Point
        {
            get { return pointDrawer; }
            set
            {
                pointDrawer = value;
                SpringLayerRepresentationChangedEvent(this);
            }
        }

        CoordinateDrawer pointCoordinateDrawer = new CoordinateDrawer();
        public CoordinateDrawer PointCoordinate
        {
            get { return pointCoordinateDrawer; }
            set
            {
                pointCoordinateDrawer = value;
                SpringLayerRepresentationChangedEvent(this);
            }
        }

        ConnectionDrawer connectionDrawer = new ConnectionDrawer();
        public ConnectionDrawer Connection
        {
            get { return connectionDrawer; }
            set
            {
                connectionDrawer = value;
                SpringLayerRepresentationChangedEvent(this);
            }
        }

        Color partColor = Color.Black;
        [CategoryAttribute("Drawer")]
        public Color PartColor
        {
            get { return partColor; }
            set { partColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        float partBorderWidth = 1.0f;
        [CategoryAttribute("Drawer")]
        public float PartBorderWidth
        {
            get { return partBorderWidth; }
            set { partBorderWidth = value; SpringLayerRepresentationChangedEvent(this); }
        }

        int alpha = 150;
        [CategoryAttribute("Drawer")]
        public int Alpha
        {
            get { return alpha; }
            set
            {
                if (value > 255)
                {
                    alpha = 255;
                }
                else if (value < 0)
                {
                    alpha = 0;
                }
                else
                {
                    alpha = value;
                }

                SpringLayerRepresentationChangedEvent(this);
            }
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                SetDrawerGraphic(graphics);

                List<IPosition_Connected> PartMiddlePointList = new List<IPosition_Connected>();

                List<PointF[]> PointArrayList = new List<PointF[]>();

                ILevel level = m2mStructure.GetLevel(levelSequence);
                IPart rootPart = m2mStructure.GetLevel(0).GetPartRefByPartIndex(0, 0);
                IPositionSet positionSet = m2mStructure.GetDescendentPositionSetByAncestorPart(levelSequence, rootPart, 0);

                Dictionary<IPosition, IPosition_Connected_Edit> partToPositionDictionary = new Dictionary<IPosition, IPosition_Connected_Edit>();

                positionSet.InitToTraverseSet();
                while (positionSet.NextPosition())
                {
                    Position_Connected_Edit PartDeputyPosition_Connected = new Position_Connected_Edit();
                    partToPositionDictionary.Add(positionSet.GetPosition(), PartDeputyPosition_Connected);
                }

                positionSet.InitToTraverseSet();

                while (positionSet.NextPosition())
                {
                    IPart_Connected currentPart = (IPart_Connected)positionSet.GetPosition();

                    IPositionSet bottomLevelPositionSet = m2mStructure.GetBottonLevelPositionSetByAncestorPart(currentPart, levelSequence);

                    IPosition tempPoint = PositionSetAttribute.GetGravityCenter(bottomLevelPositionSet);

                    IPosition_Connected_Edit PartDeputyPosition_Connected;
                    partToPositionDictionary.TryGetValue(positionSet.GetPosition(), out PartDeputyPosition_Connected);
                    PartDeputyPosition_Connected.SetX(tempPoint.GetX());
                    PartDeputyPosition_Connected.SetY(tempPoint.GetY());


                    currentPart.GetAdjacencyPositionSet().InitToTraverseSet();

                    while (currentPart.GetAdjacencyPositionSet().NextPosition())
                    {
                        IPosition_Connected_Edit adjDeputy;
                        partToPositionDictionary.TryGetValue(currentPart.GetAdjacencyPositionSet().GetPosition(), out adjDeputy);

                        PartDeputyPosition_Connected.GetAdjacencyPositionSetEdit().AddAdjacency(
                            adjDeputy, currentPart.GetAdjacencyPositionSet().GetDistanceToAdjacency());
                    }

                    PartMiddlePointList.Add(PartDeputyPosition_Connected);

                    IPositionSet ConvexHullPointSet = null;

                    if (bottomLevelPositionSet.GetNum() > 1)
                    {
                        ConvexHullPointSet = new QuickHull().ConvexHull(bottomLevelPositionSet);
                    }
                    else
                    {
                        ConvexHullPointSet = bottomLevelPositionSet;
                    }

                    PointF[] tempArray = new PointF[ConvexHullPointSet.GetNum()];

                    int sequence = 0;
                    ConvexHullPointSet.InitToTraverseSet();
                    while (ConvexHullPointSet.NextPosition())
                    {
                        tempArray[sequence].X = ConvertRealXToScreenX(ConvexHullPointSet.GetPosition().GetX());
                        tempArray[sequence].Y = ConvertRealYToScreenY(ConvexHullPointSet.GetPosition().GetY());
                        sequence++;
                    }

                    PointArrayList.Add(tempArray);
                }

                Pen pen = new Pen(partColor, partBorderWidth);
                Brush brush = new SolidBrush(Color.FromArgb(alpha, partColor));

                foreach(PointF[] pointArray in PointArrayList)
                {
                    if (pointArray.Length >= 2)
                    {
                        graphics.DrawPolygon(pen, pointArray);
                        graphics.FillPolygon(brush, pointArray);
                    }
                }

                this.positionSet = new PositionSet_ConnectedEdit(PartMiddlePointList);

                positionSetDrawerPump.ResetPump();
                positionSetDrawerPump.DrawCoordinate(pointCoordinateDrawer);
                positionSetDrawerPump.DrawPoint(pointDrawer);
                positionSetDrawerPump.DrawConnection(connectionDrawer);
                positionSetDrawerPump.Run();
            }
        }
    }
}
