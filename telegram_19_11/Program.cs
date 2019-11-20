using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using ApiAiSDK;
using ApiAiSDK.Model;




namespace telegram_19_11
{
    class Program
    {
        static TelegramBotClient Bot;
        static ApiAi apiAi;
        static void Main(string[] args)
        {

           
            Bot = new TelegramBotClient("965485992:AAGz1PR9cU9VC_O4bxsLsO6rmaPZhGKJhAQ");

            AIConfiguration config = new AIConfiguration("74026b4e86854e9eaba95dcfed472e92", SupportedLanguage.Russian);
            apiAi = new ApiAi(config);

            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery += BotOncallbackQueryRece;
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);

            Bot.StartReceiving();
            Console.ReadLine();

        }

        private static async void BotOncallbackQueryRece(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string button = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName} {e.CallbackQuery.From.Username}";
            Console.WriteLine($"{name} позьзователь нажал {button}");
            await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нжали кнопку {button}");
        }

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message.Type != MessageType.TextMessage || message == null)
            {
                return;
            }
            string name = $"{message.From.FirstName},{message.From.LastName}";

            Console.WriteLine($"{name} отправил собщение: '{message.Text}'");
            switch (message.Text) {
                case "/start":
                    string text =
 @"Список команд:
/start - запуск бота;
/BackMenu - вывод меню;
/keyboard - вывод клавиатуры; ";
                    await Bot.SendTextMessageAsync(message.From.Id, text);
                    break;
                case "/BackMenu":
                    var InlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]{
                           InlineKeyboardButton.WithUrl("Telegram", "https://t.me/maks_mischuk")
                        },
                        new[]
                        {
                           InlineKeyboardButton.WithCallbackData("Пункт 1")
                        }
                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выберите пункт", replyMarkup: InlineKeyboard);
                    break;
                case "/keyboard":
                    
                    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                         new KeyboardButton("Привет"),
                         new KeyboardButton("Какие ваши хобби?")
                         },
                        new[]
                        {
                          new KeyboardButton("Контакт"){RequestContact =true },
                          new KeyboardButton("Геолокация"){RequestLocation = true}
                          }

                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сообщение", replyMarkup: replyKeyboard);
                    break;
              
                default:
                    var response = apiAi.TextRequest(message.Text);
                    string answer = response.Result.Fulfillment.Speech;
                    if (answer == "")
                    
                        answer = "Прости,я тебя не понял";

                        await Bot.SendTextMessageAsync(message.From.Id, answer);
                    
                    break;
            }
        }
    }
}
