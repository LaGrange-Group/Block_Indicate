using Block_Indicate.Data;
using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Block_Indicate.Class
{
    public class Notify
    {
        public async Task TelegramAsync(int customerId, string message)
        {
            ChatId chatId;
            using (var db = new ApplicationDbContext())
            {
                chatId = db.Notifications.Where(n => n.CustomerId == customerId).Select(n => n.TelegramChatId).Single();
            }
            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
            await botClient.SendTextMessageAsync(
              chatId: chatId,
              text: message
            );
        }
        public void Telegram(int customerId, string message)
        {
            ChatId chatId;
            using (var db = new ApplicationDbContext())
            {
                chatId = db.Notifications.Where(n => n.CustomerId == customerId).Select(n => n.TelegramChatId).Single();
            }
            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
            botClient.SendTextMessageAsync(
              chatId: chatId,
              text: message
            );
        }
        //public async Task EmailAsync(int customerId, string message)
        //{
        //    int chatId;
        //    using (var db = new ApplicationDbContext())
        //    {
        //        //chatId = db.Notifications.Where(n => n.CustomerId == customerId).Select(n => n.TelegramChatId).Single();
        //    }
        //    var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
        //    await botClient.SendTextMessageAsync(
        //      chatId: chatId,
        //      text: message
        //    );
        //}
        //public void Email(int customerId, string message)
        //{
        //    int chatId;
        //    using (var db = new ApplicationDbContext())
        //    {
        //        //chatId = db.Notifications.Where(n => n.CustomerId == customerId).Select(n => n.TelegramChatId).Single();
        //    }
        //    var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
        //    botClient.SendTextMessageAsync(
        //      chatId: chatId,
        //      text: message
        //    );
        //}
    }
}
