using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Implement;

namespace M2M
{
    /// <summary>
    /// 使用这个抽象类实现的将是形式和真实相同的M2M数据结构
    /// </summary>
    public abstract class AM2MStructure
    {
        //得到形式上的Level
        public abstract ILevel GetLevel(int levelSequecne);

        //得到形式上的Level数目
        public abstract int GetLevelNum();

        //得到形式上的子分块集合（比如形式上的子分块有可能是数据结构上的下下层后代分块集合）
        public abstract IPositionSet GetChildPositionSetByParentPart(int parentPartLevelSequence, IPart parentPart);

        public IPart GetPartRefByDescendantPart(int AncestorLevelSequence, IPart DescendantPart, int DescendantLevelSequence)
        {
            ILevel AncestorLevel = GetLevel(AncestorLevelSequence);
            int x = ((int)DescendantPart.GetX()) / (GetLevel(DescendantLevelSequence).GetUnitNumInWidth() / AncestorLevel.GetUnitNumInWidth());
            int y = ((int)DescendantPart.GetY()) / (GetLevel(DescendantLevelSequence).GetUnitNumInHeight() / AncestorLevel.GetUnitNumInHeight());
            return AncestorLevel.GetPartRefByPartIndex(x, y);
        }

        //得到形式上的父分块指针
        public IPart GetPartRefByChildPart(IPart childPart, int chlidPartSequence)
        {
            if (chlidPartSequence <= 0)
            {
                throw new Exception("chlidPartSequence <= 0");
            }

            return GetPartRefByDescendantPart(chlidPartSequence - 1, childPart, chlidPartSequence);
        }

        /// <summary>
        /// 得到属于某一分块的下层中指定层的分块集合
        /// </summary>
        /// <param name="DescendantLevelSequence"></param>
        /// <param name="AncestorPart"></param>
        /// <param name="AncestorLevelSequence"></param>
        /// <returns></returns>
        public IPositionSet GetDescendentPositionSetByAncestorPart(int DescendantLevelSequence, IPart AncestorPart, int AncestorLevelSequence)
        {
            return new PositionSet_DescendentPosition(DescendantLevelSequence, AncestorPart, AncestorLevelSequence, this);
        }

        public IPositionSet GetBottonLevelPositionSetByAncestorPart(IPart AncestorPart, int AncestorLevelSequence)
        {
            return GetDescendentPositionSetByAncestorPart(GetLevelNum(), AncestorPart, AncestorLevelSequence);
        } 

        class PositionSet_DescendentPosition : APositionSet<IPosition>, IPositionSet
        {
            int AncestorLevelSequence;
            int DescendantLevelSequence;

            AM2MStructure M2MS;

            IPositionSet childPositionSet = null;

            IPart AncestorPart;

            IPositionSet DescendentPositionSetOfChildPart = null;

            IPart childPart = null;

            private IPosition returnPosition = null;

            enum State { None, CurrentPartSubPointNumIsOne, ChildPartSubPointNumIsOne };

            State state = State.None;

            public PositionSet_DescendentPosition(int DescendantLevelSequence, IPart AncestorPart, int AncestorLevelSequence, AM2MStructure M2MS)
            {
                if (AncestorPart == null || M2MS == null)
                {
                    throw new Exception("null reference");
                }
                this.DescendantLevelSequence = DescendantLevelSequence;
                this.AncestorLevelSequence = AncestorLevelSequence;
                this.AncestorPart = AncestorPart;
                this.M2MS = M2MS;
            }

            override public void InitToTraverseSet()
            {
                if (true)
                {
                    if (AncestorLevelSequence == DescendantLevelSequence - 1)
                    {
                        DescendentPositionSetOfChildPart =
                            M2MS.GetChildPositionSetByParentPart(AncestorLevelSequence, AncestorPart);

                        DescendentPositionSetOfChildPart.InitToTraverseSet();
                    }
                    else if (AncestorLevelSequence > DescendantLevelSequence - 1)
                    {
                        throw new Exception("AncestorLevelSequence > DescendantLevelSequence - 1");
                    }
                    else
                    {
                        childPositionSet = M2MS.GetChildPositionSetByParentPart(AncestorLevelSequence, AncestorPart);

                        childPositionSet.InitToTraverseSet();

                        if (childPositionSet.NextPosition())
                        {
                            childPart = (IPart)childPositionSet.GetPosition();

                            if (true)
                            {
                                DescendentPositionSetOfChildPart =
                                    new PositionSet_DescendentPosition(DescendantLevelSequence,
                                                                       (IPart) childPositionSet.GetPosition(),
                                                                       AncestorLevelSequence + 1, M2MS);

                                DescendentPositionSetOfChildPart.InitToTraverseSet();
                            }
                            else
                            {
                                state = State.ChildPartSubPointNumIsOne;
                                state = State.None;

                            }
                        }
                    }
                }
            }

            override public bool NextPosition()
            {
                //if (state == State.ChildPartSubPointNumIsOne)
                //{
                //    returnPosition = childPart.GetRandomOneFormDescendantPoint();

                //    if (childPositionSet.NextPosition())
                //    {
                //        childPart = (IPart)childPositionSet.GetPosition();
                //        if (childPart.GetBottomLevelPointNum() != 1)
                //        {
                //            DescendentPositionSetOfChildPart =
                //                new PositionSet_DescendentPosition(DescendantLevelSequence,
                //                                                   childPart,
                //                                                   AncestorLevelSequence + 1, M2MS);

                //            DescendentPositionSetOfChildPart.InitToTraverseSet();

                //            state = State.None;
                //        }
                //        else
                //        {
                //            state = State.ChildPartSubPointNumIsOne;
                //            state = State.None;

                //        }
                //    }
                //    else
                //    {
                //        DescendentPositionSetOfChildPart = null;
                //        state = State.None;
                //    }

                //    return true;
                //}

                if(DescendentPositionSetOfChildPart == null)
                {
                    //if (state == State.CurrentPartSubPointNumIsOne)
                    //{
                    //    state = State.None;
                    //    returnPosition = AncestorPart.GetRandomOneFormDescendantPoint();
                    //    return true;
                    //}
                    return false;
                }

                if (DescendentPositionSetOfChildPart.NextPosition())
                {
                    returnPosition = DescendentPositionSetOfChildPart.GetPosition();
                    return true;
                }
                else
                {
                    if (childPositionSet == null)
                    {
                        return false;
                    }

                    if (childPositionSet.NextPosition())
                    {
                        childPart = (IPart)childPositionSet.GetPosition();
                        if (true)
                        {
                            if (AncestorLevelSequence >= DescendantLevelSequence - 2)
                            {
                                DescendentPositionSetOfChildPart =
                                    M2MS.GetChildPositionSetByParentPart(AncestorLevelSequence,
                                                                    childPart);

                                DescendentPositionSetOfChildPart.InitToTraverseSet();

                                if (DescendentPositionSetOfChildPart.NextPosition())
                                {
                                    returnPosition = DescendentPositionSetOfChildPart.GetPosition();
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                DescendentPositionSetOfChildPart =
                                    new PositionSet_DescendentPosition(DescendantLevelSequence,
                                                                       childPart,
                                                                       AncestorLevelSequence + 1, M2MS);

                                DescendentPositionSetOfChildPart.InitToTraverseSet();

                                if (DescendentPositionSetOfChildPart.NextPosition())
                                {
                                    returnPosition = DescendentPositionSetOfChildPart.GetPosition();
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            override public IPosition GetPosition()
            {
                return returnPosition;
            }
        }
    }
}