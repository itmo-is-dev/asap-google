namespace Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Models;

/// <summary>
///     SheetId is a unique identifier that exists inside specific spreadsheet like a separate "page"
/// </summary>
public record struct SheetId(int Value);