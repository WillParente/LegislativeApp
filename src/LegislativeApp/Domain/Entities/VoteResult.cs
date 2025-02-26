using CsvHelper.Configuration.Attributes;
using LegislativeApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LegislativeApp.Domain.Entities;

public class VoteResult
{
    public VoteResult(int id, int legislatorId, int voteId, VoteType voteType)
    {
        Id = id;
        LegislatorId = legislatorId;
        VoteId = voteId;
        VoteType = voteType;
    }

    public VoteResult() { }

    [Key]
    [Name("id")]
    public int Id { get; set; }
    
    [Name("legislator_id")]
    public int LegislatorId { get; set; }
    
    [Name("vote_id")]
    public int VoteId { get; set; }
    
    [Name("vote_type")]
    public VoteType VoteType { get; set; }
}