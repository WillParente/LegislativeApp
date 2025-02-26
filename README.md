# LegislativeApp

## Implementation Overview

This application processes legislative data, allowing for the generation of reports based on CSV input.

## Responses to Questions

1. **Discuss your solution’s time complexity. What tradeoffs did you make?**
   - The time complexity of reading and writing CSV files is O(n), where n is the number of records. The tradeoff made is between simplicity and performance; while the current implementation is straightforward, it may not be optimal for very large datasets. I am processing data in parallel, which can increase machine resource consumption but improves agility. Another tradeoff is that I first read all the data and then use LINQ with lambda expressions to perform queries, similar to SQL Server, leveraging my expertise in SQL.
   - A possible improvement would be to read all CSV files and process everything in memory to avoid multiple calls to the same CSV file.

2. **How would you change your solution to account for future columns that might be requested, such as “Bill Voted On Date” or “Co-Sponsors”?**
   - To accommodate future columns, I would update the entity classes (e.g., `Bill`, `Legislator`) to include new properties. The `CsvRepository` methods would automatically handle these new properties during read and write operations. Additionally, I would ensure that the report generation logic in `ReportService` is updated to include these new fields in the reports.

3. **How would you change your solution if instead of receiving CSVs of data, you were given a list of legislators or bills that you should generate a CSV for?**
   - I would create new methods in the `LegislativeRepository` to accept lists of legislators or bills. These methods would convert the lists into CSV format using the existing `CsvRepository` methods for writing records.

4. **How long did you spend working on the assignment?**
   - The total time spent on this assignment was approximately 4 hours, including research, implementation, and testing.

## Conclusion
This README provides an overview of the implementation and responses to the specified questions. Future enhancements can be made to improve performance and accommodate additional data formats.
