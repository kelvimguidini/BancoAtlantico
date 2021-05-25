using Atlantico.Application.DTO;
using Atlantico.Application.Interfaces;
using Atlantico.CrossCutting.Massages.Interfaces;
using Atlantico.WebApi.Configuration.HubConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;

namespace Atlantico.WebApi.Controllers
{
    public class ATMController : MainController
    {
        private readonly IATMService _atmService;
        private readonly INotificator _notificator;
        private readonly IHubContext<NotesAlertHub> _hub;

        public ATMController(IATMService atmService,
            INotificator notification,
            IHubContext<NotesAlertHub> hub) : base(notification)
        {
            _atmService = atmService;
            _notificator = notification;
            _hub = hub;
        }

        /// <summary>
        /// Autenticação na API
        /// </summary>
        [HttpPost]
        [Route("atm/withdraw")]
        [Authorize]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<ResponseDTO>), StatusCodes.Status200OK)]
        public IActionResult Withdraw([FromBody] WithdrawDTO withdraw)
        {
            List<ResponseDTO> result = new List<ResponseDTO>();
            try
            {
                result = _atmService.Withdraw(withdraw);
                _hub.Clients.All.SendAsync("transfernotesalertdata", _atmService.GetActveATM());
            }
            catch (Exception e)
            {
                _notificator.notify(e.Message);
            }
            return CustomResponse(result);
        }
    }
}
