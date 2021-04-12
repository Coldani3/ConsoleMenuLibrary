using System;

namespace ConsoleMenuLibrary
{
	public class KeyCombo
	{
		ConsoleKey Letter;
		ConsoleModifiers[] Modifiers;
		public KeyCombo(ConsoleKey letter, params ConsoleModifiers[] modifiers)
		{
			this.Letter = letter;
			this.Modifiers = modifiers;

		}

		public bool Matches(ConsoleKeyInfo input)
		{
			if (!(input.Key == this.Letter))
			{
				return false;
			}

			foreach (ConsoleModifiers modifier in this.Modifiers)
			{
				if (!((input.Modifiers & modifier) == modifier))
				{
					return false;
				}
			}

			return true;
		}
	}
}