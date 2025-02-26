using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LegislativeApp.Domain.Entities;

public class Bill
{
    public Bill(int id, string title, int? sponsorId)
    {
        Id = id;
        Title = title;
        SponsorId = sponsorId;
    }

    public Bill() { }

    [Key]
    [Name("id")]
    public int Id { get; set; }
    [Name("title")]
    public string Title { get; set; }
    [Name("sponsor_id")]
    public int? SponsorId { get; set; }
}