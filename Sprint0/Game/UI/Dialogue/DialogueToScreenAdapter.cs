using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    public class DialogueToScreenAdapter : IScreen
    {
        private Dialogue dialogue;
        public bool BlocksInput => true;
        public bool IsFinished { get; private set; }
        
            private float dismissalTimer = 0f;
        private const float DismissalDelay = 0.2f;
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;

        public DialogueToScreenAdapter(Dialogue dialogue)
        {
            this.dialogue = dialogue;
            IsFinished = false;
            dismissalTimer = 0f;
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
        }

        public void Update()
        {
            dialogue.Update();
            KeyboardState currentKbState = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();

            if (!dialogue.IsComplete)
            {
                // Check for a fresh press to force-complete the dialogue.
                bool enterFresh = currentKbState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter);
                bool spaceFresh = currentKbState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space);
                bool mouseFresh = currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed;
                if (enterFresh || spaceFresh || mouseFresh)
                {
                    dialogue.ForceComplete();
                    // Reset input states so the dismissal can be detected on the next update.
                    previousKeyboardState = new KeyboardState();
                    previousMouseState = new MouseState();
                    dismissalTimer = 0f;
                }
            }
            else
            {
                // Increase the timer once dialogue is complete.
                dismissalTimer += Globals.FRAMETIME;
                // After the delay, if any key or mouse button is pressed, dismiss the dialogue.
                if (dismissalTimer >= DismissalDelay && 
                    (currentKbState.IsKeyDown(Keys.Enter) || currentKbState.IsKeyDown(Keys.Space) || currentMouseState.LeftButton == ButtonState.Pressed))
                {
                    IsFinished = true;
                }
            }

            previousKeyboardState = currentKbState;
            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            dialogue.Draw(spriteBatch);
        }
    }
}
