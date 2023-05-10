﻿namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model
{

    /// <summary>
    /// Класс-инструкция для конвертации строки в заданный тип
    /// </summary>
    /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
    public sealed class ConvertRule<TOut> : ConvertRule
    {
        /// <summary>
        /// Инструкция конвертации заданной входной строки в целевой тип конвертации.
        /// </summary>
        public Func<string, ConvertResult<TOut>> Converter { get; set; }

        /// <summary>
        /// Конструктор класса-инструкции с целевым типом конвертации и инструкцией конвертации.
        /// </summary>
        /// <param name="converter">Инструкция конвертации заданной входной строки в целевой тип конвертации</param>
        public ConvertRule(Func<string, ConvertResult<TOut>> converter) : base(typeof(TOut))
        {
            Converter = converter;
        }
    }
}