using FileStats.CharacterPresence;
using FluentAssertions;
using NUnit.Framework;

namespace FileStatsTests.CharacterPresence
{
	public class CharacterPresenceServiceTests
	{
		[Test]
		public void Calculate_WhenValid_ReturnsCorrectData()
		{
			const string text = "Bu\nga\tbuga";

			var result = characterPresenceService.Calculate(text);

			result.Data.Should().BeEquivalentTo(new[]
			{
				new {Count = 2, Letter = "u", Percentage = "20.00 %",},
				new {Count = 2, Letter = "g", Percentage = "20.00 %",},
				new {Count = 2, Letter = "a", Percentage = "20.00 %",},
				new {Count = 1, Letter = "B", Percentage = "10.00 %",},
				new {Count = 1, Letter = "b", Percentage = "10.00 %",},
				new {Count = 1, Letter = "tab", Percentage = "10.00 %",},
				new {Count = 1, Letter = "new line", Percentage = "10.00 %",},
			});
		}

		[Test]
		public void Calculate_WhenValid_ReturnsCorrectEntropy()
		{
			const string text = "ab";

			var result = characterPresenceService.Calculate(text);

			result.Entropy.Should().BeGreaterOrEqualTo(1.0);
		}

		[SetUp]
		public void SetUp()
		{
			characterPresenceService = new CharacterPresenceService();
		}

		private ICharacterPresenceService characterPresenceService;
	}
}