using Sprint0.Interfaces;
using Sprint0.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0.Commands
{
    public class AttackCommand : ICommand
    {
        private readonly Link player;

        public AttackCommand(Link player)
        {
            this.player = player;
        }

        public void Execute()
        {
            player.Attack();
        }
    }
}
