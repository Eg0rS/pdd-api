using Microsoft.AspNetCore.Mvc;
using Minio.AspNetCore;
using Minio.DataModel.Args;


namespace pdd_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : BaseController
{
    private ILogger<FileController> logger;
    private readonly IMinioClientFactory minioClientFactory;


    public FileController(ILogger<FileController> logger, IMinioClientFactory minioClient)
    {
        this.logger = logger;
        this.minioClientFactory = minioClient;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        // var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", file.FileName);
        // await using (var stream = new FileStream(path, FileMode.Create))
        // {
        //     await file.CopyToAsync(stream);
        // }

        try
        {
            using (var minio = minioClientFactory.CreateClient())
            {
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket("requests")
                    .WithObject(file.FileName)
                    .WithObjectSize(file.Length)
                    .WithStreamData(file.OpenReadStream());
                var res = await minio.PutObjectAsync(putObjectArgs);
                if (res.Etag == null)
                {
                    return BadRequest("Failed to upload file to Minio");
                }

                return Ok(res.Etag);
            }
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
        finally
        {
           
        }
    }
}
