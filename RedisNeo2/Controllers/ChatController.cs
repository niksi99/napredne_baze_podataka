﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RedisNeo2.Hubs;
using RedisNeo2.Models.DTOs;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using System.Security.Claims;

namespace RedisNeo2.Controllers
{
    public class ChatController : Controller
    {
        public readonly IChatServices chatService;

        public ChatController(IChatServices chatService) {
            
            this.chatService = chatService;
        }

        [Authorize]
        public IActionResult ChatPage()
        {
            //var currentLogedInUser = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //ViewBag.CurrentLogedIn = currentLogedInUser;
            return View();
        }

       

        [Authorize(Roles = "Korisnik")]
        public IActionResult ChatVjuKorisnik() {
            //string D = this.chatService.Receive().ToString();
            //string[] slice = D.Split("^");
            //ViewBag.PORUKA = slice[1];
            //ViewBag.KORISNIK = slice[0];
            return View();
        }

        [Authorize(Roles = "NGO")]
        public IActionResult ChatVjuNGO()
        {
            return View();
        }


        [HttpPost]
        public async Task<NoContentResult> Send(string poruka) {

            this.chatService.SendMessage(poruka);
            return NoContent();
            
        }

        [HttpGet]
        public async Task<PorukaDTO> Receive() {

            string D = await this.chatService.Receive();
            string[] slice = D.Split("^");
            //ViewBag.PORUKA = D;
            PorukaDTO PP = new PorukaDTO(); 
            PP.poruka = slice[1];
            PP.korisnik = slice[0];

            return PP;
        }

    }
}
