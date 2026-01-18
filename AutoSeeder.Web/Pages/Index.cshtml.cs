using AutoSeeder.ServiceContracts.Seed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    private readonly ISeedService _seedService;

    public IndexModel(ISeedService seedService)
    {
        _seedService = seedService;
    }

    [BindProperty]
    public string SchemaText { get; set; } = string.Empty;

    public string SeedSql { get; private set; } = string.Empty;

    public void OnGet()
    {
        // empty page
    }

    public void OnPost()
    {
        if (string.IsNullOrWhiteSpace(SchemaText))
        {
            SeedSql = "-- Schema is empty";
            return;
        }

        //  so far can't work with IDENTITY e.g (IDENTITY(1,1))

        try
        {
            SeedSql = _seedService.Create(SchemaText);
        }
        catch (Exception ex)
        {
            SeedSql = $"-- Error:\n-- {ex.Message}";
        }
    }
}