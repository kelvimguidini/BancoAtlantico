using Atlantico.Application.DTO;
using Atlantico.Application.Interfaces;
using Atlantico.CrossCutting.Massages.Interfaces;
using Atlantico.WebApi.Configuration.HubConfig;
using Atlantico.WebApi.Configuration.TimerFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;

namespace Atlantico.WebApi.Controllers
{
    public class ManegerController : MainController
    {
        private readonly IATMService _atmService;
        private readonly INotificator _notificator;

            
            public ManegerController(IATMService atmService,
            INotificator notification) : base(notification)
        {
            _atmService = atmService;
            _notificator = notification;
        }


        /// <summary>
        /// Autenticação na API
        /// </summary>
        [HttpGet]
        [Route("maneger/getactveatm")]
        [Authorize]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<ATMResponseDTO>), StatusCodes.Status200OK)]
        public IActionResult GetActveATM()
        {
            List<ATMResponseDTO> result = new List<ATMResponseDTO>();
            try
            {
                result = _atmService.GetActveATM();
                //var timerManager = new TimerManager(() => ));
            }
            catch (Exception e)
            {
                _notificator.notify(e.Message);
            }
            return CustomResponse(result);
        }



        /// <summary>
        /// Autenticação na API
        /// </summary>
        [HttpGet]
        [Route("maneger/CountNotesAvailable")]
        [Authorize]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<ResponseDTO>), StatusCodes.Status200OK)]
        public IActionResult CountNotesAvailable([FromBody] int id)
        {
            List<ResponseDTO> result = new List<ResponseDTO>();
            try
            {
                result = _atmService.CountNotes(id);
            }
            catch (Exception e)
            {
                _notificator.notify(e.Message);
            }
            return CustomResponse(result);
        }



        /// <summary>
        /// Autenticação na API
        /// </summary>
        [HttpPost]
        [Route("maneger/turnoffatm")]
        [Authorize]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public IActionResult TurnOffATM([FromBody] int id)
        {
            bool result = false;
            try
            {
                result = _atmService.turnOff(id);
            }
            catch (Exception e)
            {
                _notificator.notify(e.Message);
            }
            return CustomResponse(result);
        }
    }
}