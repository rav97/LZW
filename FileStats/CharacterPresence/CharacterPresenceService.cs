using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileStats.CharacterPresence
{
	public class CharacterPresenceService : ICharacterPresenceService
	{
		public CharactersPresenceData Calculate(string text)
		{
			var entropy = 0.0;
			var abb = text.GroupBy(character => character)
				.Select(character =>
				{
					var characterCount = character.Count();
					var characterPercentage = (double)characterCount / text.Length;
					var log = Math.Log2(1 / characterPercentage);

					entropy += characterPercentage * log;

					return new CharacterPresenceData
					{
						Letter = Map(character.Key),
						Count = characterCount,
						Percentage = characterPercentage.ToString("P2", CultureInfo.InvariantCulture),
					};
				})
				.OrderByDescending(x => x.Count).ToList();

			return new CharactersPresenceData
			{
				Data = abb,
				Entropy = entropy,
			};
		}

		private string Map(char character)
		{
			if (!char.IsWhiteSpace(character))
				return character.ToString();

			if (character == ' ')
				return "space";
			if (character == '\t')
				return "tab";
			if (character == '\n')
				return "new line";

			return $"Nieznany {Convert.ToByte(character)}";
		}

	}

	public interface ICharacterPresenceService
	{
		CharactersPresenceData Calculate(string text);
	}
}
