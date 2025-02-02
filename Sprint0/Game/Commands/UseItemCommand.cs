using Sprint0.Interfaces;
using Sprint0.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0.Commands
{
    public class UseItemCommand : ICommand
    {
        private readonly Link player;
        private readonly int itemIndex;

        public UseItemCommand(Link player, int itemIndex)
        {
            this.player = player;
            this.itemIndex = itemIndex;
        }

        public void Execute()
        {
            player.UseItem(itemIndex);
        }
    }
}
