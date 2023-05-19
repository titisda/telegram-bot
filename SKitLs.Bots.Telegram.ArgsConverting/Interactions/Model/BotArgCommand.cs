﻿using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    public class BotArgCommand<TArg> : DefaultCommand, IArgedAction<TArg, SignedMessageTextUpdate> where TArg : notnull, new()
    {
        public char SplitToken { get; set; }
        public BotArgedInteraction<TArg, SignedMessageTextUpdate> ArgAction { get; set; }

        public BotArgCommand(string @base, BotArgedInteraction<TArg, SignedMessageTextUpdate> action, char splitToken = ';') : base(@base)
        {
            Action = MiddleAction;
            ArgAction = action;
            SplitToken = splitToken;
        }

        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => ActionBase == update.Text[..update.Text.IndexOf(SplitToken)];
        public ConvertResult<TArg> DeserializeArgs(SignedMessageTextUpdate update, IArgsSerilalizerService serilalizer)
            => serilalizer.Deserialize<TArg>(update.Text[(update.Text.IndexOf(SplitToken) + 1)..], SplitToken);
        public string SerializeArgs(TArg data, IArgsSerilalizerService serialize) => serialize.Serialize(data, SplitToken);
        public string GetSerializedData(TArg data, IArgsSerilalizerService serialize) => ActionBase + SerializeArgs(data, serialize);

        private async Task MiddleAction(IBotAction<SignedMessageTextUpdate> trigger, SignedMessageTextUpdate update)
        {
            var argedAction = trigger as IArgedAction<TArg, SignedMessageTextUpdate> ?? throw new Exception();
            var argService = update.Owner.ResolveService<IArgsSerilalizerService>();
            var args = DeserializeArgs(update, argService).Value;
            await ArgAction.Invoke(argedAction, args, update);
        }
    }
}