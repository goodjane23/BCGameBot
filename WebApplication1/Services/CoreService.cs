using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WebApplication1.Services;

public class CoreService : IHostedService
{
    private readonly ITelegramBotClient сlient;
    private readonly BCGameService game;

    public CoreService(
        ITelegramBotClient сlient, 
        BCGameService bCGameService)        
    {
        this.сlient = сlient;
        this.game = bCGameService;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            сlient.StartReceiving(UpdateHandler, 
                ErrorHandler,
                cancellationToken:cancellationToken);
            game.GenerateNum();

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

                }
                if (message.Text.Equals("/rules", StringComparison.OrdinalIgnoreCase))
                {
                    await client.SendTextMessageAsync(
                        chat.Id,
                        Strings.RulesText);
                }
                var stepResult = .GetMessage(update.Message.Text);
                var messageFin = string.Empty;
            }
        }
        catch (Exception)
        {

            throw;
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
