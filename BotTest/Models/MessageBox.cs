using Microsoft.Bot.Builder.FormFlow;
using System;

namespace BotTest.Models
{
    [Serializable]
    public class MessageBox
    {
        [Describe("numéro d'identification")]
        [Prompt("Quel est le {&} de votre Message Box ?")]
        public int Id { get; set; }

        [Describe("surnom")]
        [Prompt("Quel {&} voulez-vous lui donner ?")]
        public string BoxName { get; set; }

        public static IForm<MessageBox> BuildForm()
        {
            return new FormBuilder<MessageBox>()
                .Message("Mmmh... Je ne connais pas votre Message Box. J'ai besoin de quelques informations.")
                .Build();
        }
    }
}