﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace RubiksCubeSimulator
{
    [Serializable]
    public abstract class SettingsBase<T> where T : SettingsBase<T>, new()
    {
        static SettingsBase()
        {
            Load();
        }
        private static void Load()
        {
            Instance = new T();
            Instance.Reset();
        }
        public abstract void Reset();
        public static T Instance { get; private set; }
    }
}