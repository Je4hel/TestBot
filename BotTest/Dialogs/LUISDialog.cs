using BotTest.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BotTest.Dialogs
{
    [Serializable]
    [LuisModel("de65b1a3-e6fa-426c-9193-934a76c20bfa", "b0ac74feaf24454994a820b87905f21b")]
    public class LUISDialog : LuisDialog<MessageBox>
    {
        public readonly BuildFormDelegate<MessageBox> _configureMessageBox;

        public LUISDialog(BuildFormDelegate<MessageBox> configureMessageBox)
        {
            _configureMessageBox = configureMessageBox;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Désolé, je n'ai pas compris ce que vous vouliez dire");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), LoopBackCallback);
        }

        [LuisIntent("SendMessage")]
        public async Task SendMessage(IDialogContext context, LuisResult result)
        {
            // Check if the user has already configured its message box
            bool isBoxConfigured = false;
            context.UserData.TryGetValue<bool>(Constants.UserData.IsBoxConfigured, out isBoxConfigured);

            if (!isBoxConfigured)
            {
                // Configure box
                context.Call(new FormDialog<MessageBox>(new MessageBox(), this._configureMessageBox, FormOptions.PromptInStart), ConfigurationCallback);
            }
            else
            {
                // Send message
                await context.PostAsync("Votre Message Box est déjà configurée. Le message a été envoyé !");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("ConfigureMessageBox")]
        public async Task ConfigureMessageBox(IDialogContext context, LuisResult result)
        {
            context.Call(new FormDialog<MessageBox>(new MessageBox(), this._configureMessageBox, FormOptions.PromptInStart), async (c, r) =>
            {
                
            });

            context.Wait(MessageReceived);
        }

        private async Task LoopBackCallback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

        private async Task ConfigurationCallback(IDialogContext context, IAwaitable<object> result)
        {
            var r = await result;

            await context.PostAsync("Message Box configurée !");
            context.Wait(MessageReceived);
        }
    }
}