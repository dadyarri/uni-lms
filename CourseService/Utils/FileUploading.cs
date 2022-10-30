namespace src.Utils;

/// <summary>
/// Set of utils to work with files
/// </summary>
public static class FileUploading {
  /// <summary>
  /// Method, which will prepare <see cref="MultipartFormDataContent"/> from <see cref="IFormFile"/>
  /// </summary>
  /// <returns>Prepared <see cref="IFormFile"/></returns>
  public static async Task<MultipartFormDataContent> BuildFormDataContent(IFormFile file) {
    var multipartFormData = new MultipartFormDataContent();
    var ms = new MemoryStream();
    await file.CopyToAsync(ms);
    multipartFormData.Add(
      new ByteArrayContent(ms.ToArray()),
      file.FileName,
      file.FileName
    );

    return multipartFormData;
  }
}