using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M2M.Position.Interface
{
    public interface IPosition_Connected : IPosition
    {
        /// <summary>
        /// Get a list of the edges that start from this position 
        /// </summary>
        /// <returns></returns>
        ICollection<IAdjacency> GetAdjacencyOut();

        /// <summary>
        /// Get a list of the edges that end at this position 
        /// </summary>
        /// <returns></returns>
        ICollection<IAdjacency> GetAdjacencyIn();

        /// <summary>
        /// Get the tag index of this position.
        /// The tag is used to store addition information about this position.
        /// </summary>
        /// <returns></returns>
        int GetTagIndex();

        /// <summary>
        /// Set the tag index of this position.
        /// The tag is used to store addition information about this position.
        /// </summary>
        /// <param name="index"></param>
        void SetTagIndex(int index);
    }
}
