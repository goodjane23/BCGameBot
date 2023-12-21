using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WebApplication1.Services;

public class CoreService : IHostedService
{
    private readonly ITelegramBotClient сlient;
    private readonly BCGameService game;
    private readonly RedisService redisService;

    public CoreService(
        ITelegramBotClient сlient, 
        BCGameService bCGameService,
        RedisService redisService)        
    {
        this.сlient = сlient;
        this.game = bCGameService;
        this.redisService = redisService;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            сlient.StartReceiving(UpdateHandler, 
                ErrorHandler,
                cancellationToken:cancellationToken);
            

        }
        catch (Exception)
        {

        }
        return Task.CompletedTask;
    }

    private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
    {
        var message = update.Message;
        var chat = message.Chat;
        try
        {
            if (update.Type is UpdateType.Message)
            {
                //commands
                if (message.Text.Equals("/start", StringComparison.OrdinalIgnoreCase))
                {
                    await client.SendTextMessageAsync(
                        chat.Id,
                        Strings.StartText);
                    var quiz = await game.GenerateNum();
                    redisService.SetData(chat.Id, quiz);

                }

                if (message.Text.Equals("/rules", StringComparison.OrdinalIgnoreCase))
                {
                    await client.SendTextMessageAsync(
                        chat.Id,
                        Strings.RulesText);
                }
               
                var stepResult = game.CheckAnswer(update.Message.Text);
                var messageFin = string.Empty;
               
                if (!stepResult.IsWin)
                {
                    messageFin = $"{message.Text} | Быков: {stepResult.Nums[0]}, коров: {stepResult.Nums[1]}";
                }
                else
                {
                    KeyboardButton keyboardButton = new KeyboardButton("Еще раз");
                    var replayMarkup = new ReplyKeyboardMarkup(keyboardButton);
                    await client.SendTextMessageAsync(
                        chat.Id, Strings.FinishText,
                        replyMarkup: replayMarkup);                    
                }
                await client.SendTextMessageAsync(
                        chat.Id,
                        messageFin);
            }
        }
        catch (ArgumentException)
        {
            await client.SendTextMessageAsync(
                        chat.Id,
                        Strings.NotValidMessageText);
        }
        catch (Exception)
        {
            await client.SendTextMessageAsync(
                         chat.Id,
                         Strings.CommonExeptionText);
        } 
    }
    private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

}
