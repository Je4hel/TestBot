using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace BotTest.Dialogs
{
    public class GreetingDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Bonjour, {context.Activity.From.Name} ! Comment puis-je vous aider ?");
            context.Done("");
        }
    }
}