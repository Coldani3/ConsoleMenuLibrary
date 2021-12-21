using System;
using System.Collections.Generic;

namespace ConsoleMenuLibrary
{
	public class TextInputMenu : IMenu
	{
		protected Tuple<int[], string>[] InputAreas;
		protected string[] PreTextMessages;
		private int InputAreaIndex = 0;
		private int CursorIndex = 0;
		protected string ErrorMessage = "";

		public TextInputMenu(int[][] inputAreas, string[] preTextMessages)
		{
			this.PreTextMessages = new string[preTextMessages.Length];
			this.InputAreas = new Tuple<int[], string>[inputAreas.Length];

			for (int i = 0; i < inputAreas.Length; i++) 
			{
				this.InputAreas[i] = Tuple.Create<int[], string>(inputAreas[i], "");
				this.PreTextMessages[i] = preTextMessages[i];
			}
		}

		public virtual void OnInput(ConsoleKeyInfo input, MenuManager manager)
		{
			//manual text editor stuff wooo
			//can't wait for this to break horribly
			string currString = this.InputAreas[this.InputAreaIndex].Item2;
			switch (input.Key)
			{
				case ConsoleKey.UpArrow:
					if (this.InputAreaIndex - 1 >= 0) 
					{
						this.InputAreaIndex--;
						if (this.CursorIndex > this.InputAreas[this.InputAreaIndex].Item2.Length) 
						{
							this.CursorIndex = this.InputAreas[this.InputAreaIndex].Item2.Length;
						}
					}

					break;
				case ConsoleKey.Tab:
					if ((input.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift) 
					{
						goto case ConsoleKey.UpArrow;
					}
					else 
					{
						goto case ConsoleKey.DownArrow;
					}
				case ConsoleKey.DownArrow:
					if (this.InputAreaIndex + 1 < this.InputAreas.Length) 
					{
						this.InputAreaIndex++;
						if (this.CursorIndex > this.InputAreas[this.InputAreaIndex].Item2.Length) 
						{
							this.CursorIndex = this.InputAreas[this.InputAreaIndex].Item2.Length;
						}
					}
					break;
				//navigating in a text entry
				case ConsoleKey.LeftArrow:
					if (this.CursorIndex > 0) 
					{
						this.CursorIndex--;
					}
					break;
				case ConsoleKey.RightArrow:
					if (this.CursorIndex < currString.Length) 
					{
						this.CursorIndex++;
					}
					break;
				case ConsoleKey.Backspace:
					//you can't delete nothing
					if (this.CursorIndex > 0) 
					{
						this.InputAreas[this.InputAreaIndex] = Tuple.Create(this.InputAreas[this.InputAreaIndex].Item1, currString.Remove(this.CursorIndex - 1, 1));
						goto case ConsoleKey.LeftArrow;
					}
					break;
				case ConsoleKey.Enter:
					if (!this.Submit())
					{
						this.InputAreaIndex = 0;
						this.CursorIndex = 0;

						if (this.ErrorMessage == "") 
						{
							this.ErrorMessage = "An unexpected error with your inputs was found! Check the types of the inputs and try again.";
						}
					}

					break;
				default:
					if (input.KeyChar == ']' && (input.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt) 
					{
						manager.ChangeMenu(manager.GetPreviousMenu());
					}

					this.InputAreas[InputAreaIndex] = Tuple.Create(this.InputAreas[this.InputAreaIndex].Item1, currString.Insert(this.CursorIndex, input.KeyChar.ToString()));

					if (this.CursorIndex <= currString.Length) 
					{
						this.CursorIndex++;
					}

					break;
			}

		}

		public string GetInputByIndex(int inputAreaIndex)
		{
			return this.InputAreas[inputAreaIndex].Item2;
		}

		//NOTE: set ErrorMessage here to the appropriate message
		public virtual bool Submit()
		{
			return true;
		}

		public virtual void Display()
		{
			Console.CursorVisible = false;
			Console.SetCursorPosition(0, 0);
			Console.WriteLine(ErrorMessage.Length > 0 ? ErrorMessage : "Press Enter to submit and use arrow keys to navigate! \nPress Alt+] to exit! Press Esc to close the program!");
			
			for (int i = 0; i < this.InputAreas.Length; i++)
			{
				int[] coords = this.InputAreas[i].Item1;
				Console.SetCursorPosition(coords[0], coords[1]);
				Console.Write(this.PreTextMessages[i] + " " + this.InputAreas[i].Item2);
			}

			int[] startCoords = this.InputAreas[InputAreaIndex].Item1;
			int inputStart = startCoords[0] + this.PreTextMessages[InputAreaIndex].Length + 1;
			Console.SetCursorPosition(inputStart + CursorIndex, startCoords[1]);
			Console.CursorVisible = true;
		}
	}
}