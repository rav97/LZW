using System.Collections.Generic;

namespace FileStats.CharacterPresence
{
	public class CharacterPresenceData : ICharacterPresenceData
	{
		public string Letter { get; internal set; }
		public int Count { get; internal set; }
		public string Percentage { get; internal set; }
	}

	public interface ICharacterPresenceData
	{
		string Letter { get; }
		int Count { get; }
		string Percentage { get; }
	}

	public class CharactersPresenceData : ICharactersPresenceData
	{
		public IEnumerable<CharacterPresenceData> Data { get; internal set; }
		public double Entropy { get; internal set; }

	}

	public interface ICharactersPresenceData
	{
		IEnumerable<CharacterPresenceData> Data { get; }
		double Entropy { get; }
	}
}