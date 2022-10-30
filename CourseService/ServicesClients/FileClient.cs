using System.Net;
using System.Net.Http.Headers;

using Mapster;

using src.Dtos;


namespace src.ServicesClients;

/// <summary>
/// Client for File API
/// </summary>
public class FileClient {
  private readonly HttpClient _httpClient;
  private readonly string _token;

  /// <summary>
  /// Constructor of File API's client
  /// </summary>
  /// <param name="client"><see cref="HttpClient"/>'s object</param>
  /// <param name="token">HTTP Authorization token</param>
  public FileClient(HttpClient client, string? token) {
    _httpClient = client;
    _token = token ?? throw new ArgumentNullException(nameof(token), "Token must not be null");
  }

  /// <summary>
  /// Uploads file to File API 
  /// </summary>
  /// <returns>Guid of uploaded file</returns>
  public async Task<Guid?> UploadFile(MultipartFormDataContent file) {
    // TODO: Придумать способ динамически получать урл микросервиса
    var message = new HttpRequestMessage(HttpMethod.Get, "https://localhost:8080/api/File");
    message.Content = file;
    message.Headers.Authorization = new AuthenticationHeaderValue(_token);

    var result = await _httpClient.SendAsync(message);

    if (result.StatusCode != HttpStatusCode.Created) {
      return null;
    }

    var resultBody = await result.Content.ReadAsStringAsync();
    var attachment = resultBody.Adapt<AttachmentDto>();
    return attachment.Id;
  }
}