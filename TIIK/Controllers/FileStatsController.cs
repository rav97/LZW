using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FileStats.CharacterPresence;
using FileStats.LZW;
using Microsoft.AspNetCore.Mvc;

namespace TIIK.Controllers
{
	[ApiController]
	public class FileStatsController : ControllerBase
	{
		public FileStatsController(ICharacterPresenceService characterPresenceService)
		{
			this.characterPresenceService = characterPresenceService;
		}

		[HttpPost("api/entropy")]
		public ICharactersPresenceData Entropy([FromBody]RequestData request)
		{
			return characterPresenceService.Calculate(request.Data);
		}

		[HttpPost("api/encode")]
		public EncodeResponse LzwEncode([FromBody] RequestData request)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();

			var encoded = LZW.Encode(request.Data);
			stopWatch.Stop();

			var inputLength = request.Data.Length;
			var encodedLength = encoded.Count * 2;

			return new EncodeResponse
			{
				Encoded = encoded.ConvertAll(x => x.ToString()),
				InputLength = inputLength,
				EncodedLength = encoded.Count * 2,
				CompressionRatio = (double) encodedLength / inputLength,
				ElapsedTime = stopWatch.Elapsed,
			};
		}

		[HttpPost("api/decode")]
		public DecodeResponse LzwDecode([FromBody] RequestData request)
		{
			var data = request.Data.Split(' ').ToList().ConvertAll(Convert.ToUInt16);

			var stopWatch = new Stopwatch();
			stopWatch.Start();

			var decodedData = LZW.Decode(data);
			stopWatch.Stop();

			return new DecodeResponse
			{
				Decoded = decodedData,
				ElapsedTime = stopWatch.Elapsed,
			};
		}

		private static byte[] ListToBytes(IReadOnlyCollection<ushort> list)
		{
			var bytes = new byte[list.Count * 2];

			var i = 0;

			foreach (var e in list)
			{
				var b = BitConverter.GetBytes(e);
				bytes[i] = b[0];
				bytes[i + 1] = b[1];

				i += 2;
			}
			return bytes;
		}

		private readonly ICharacterPresenceService characterPresenceService;
	}

	public class RequestData
	{
		public string Data { get; set; }
	}

	public class DecodeResponse
	{
		public string Decoded { get; set; }
		public TimeSpan ElapsedTime { get; set; }
	}

	public class EncodeResponse
	{
		public IList<string> Encoded { get; set; }
		public double CompressionRatio { get; set; }
		public int InputLength { get; set; }
		public int EncodedLength { get; set; }
		public TimeSpan ElapsedTime { get; set; }
	}
}
