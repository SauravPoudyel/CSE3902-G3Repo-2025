using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Sprint0;
public interface ICommand
    {
        void Execute(Dictionary<string, object> parameters);
    }