using Atlantico.Application.DTO;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atlantico.WebApi.Configuration.HubConfig
{
    public class NotesAlertHub : Hub
    {
        public async Task NotesAlertData(ATMResponseDTO data) => await Clients.All.SendAsync("notesalertdata", data);
    }
}
