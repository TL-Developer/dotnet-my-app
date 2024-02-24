using dotnet_my_app.Entities;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace dotnet_my_app.Controllers;

[ApiController]
// [Route("[controller]")]
[Route("advisor")]
public class AdvisorController : ControllerBase
{
    private readonly ILogger<AdvisorController> _logger;
    private readonly IConfiguration _configuration;

    public AdvisorController(ILogger<AdvisorController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("{advisorLogin}")]
    public async Task<ActionResult<string>> Get(string advisorLogin)
    {
        using var connection = new MySqlConnection(_configuration.GetValue<string>("ConnectionString"));
        await connection.OpenAsync();

        string sql = $"SELECT * FROM advisors WHERE name like '{advisorLogin}%' LIMIT 10;";
        using var command = new MySqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var value = reader.GetValue(2);
            Console.WriteLine(value);

            return Ok(new {
                result = value
            });
        }

        reader.Close();

        return NotFound();
    }
}
