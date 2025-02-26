using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LegislativeApp.Domain.Entities;

public class Vote
{
    public Vote(int id, int billId)
    {
        Id = id;
        BillId = billId;
    }

    public Vote() { }

    [Key]
    [Name("id")]
    public int Id { get; set; }
    [Name("bill_id")]
    public int BillId { get; set; }
}