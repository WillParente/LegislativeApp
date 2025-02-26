using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LegislativeApp.Domain.Entities;

public class Legislator
{
    public Legislator(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public Legislator() { }

    [Key]
    [Name("id")]
    public int Id { get; set; }
    [Name("name")]
    public string Name { get; set; }
}