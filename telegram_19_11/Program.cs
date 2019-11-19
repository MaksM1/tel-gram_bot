using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace telegram_19_11
{
    class Program
    {
        static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            Bot =new TelegramBotClient( "965485992:AAGz1PR9cU9VC_O4bxsLsO6rmaPZhGKJhAQ");
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery += BotOncallbackQueryRece;
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine( me.FirstName);

            Bot.StartReceiving();
            Console.ReadLine();

        }

        private static void BotOncallbackQueryRece(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            //var message =
        }

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if(message.Type != MessageType.Text||message==null)
            {
                return;
            }
            string name = $"{message.From.FirstName},{message.From.LastName}";
            
            Console.WriteLine($"{name} отправил собщение: '{message.Text}'");
            switch (message.Text){
                case"/start":
                    string text =
 @"Список команд:
/start - запуск бота;
/inline - вывод меню;
/keyboard - вывод клавиатуры; ";

                    await Bot.SendTextMessageAsync(message.From.Id, text);   
                    break;
                case "/inline":
                    break;
                case"/keyboard":
                    break;
              
                default:
                    break;
            }
        }
    }
}
