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
using System.Collections.Generic;
using System.Collections;
using System.IO.IsolatedStorage;

namespace Perspective.Core
{
    /// <summary>
    /// Provides support for machine- and assembly-scope settings.
    /// </summary>
    public static class AssemblyConfigManager
    {
        private static string _perspectiveFilename = "Perspective.Core.dat";

        /// <summary>
        /// Static constructor. Loads the MachineStoreForAssembly dictionary from isolated storage.
        /// </summary>
        static AssemblyConfigManager()
        {
            LoadMachineStoreForAssembly();
        }

        private static Hashtable _machineStoreForAssembly = new Hashtable();

        /// <summary>
        /// Gets a machine-scoped dictionary corresponding to the Perspective.Core assembly identity.
        /// </summary>
        public static Hashtable MachineStoreForAssembly
        {
            get { return _machineStoreForAssembly; }
        }

        /// <summary>
        /// Fills the MachineStoreForAssembly dictionary with the values stored in a machine-scoped isolated storage corresponding to the Perspective.Core assembly identity.
        /// First, the dictionary is emptied.
        /// </summary>
        public static void LoadMachineStoreForAssembly()
        {
            IsolatedStorageHelper.Load(_machineStoreForAssembly,
                IsolatedStorageScope.Assembly |
                IsolatedStorageScope.Machine,
                _perspectiveFilename);
        }

        /// <summary>
        /// Saves the MachineStoreForAssembly dictionary in a machine-scoped isolated storage corresponding to the Perspective.Core assembly identity.
        /// </summary>
        public static void SaveMachineStoreForAssembly()
        {
            IsolatedStorageHelper.Save(_machineStoreForAssembly,
                IsolatedStorageScope.Assembly |
                IsolatedStorageScope.Machine,
                _perspectiveFilename);
        }
    }
}
