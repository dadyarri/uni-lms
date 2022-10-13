using Microsoft.AspNetCore.Mvc;

using src.Models;


namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase {
  [HttpPost]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  public async Task<ActionResult<Attachment>> UploadFile() {
    return Created(
      "api/File",
      new Attachment {
        Checksum = "bb078fd350bdfbf74975735c3c7734d6",
        FileUrl = "https://www.dadyarri.ru/images/index/webp/avatar.webp",
        VisibleName = "dadyarri",
      }
    );
  }
}