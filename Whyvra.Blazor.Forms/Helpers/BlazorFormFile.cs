using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace Whyvra.Blazor.Forms.Helpers
{
    public class BlazorFormFile : IFormFile
    {
        private readonly IBrowserFile _file;

        public BlazorFormFile(IBrowserFile file)
        {
            _file = file;
        }

        public string ContentType => _file.ContentType;

        public string ContentDisposition => throw new InvalidOperationException();

        public IHeaderDictionary Headers => throw new InvalidOperationException();

        public long Length => _file.Size;

        public string Name => _file.Name;

        public string FileName => _file.Name;

        public void CopyTo(Stream target)
        {
            using var source = _file.OpenReadStream();
            source.CopyTo(target);
        }

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            await using var source = _file.OpenReadStream(cancellationToken: cancellationToken);
            await source.CopyToAsync(target, cancellationToken);
        }

        public Stream OpenReadStream()
        {
            return _file.OpenReadStream();
        }
    }
}
