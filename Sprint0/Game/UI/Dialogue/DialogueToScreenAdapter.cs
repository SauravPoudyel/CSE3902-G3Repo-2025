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

            if (dialogue.IsComplete)
                dismissalTimer += Globals.FRAMETIME;
            else
                dismissalTimer = 0f;

            bool enterFresh = currentKbState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter);
            bool spaceFresh = currentKbState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space);
            bool mouseFresh = currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed;

            if (!dialogue.IsComplete)
            {
                if (enterFresh || spaceFresh || mouseFresh)
                {
                    dialogue.ForceComplete();
                    dismissalTimer = 0f;
                }
            }
            else
            {
                if (dismissalTimer >= DismissalDelay && (enterFresh || spaceFresh || mouseFresh))
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
