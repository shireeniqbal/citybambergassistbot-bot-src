// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.QnA.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Net.Mail;
namespace Microsoft.BotBuilderSamples.Dialog
{
    /// <summary>
    /// QnAMaker action builder class
    /// </summary>
    public class QnAMakerBaseDialog : QnAMakerDialog
    {
        // Dialog Options parameters
        public const string DefaultNoAnswer = "Thank you for your query we will update you very soon.";
        public const string DefaultCardTitle = "Did you mean:";
        public const string DefaultCardNoMatchText = "None of the above.";
        public const string DefaultCardNoMatchResponse = "Thanks for the feedback.";

        private readonly IBotServices _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="QnAMakerBaseDialog"/> class.
        /// Dialog helper to generate dialogs.
        /// </summary>
        /// <param name="services">Bot Services.</param>
        public QnAMakerBaseDialog(IBotServices services): base()
        {
            this._services = services;
        }

        protected async override Task<IQnAMakerClient> GetQnAMakerClientAsync(DialogContext dc)
        {
            return this._services?.QnAMakerService;
        }

        protected override Task<QnAMakerOptions> GetQnAMakerOptionsAsync(DialogContext dc)
        {
            return Task.FromResult(new QnAMakerOptions
            {
                ScoreThreshold = DefaultThreshold,
                Top = DefaultTopN,
                QnAId = 0,
                RankerType = "Default",
                IsTest = false
            });
        }

        protected async override Task<QnADialogResponseOptions> GetQnAResponseOptionsAsync(DialogContext dc)
        {
            var noAnswer = (Activity)Activity.CreateMessageActivity();
            noAnswer.Text = DefaultNoAnswer;

            var cardNoMatchResponse = (Activity)MessageFactory.Text(DefaultCardNoMatchResponse);
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("shireeniqbal02@gmail.com");
                mail.To.Add("shireeniqbal02@yahoo.com");
                mail.Subject = "Chatbot Message";
                mail.Body = "You have got a chat in chatbot";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("shireeniqbal02@gmail.com", "jrxgplzsaifdstcz");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch
            {
                
            }


            var responseOptions = new QnADialogResponseOptions
            {
                ActiveLearningCardTitle = DefaultCardTitle,
                CardNoMatchText = DefaultCardNoMatchText,
                NoAnswer = noAnswer,
                CardNoMatchResponse = cardNoMatchResponse,
            };

            return responseOptions;
        }
    }
}
