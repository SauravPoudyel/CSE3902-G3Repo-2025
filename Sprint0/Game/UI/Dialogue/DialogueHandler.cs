using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class DialogueHandler
    {
        private readonly ContentManager content;
        private readonly GraphicsDevice graphicsDevice;
        private Dictionary<string, DialogueData> dialogueDictionary;
        private readonly Dictionary<string, DialogueToScreenAdapter> activeDialogueAdapters;
        private bool dialoguesLoaded;

        public DialogueHandler(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            dialogueDictionary = new Dictionary<string, DialogueData>();
            activeDialogueAdapters = new Dictionary<string, DialogueToScreenAdapter>();
            dialoguesLoaded = false;
        }

        // Called by your commands:
        public void AddDialogueByKey(string key, ScreenManager screenManager)
        {
            EnsureDialoguesLoaded();
            if (!dialogueDictionary.TryGetValue(key, out var data)) return;
            if (activeDialogueAdapters.ContainsKey(key)) return;

            ShowDialogue(screenManager, data);
        }

        // Optional: if you still use Update() to auto-trigger elsewhere
        public void Update(ScreenManager screenManager, int levelNumber, bool isPaused, bool gameStarted)
        {
            if (!gameStarted || isPaused) return;
            EnsureDialoguesLoaded();

            if (activeDialogueAdapters.Count > 0)
            {
                foreach (var pair in activeDialogueAdapters)
                {
                    if (!pair.Value.IsFinished) return;
                    screenManager.RemoveScreen(pair.Value);
                    activeDialogueAdapters.Clear();
                    return;
                }
            }
        }

        private void EnsureDialoguesLoaded()
        {
            if (dialoguesLoaded) return;
            string path = Path.Combine(Globals.projectDirectory, "Data", "DialogueData.csv");
            dialogueDictionary = CSVDialogueParser.ParseDialogueCSV(path);
            dialoguesLoaded = true;
        }

        private void ShowDialogue(ScreenManager screenManager, DialogueData data)
        {
            Sprite circle = CreatePortraitSprite(data.Character);

            var rectTex = new Texture2D(graphicsDevice, 1, 1);
            rectTex.SetData(new[] { Color.White });

            SpriteFont font = Globals.FONT;
            string fullText = $"{data.Character}:\n\n{data.Text}";

            // create and show dialoguie
            var dialogue = new Dialogue(circle, rectTex, font, fullText);
            var adapter  = new DialogueToScreenAdapter(dialogue);
            screenManager.AddScreen(adapter, true);
            activeDialogueAdapters[data.Key] = adapter;
        }

        private Sprite CreatePortraitSprite(string characterName)
        {
            Texture2D sheet = content.Load<Texture2D>("CharacterCircle");
            const int size = 150;
            int index = characterName.ToLower().Contains("commander") ? 1 : 0;

            var sprite = new Sprite(0.4f);
            sprite.spriteSheet = sheet;
            sprite.LoadContent(content, "CharacterCircle", index * size, 0, size, size, 1);
            return sprite;
        }
    }
}
