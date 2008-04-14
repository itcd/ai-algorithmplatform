//------------------------------------------------------------------
//
//  For licensing information and to get the latest version go to:
//  http://www.codeplex.com/perspective
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY
//  OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
//  LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
//  FITNESS FOR A PARTICULAR PURPOSE.
//
//------------------------------------------------------------------
using System;
using System.Collections;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Perspective.Core
{
    /// <summary>
    /// A helper class for isolated storage operations, like dictionary storage.
    /// </summary>
    public static class IsolatedStorageHelper
    {
        /// <summary>
        /// Fills a dictionary with the values stored in an user-scoped isolated storage corresponding to the calling code's application identity.
        /// This requires a ClickOnce aplication (see http://blogs.msdn.com/shawnfa/archive/2006/01/18/514407.aspx).
        /// First, the dictionary is emptied.
        /// </summary>
        /// <param name="d">The dictionary to fill.</param>
        /// <param name="filename">The file name.</param>
        public static void LoadFromUserStoreForApplication(IDictionary d, string filename)
        {
            Load(d, 
                IsolatedStorageScope.Application | 
                IsolatedStorageScope.User, 
                filename);
        }

        /// <summary>
        /// Saves a dictionary in an user-scoped isolated storage corresponding to the calling code's application identity.
        /// This requires a ClickOnce aplication (see http://blogs.msdn.com/shawnfa/archive/2006/01/18/514407.aspx).
        /// </summary>
        /// <param name="d">The dictionary to save.</param>
        /// <param name="filename">The file name.</param>
        public static void SaveToUserStoreForApplication(IDictionary d, string filename)
        {
            Save(d, 
                IsolatedStorageScope.Application | 
                IsolatedStorageScope.User, 
                filename);
        }

        /// <summary>
        /// Fills a dictionary with the values stored in an user-scoped isolated storage corresponding to the application domain identity and Perspective.Core assembly identity.
        /// First, the dictionary is emptied.
        /// </summary>
        /// <param name="d">The dictionary to fill.</param>
        /// <param name="filename">The file name.</param>
        public static void LoadFromUserStoreForDomain(IDictionary d, string filename)
        {
            Load(d, 
                IsolatedStorageScope.Assembly |
                IsolatedStorageScope.Domain | 
                IsolatedStorageScope.User, 
                filename);
        }

        /// <summary>
        /// Saves a dictionary in an user-scoped isolated storage corresponding to the application domain identity and Perspective.Core assembly identity.
        /// </summary>
        /// <param name="d">The dictionary to save.</param>
        /// <param name="filename">The file name.</param>
        public static void SaveToUserStoreForDomain(IDictionary d, string filename)
        {
            Save(d, 
                IsolatedStorageScope.Assembly |
                IsolatedStorageScope.Domain | 
                IsolatedStorageScope.User, 
                filename);
        }

        /// <summary>
        /// Fills a dictionary with the values stored in an isolated store.
        /// First, the dictionary is emptied.
        /// </summary>
        /// <param name="d">The dictionary to fill.</param>
        /// <param name="scope">The degree of scope for the isolated store : a bitwise combination of the IsolatedStorageScope values.</param>
        /// <param name="filename">The file name.</param>
        public static void Load(IDictionary d, IsolatedStorageScope scope, string filename)
        {
            d.Clear();
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetStore(scope, null, null))
            {
                string[] files = storage.GetFileNames(filename);
                if ((files.Length > 0) && (files[0] == filename))
                {
                    using (Stream stream =
                        new IsolatedStorageFileStream(filename, FileMode.OpenOrCreate, storage))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        IDictionary data = (IDictionary)formatter.Deserialize(stream);
                        IDictionaryEnumerator enumerator = data.GetEnumerator();
                        while ( enumerator.MoveNext() )
                        {
                            d.Add(enumerator.Key, enumerator.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves a dictionary in an isolated store.
        /// </summary>
        /// <param name="d">The dictionary to save.</param>
        /// <param name="scope">The degree of scope for the isolated store : a bitwise combination of the IsolatedStorageScope values.</param>
        /// <param name="filename">The file name.</param>
        public static void Save(IDictionary d, IsolatedStorageScope scope, string filename)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetStore(scope, null, null);
            using (IsolatedStorageFileStream stream = 
                new IsolatedStorageFileStream(filename, FileMode.Create, storage))
            {
                {
                    IFormatter formatter = new BinaryFormatter(); 
                    formatter.Serialize(stream, d);
                }
            }
        }
    }
}
