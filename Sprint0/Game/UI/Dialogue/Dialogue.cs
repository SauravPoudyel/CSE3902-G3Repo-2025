using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class Dialogue : IHUD
    {
        private Sprite circleSprite;
        private Texture2D rectangleTexture;
        private SpriteFont font;
        private string fullText;
        internal int charactersDisplayed;
        private float typeTimer;
        private float charInterval = 0.05f;
        private Vector2 circlePosition;
        private Vector2 speechBubblePosition;
        private Vector2 speechBubbleSize;
        private float maxTextWidth;
        private bool soundStarted = false;

        public Dialogue(Sprite circleSprite, Texture2D rectangleTexture, SpriteFont font, string text)
        {
            this.circleSprite = circleSprite;
            this.rectangleTexture = rectangleTexture;
            this.font = font;
            this.fullText = text;
            this.charactersDisplayed = 0;
            this.typeTimer = 0f;

            int circleDiameter = 140;
            int marginX = 30;
            int marginY = 50;

            circlePosition = new Vector2(Globals.SCREENWIDTH - circleDiameter + 40, marginY + circleDiameter / 2);
            // Speech bubble: placed to the left of the circle by (circleDiameter + 40)
            speechBubbleSize = new Vector2(380, 160);
            speechBubblePosition = new Vector2(Globals.SCREENWIDTH - circleDiameter + 30 - speechBubbleSize.X - 80, marginY);
            maxTextWidth = speechBubbleSize.X - 20;
        }

        public void Update()
        {
            typeTimer += Globals.FRAMETIME;
            if (typeTimer >= charInterval && charactersDisplayed < fullText.Length)
            {
                charactersDisplayed++;
                typeTimer = 0f;
            }

            // Start playing dialogue sound if not already started and dialogue not complete.
            if (!soundStarted && !IsComplete)
            {
                AudioManager.PlaySound(AudioManager.SoundKey.Dialogue);
                soundStarted = true;
            }
            // Once dialogue is complete, stop the dialogue sound.
            if (IsComplete && soundStarted)
            {
                AudioManager.StopDialogue();
                soundStarted = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle bubbleRect = new Rectangle((int)speechBubblePosition.X, (int)speechBubblePosition.Y,
                                                  (int)speechBubbleSize.X, (int)speechBubbleSize.Y);
            spriteBatch.Draw(rectangleTexture, bubbleRect, new Color(64, 64, 64, 200));

            string textToDisplay = fullText.Substring(0, charactersDisplayed);
            string wrappedText = WrapText(font, textToDisplay, maxTextWidth);
            Vector2 textPosition = speechBubblePosition + new Vector2(10, 10);
            spriteBatch.DrawString(font, wrappedText, textPosition, Color.White);

            // Draw the character circle using the Sprite.
            circleSprite.Draw(spriteBatch, circlePosition, scale: 1f);
        }

        private string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            string result = "";
            string line = "";
            foreach (var word in words)
            {
                string testLine = (line.Length == 0) ? word : line + " " + word;
                if (spriteFont.MeasureString(testLine).X > maxLineWidth)
                {
                    result += line + "\n";
                    line = word;
                }
                else
                {
                    line = testLine;
                }
            }
            result += line;
            return result;
        }

        public bool IsComplete => charactersDisplayed >= fullText.Length;

        public void ForceComplete()
        {
            charactersDisplayed = fullText.Length;
        }
    }
}
