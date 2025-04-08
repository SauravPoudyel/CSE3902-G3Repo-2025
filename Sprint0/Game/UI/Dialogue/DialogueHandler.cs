using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class DialogueHandler
    {
        private ContentManager content;
        private Game1 game;
        private Dictionary<string, DialogueData> dialogueDictionary;
        private bool dialoguesLoaded = false;
        private Texture2D characterCircleSheet;
        private enum CharacterPortrait
        {
            Player = 0,
            Commander = 1
        }
        private Dictionary<string, DialogueToScreenAdapter> activeDialogueAdapters;

        public DialogueHandler(ContentManager content, Game1 game)
        {
            this.content = content;
            this.game = game;
            activeDialogueAdapters = new Dictionary<string, DialogueToScreenAdapter>();
        }

        private void LoadDialogues(string csvPath)
        {
            dialogueDictionary = CSVDialogueParser.ParseDialogueCSV(csvPath);
            dialoguesLoaded = true;
        }

        private StaticSprite CreatePortraitSprite(string characterName)
        {
            if (characterCircleSheet == null)
                characterCircleSheet = content.Load<Texture2D>("CharacterCircle");

            StaticSprite sprite = new StaticSprite();
            int spriteSize = 150;

            CharacterPortrait portraitEnum = CharacterPortrait.Player;
            if (characterName.ToLower().Contains("commander"))
                portraitEnum = CharacterPortrait.Commander;

            int startX = (int)portraitEnum * spriteSize;
            int startY = 0;

            sprite.spriteSheet = characterCircleSheet;
            sprite.LoadContent(content, "CharacterCircle", startX, startY, spriteSize, spriteSize, 1);

            return sprite;
        }

        private void ShowDialogue(ScreenManager screenManager, DialogueData data)
        {
            // Remove any quotation marks and format the dialogue text.
            string finalText = data.Text.Replace("\"", "");
            finalText = finalText.Replace("{playerName}", Globals.PlayerData.GetString("Name"));
            finalText = $"{data.Character}:\n\n" + finalText;

            StaticSprite circleSprite = CreatePortraitSprite(data.Character);
            Texture2D rectangleTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new Color[] { Color.White });
            SpriteFont font = Globals.FONT;

            Dialogue dialogue = new Dialogue(circleSprite, rectangleTexture, font, finalText);
            DialogueToScreenAdapter adapter = new DialogueToScreenAdapter(dialogue);

            screenManager.AddScreen(adapter, true);
            activeDialogueAdapters[data.Key] = adapter;
        }

        public void AddDialogueByKey(string key, ScreenManager screenManager)
        {
            if (!dialoguesLoaded)
            {
                string dialogueCSVPath = Path.Combine(Globals.projectDirectory, "Data", "DialogueData.csv");
                LoadDialogues(dialogueCSVPath);
            }

            if (dialogueDictionary.TryGetValue(key, out DialogueData value))
            {
                DialogueData data = value;
                /* even if automatic trigger check would normally skip this dialogue, manual addition 
                will force it. */
                if (!activeDialogueAdapters.ContainsKey(key))
                {
                    ShowDialogue(screenManager, data);
                }
            }
            else
            {
                System.Console.WriteLine($"[DialogueHandler] Dialogue key '{key}' not found.");
            }
        }

        private bool ShouldTriggerDialogue(DialogueData data)
        {
            // For automatic triggers we skip dialogues meant to be externally triggered.
            string key = data.Key.Trim().ToUpper();
            string trigger = data.Trigger?.Trim().ToUpper();
            if (key == "DEATH" || trigger == "N/A" || string.IsNullOrEmpty(trigger))
                return false;

            // For tutorial dialogues, compare against TutorialDialogueCount.
            if (data.Key.StartsWith("Tutorial"))
            {
                if (int.TryParse(data.Key.Substring("Tutorial".Length), out int tutorialNumber))
                {
                    int currentCount = Globals.PlayerData.GetInt("TutorialDialogueCount");
                    return (currentCount == tutorialNumber);
                }
                return false;
            }
            // For other dialogues, trigger if the associated trigger variable equals 0.
            return (Globals.PlayerData.GetInt(data.Trigger) == 0);
        }

        public void Update(ScreenManager screenManager, int levelNumber, bool isPaused, bool gameStarted)
        {
            if (!gameStarted || isPaused)
                return;

            if (!dialoguesLoaded)
            {
                string dialogueCSVPath = Path.Combine(Globals.projectDirectory, "Data", "DialogueData.csv");
                LoadDialogues(dialogueCSVPath);
            }

            if (activeDialogueAdapters.Count > 0)
            {
                foreach (var pair in activeDialogueAdapters)
                {
                    DialogueToScreenAdapter adapter = pair.Value;
                    if (adapter.IsFinished)
                    {
                        screenManager.RemoveScreen(adapter);
                        // Update trigger values when a dialogue is completed.
                        if (pair.Key.StartsWith("Tutorial"))
                        {
                            if (int.TryParse(pair.Key.Substring("Tutorial".Length), out int tutorialNumber))
                            {
                                int currentCount = Globals.PlayerData.GetInt("TutorialDialogueCount");
                                if (currentCount == tutorialNumber)
                                {
                                    int newValue = tutorialNumber - 1;
                                    Globals.PlayerData.SetInt("TutorialDialogueCount", newValue);
                                }
                            }
                        }
                        else
                        {
                            if (dialogueDictionary.TryGetValue(pair.Key, out DialogueData data))
                            {
                                Globals.PlayerData.SetInt(data.Trigger, 1);
                            }
                        }
                        activeDialogueAdapters.Clear();
                        return; 
                    }
                    else
                    {
                        return;
                    }
                    break; 
                }
            }

            // scan through dialogues and add the first that should trigger.
            foreach (var kvp in dialogueDictionary)
            {
                DialogueData data = kvp.Value;
                if (ShouldTriggerDialogue(data) && !activeDialogueAdapters.ContainsKey(data.Key))
                {
                    ShowDialogue(screenManager, data);
                    break; 
                }
            }
        }
    }
}
