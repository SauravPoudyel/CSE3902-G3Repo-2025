using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    public class LevelManager
    {
        private Level level;

        public LevelManager()
        {
            level = new Level();
        }

        public void LoadContent(ContentManager content)
        {
            // Compute the file path for the level CSV.
            string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            string filePath = Path.Combine(projectDirectory, "Content\\LevelParseTest1.csv");
            level = CSVParser.ParseLevel(filePath, content);
        }

        public Dictionary<string, Entity> LoadLevelEntities()
        {
            return level.GetLevelEntities();
        }
    }
}
   
