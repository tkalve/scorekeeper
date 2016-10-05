using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace ScoreKeeper.Hubs
{
    public class ScoreHub : Hub
    {

        public override Task OnConnected()
        {
            Clients.Caller.update(
                GameHub.Instance.CurrentGame.BlueTeamName,
                GameHub.Instance.CurrentGame.WhiteTeamName,
                GameHub.Instance.CurrentGame.BlueTeamGoals,
                GameHub.Instance.CurrentGame.WhiteTeamGoals,
                $"{GameHub.Instance.CurrentGame.TimeLeft.Minutes}:{GameHub.Instance.CurrentGame.TimeLeft.Seconds:00}",
                GameHub.Instance.CurrentGame.CurrentRound
                );

            return base.OnConnected();
        }
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
    }
}
