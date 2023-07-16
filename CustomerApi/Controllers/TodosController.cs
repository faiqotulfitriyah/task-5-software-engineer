// TodosController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json; 

[ApiController]
[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TodosController> _logger;

    public TodosController(IHttpClientFactory httpClientFactory, ILogger<TodosController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos");
            response.EnsureSuccessStatusCode();
            var todos = await response.Content.ReadFromJsonAsync<dynamic>(); // Ubah metode ReadAsAsync menjadi ReadFromJsonAsync

            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching todos.");
            return StatusCode(500, "An error occurred while fetching todos.");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetTodoById(int id)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = httpClient.GetAsync($"https://jsonplaceholder.typicode.com/todos/{id}").Result;
            response.EnsureSuccessStatusCode();
            var todo = response.Content.ReadFromJsonAsync<dynamic>().Result; 

            return Ok(todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching todo.");
            return StatusCode(500, "An error occurred while fetching todo.");
        }
    }
}