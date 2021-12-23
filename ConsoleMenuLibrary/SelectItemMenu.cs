using System;
using System.Collections.Generic;

namespace ConsoleMenuLibrary
{
	/***
	 * <summary>A menu that acts as a simple up and down arrow key select. This is written so that you don't need to extend it to
	 * modify its behavious and aesthetics but can if you need to.</summary>
	 */
	public class SelectItemMenu : IMenu
	{
		/// <summary>
		/// The currently selected menu item index.
		/// </summary>
		protected int SelectedIndex = 0;
		protected Action[] ActionsForIndex;
		protected string[] Options;
		public ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;
		public ConsoleColor DefaultForegroundColor = ConsoleColor.White;
		public ConsoleColor HighlightBackgroundColor = ConsoleColor.White;
		public ConsoleColor HighlightForegroundColor = ConsoleColor.Black;
		public int OptionsListStartX = 2;
		public int OptionsListStartY = 3;
		public int OptionSpacing = 1;
		public char SelectionIndicator = '>';

		/***
		 * <param name="options">The names of the options available to be selected in the menu.</param>
		 * <param name="actions">Action objects that describe what selecting an option of the name specified by the equivalent
		 * index in <paramref name="options"/>. IE. the Action at index 0 will correspond to the option in 
		 * <paramref name="options"/> at index 0.</param>
		 */
		public SelectItemMenu(string[] options, Action[] actions)
		{
			this.ActionsForIndex = new Action[actions.Length];
			this.Options = new string[options.Length];

			for (int i = 0; i < options.Length; i++)
			{
				this.ActionsForIndex[i] = actions[i];
				this.Options[i] = options[i];
			}
		}

		public virtual void OnInput(ConsoleKeyInfo input, MenuManager manager)
		{
			switch (input.Key)
			{
				case ConsoleKey.DownArrow:
					if (this.SelectedIndex + 1 < this.Options.Length) 
					{
						this.SelectedIndex++;
					}

					break;

				case ConsoleKey.UpArrow:
					if (this.SelectedIndex - 1 >= 0) 
					{
						this.SelectedIndex--;
					}

					break;

				case ConsoleKey.Enter:
					this.ActionsForIndex[this.SelectedIndex]();
					break;
			}
		}

		public virtual void Display()
		{
			Console.SetCursorPosition(0, 0);
			Console.Write("Use Arrow Keys to Navigate! \nPress Esc to close the program!");

			int newlineAddition = 0;
			
			for (int i = 0; i < this.Options.Length; i++)
			{
				int y = this.OptionsListStartY + (i * this.OptionSpacing);
				Console.SetCursorPosition(this.OptionsListStartX, y + newlineAddition);
				string preOption = (i == this.SelectedIndex ? this.SelectionIndicator : ' ') + " ";

				Console.Write(preOption);

				if (i == this.SelectedIndex) 
				{
					Console.BackgroundColor = this.HighlightBackgroundColor;
					Console.ForegroundColor = this.HighlightForegroundColor;
				}
				
				string option = this.Options[i];
				int optionLength = option.Length;
				int maxLineLength = Console.WindowWidth - this.OptionsListStartX - preOption.Length;

				int newlinesForMenuOption = 0;
				int initNewlineAddition = newlineAddition;

				while (optionLength > maxLineLength)
                {
					Console.Write(option.Substring(0, maxLineLength));
					option = option.Substring(maxLineLength);
					optionLength = option.Length;
					newlineAddition++;
					newlinesForMenuOption++;
					Console.SetCursorPosition(this.OptionsListStartX + preOption.Length, y + initNewlineAddition + newlinesForMenuOption);
                }

				Console.Write(option);
				Console.BackgroundColor = this.DefaultBackgroundColor;
				Console.ForegroundColor = this.DefaultForegroundColor;
			}	
		}

        #region Color builder methods
		/*These methods are so that people who want to one line their menu instantiation can do so, while others can
		manually set the properties.*/
        public virtual SelectItemMenu SetHighlightBackgroundColor(ConsoleColor newHighlightBackgroundColor)
        {
			this.HighlightBackgroundColor = newHighlightBackgroundColor;
			return this;
        }

		public virtual SelectItemMenu SetHighlightForegroundColor(ConsoleColor newHighlightForegroundColor)
		{
			this.HighlightForegroundColor = newHighlightForegroundColor;
			return this;
		}

		public virtual SelectItemMenu SetDefaultBackgroundColor(ConsoleColor newDefaultBackgroundColor)
		{
			this.DefaultBackgroundColor = newDefaultBackgroundColor;
			return this;
		}

		public virtual SelectItemMenu SetDefaultForegroundColor(ConsoleColor newDefaultForegroundColor)
		{
			this.DefaultForegroundColor = newDefaultForegroundColor;
			return this;
		}

		public virtual SelectItemMenu SetOptionsListStart(int x, int y)
        {
			this.OptionsListStartX = x;
			this.OptionsListStartY = y;
			return this;
        }

		public virtual SelectItemMenu SetOptionsStartX(int x)
        {
			return this.SetOptionsListStart(x, this.OptionsListStartY);
        }

		public virtual SelectItemMenu SetOptionsStartY(int y)
		{
			return this.SetOptionsListStart(this.OptionsListStartX, y);
		}
		#endregion
	}
}