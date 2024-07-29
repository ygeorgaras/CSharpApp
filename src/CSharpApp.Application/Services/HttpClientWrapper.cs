using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Services;

public class HttpClientWrapper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpClientWrapper> _logger;

    public HttpClientWrapper(ILogger<HttpClientWrapper> logger, 
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    /// <summary>
    /// This method will try to retrieve a record if it exists.
    /// </summary>
    /// <param name="id">ID we are searching for.</param>
    /// <returns>Existing record or null if record doesn't exist.</returns>
    public async Task<T> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to GET from {endpoint}");
            return default;
        }
    }

    /// <summary>
    /// This method will get a list of all records existing at the endpoint.
    /// </summary>
    /// <returns>All records. Or an empty list if no records exist.</returns>
    public async Task<ReadOnlyCollection<T>> GetAllAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            var records = await response.Content.ReadFromJsonAsync<List<T>>();
            return new ReadOnlyCollection<T>(records);
        }

        _logger.LogError($"No records exist at {endpoint}.");
        return new ReadOnlyCollection<T>(new List<T>());
    }

    /// <summary>
    /// Attempts to add a new record.
    /// </summary>
    /// <param name="endpoint">Endpoint to add the record to.</param>
    /// <param name="record">New record we want to add.</param>
    /// <returns></returns>
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest record)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, record);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
        else
        {
            _logger.LogError($"Failed to POST to {endpoint}. Status Code: {response.StatusCode}");
            return default;
        }
    }

    /// <summary>
    /// Attempts to edit an existing record.
    /// </summary>
    /// <param name="endpoint">Endpoint to edit the record to.</param>
    /// <param name="record">New record we want to use to overwrite existing record.</param>
    /// <param name="id">Id of the record we want to edit.</param>
    /// <returns></returns>
    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest record, int id)
    {
        var result = await _httpClient.GetAsync($"{endpoint}/{id}");

        if (result.IsSuccessStatusCode)
        {
            var response = await _httpClient.PutAsJsonAsync($"{endpoint}/{id}", record);
            return await response.Content.ReadFromJsonAsync<TResponse>(); ;
        }
        else
        {
            return default(TResponse?);
        }
    }
    /// <summary>
    /// This will search using the input ID if the record exists. If it exists then it will issue a Delete to the API.
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<HttpStatusCode> DeleteByIdAsync(string endpoint, int id)
    {
        var response = await _httpClient.GetAsync($"{endpoint}/{id}");

        if (response.IsSuccessStatusCode)
        {
            var deleteResponse = await _httpClient.DeleteAsync($"{endpoint}/{id}");
            return deleteResponse.StatusCode;
        }
        else
        {
            _logger.LogError($"Unable to delete record with ID:{id}");
        }
        return response.StatusCode;
    }

}

