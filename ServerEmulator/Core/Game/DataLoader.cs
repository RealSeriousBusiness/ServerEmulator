﻿using ServerEmulator.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static ServerEmulator.Core.Constants;

namespace ServerEmulator.Core.Game
{
    class DataLoader
    {
        public static bool LoadingComplete { get; private set; } = false;
        internal static Dictionary<short, int[]> mapXteas = new Dictionary<short, int[]>();

        public static NPC[] Npcs;
        public static Item[] Items;
        public static GameObject[] Objects;

        public static Dictionary<int, Action> ActionButtons;
        public static Definition PlayerActions;

        internal static void LoadContent()
        {
            ObjectMap();
            MapXteas();
            Lists();
            Modules();
            Scripts();

            LoadingComplete = true;

            Program.Log("All data loaded.");
        }

        private static void Lists()
        {


            Program.Warning("Content lists could not be loaded.");
        }

        private static void Scripts()
        {

        }

        private static void Modules()
        {
            string[] files = Directory.GetFiles(PLUGIN_PATH);
            int modulesLoaded = 0;
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                try
                {
                    Assembly asm = Assembly.LoadFrom(file);
                    var types = asm.GetTypes();

                    for (int j = 0; j < types.Length; j++)
                    {
                        Type t = types[j];

                        if (typeof(IContent) == t)
                        {
                            IContent module = (IContent)Activator.CreateInstance(t);
                            module.Load();
                        }
                    }
                    modulesLoaded++;
                }
                catch
                {

                }
      
            }


            var me = Assembly.GetExecutingAssembly();
            
        }

        private static void ObjectMap()
        {
            Program.Warning("Map Verification deactivated.");
        }

        private static void MapXteas()
        {
            Program.Log("Loading Map Xtea Keys...");
            try
            {
                FileStream fs = new FileStream(DATA_PATH + "mapXteas.dat", FileMode.Open);
                RSStreamReader stream = null;// new RSStreamReader(fs);
                while (fs.Position < fs.Length)
                {
                    short area = (short)stream.ReadShort();
                    int[] parts = new int[4];
                    for (int i = 0; i < parts.Length; i++)
                        parts[i] = stream.ReadInt();
                    mapXteas.Add(area, parts);
                }
                fs.Close();
                fs.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mapdata");
            }
        }
    }
}