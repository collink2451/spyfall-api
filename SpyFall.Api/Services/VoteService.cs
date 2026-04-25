using System.Collections.Concurrent;

namespace SpyFall.Api.Services;

public class VoteService
{
	private readonly ConcurrentDictionary<string, (int AccusedId, Dictionary<int, bool> Votes)> _votes = new();

	public void StartVote(string code, int accusedId)
	{
		_votes[code] = (accusedId, []);
	}

	public bool TryGetVote(string code, out (int AccusedId, Dictionary<int, bool> Votes) voteState)
	{
		return _votes.TryGetValue(code, out voteState);
	}

	public void CastVote(string code, int playerId, bool guilty)
	{
		if (_votes.TryGetValue(code, out (int AccusedId, Dictionary<int, bool> Votes) state))
			state.Votes[playerId] = guilty;
	}

	public void RemoveVote(string code)
	{
		_votes.TryRemove(code, out _);
	}
}
