
using System;
using System.Windows.Input;
namespace NabbR.ViewModels
{
    public class CommandViewModel
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="CommandViewModel"/> class from being created.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="command">The command.</param>
        CommandViewModel(String caption, ICommand command)
        {
            this.Caption = caption;
            this.Command = command;
        }
        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        public String Caption { get; private set; }
        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public ICommand Command { get; private set; }
        /// <summary>
        /// Gets a value indicating whether whether or not this <see cref="CommandViewModel"/> should be the default button.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="CommandViewModel"/> should be the default button; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsDefault { get; private set; }


        /// <summary>
        /// Creates a <see cref="CommandViewModel"/>.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static CommandViewModel Create(String caption, ICommand command)
        {
            return new CommandViewModel(caption, command);
        }
        /// <summary>
        /// Creates a default <see cref="CommandViewModel"/>.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static CommandViewModel CreateDefault(String caption, ICommand command)
        {
            CommandViewModel vm = Create(caption, command);
            vm.IsDefault = true;
            return vm;
        }
    }
}
