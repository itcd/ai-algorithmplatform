using System;
using System.Collections.Generic;
using System.Text;

using Position_Interface;
using PositionSetDrawer;
using DataStructure;
using Position_Connected_Interface;

namespace PositionSetViewer
{
    public partial class PositionSetDrawerPump
    {
        #region PositionSet_Connected

        public delegate void GetConnection(float x1, float y1, float x2, float y2);
        event GetConnection OnGetConnection;

        public void TraversePositionSet_ConnectedAndSpringEvent(IPositionSet ps)
        {
            if (OnGetConnection != null)
            {
                IPosition_Connected[] ary = (IPosition_Connected[])ps.ToArray();
                if (ary != null)
                {
                    foreach (IPosition_Connected p in ary)
                    {
                        float x = layer.ConvertPositionSetXToScreenX(p.GetX());
                        float y = layer.ConvertPositionSetYToScreenY(p.GetY());
                        IPositionSet_Connected_Adjacency adj_set = p.GetAdjacencyPositionSet();
                        adj_set.InitToTraverseSet();
                        while (adj_set.NextPosition())
                        {
                            IPosition_Connected adj = adj_set.GetPosition_Connected();
                            float adj_x = layer.ConvertPositionSetXToScreenX(adj.GetX());
                            float adj_y = layer.ConvertPositionSetYToScreenY(adj.GetY());
                            OnGetConnection(x, y, adj_x, adj_y);
                        }
                    }
                }
            }

            TraversePositionSetAndSpringEvent(ps);
        }

        public void DrawConnection(ConnectionDrawer cd)
        {
            OnGetConnection += cd.Draw;
        }

        public void ResetPump()
        {
            OnGetPoint = null;
            OnGetFirstPoint = null;
            OnGetMiddlePoint = null;
            OnGetLastPoint = null;
            OnGetConnection = null;
        }

        /// <summary>
        /// startup event pump
        /// </summary>
        public void Run()
        {
            TraversePositionSet_ConnectedAndSpringEvent(layer.PositionSet);
        }

        #endregion
    }
}
